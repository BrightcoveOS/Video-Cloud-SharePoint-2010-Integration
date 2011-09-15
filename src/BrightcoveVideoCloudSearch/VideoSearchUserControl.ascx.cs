using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.SharePoint;

using BrightcoveSDK.Entities;
using BrightcoveSDK;
using BrightcoveSDK.Entities.Containers;
using BrightcoveSDK.Media;
using BrightcoveSDK.JSON;
using BrightcoveSDK.UI;

using BrightcoveVideoCloudIntegration;

namespace BrightcoveVideoCloudIntegration.VideoSearch
{
    public partial class VideoSearchUserControl : VideoCloudWebPartUserControl
    {
        protected BCQueryResult result = null;
        protected int _itemCount = 0;
        protected int _itemsPerPage = 50;
        protected int _pageNumber = 0;

        public string PlayVideoLink;
        public string EditVideoLink;
        public string CustomFields;

        public override void Deprecated_Page_Load()
        {
            RenderResults();
        }

        public BCQueryResult GetResults(string query)
        {
            this._pageNumber = Util.ParseInt(Request.QueryString[Util.PagingKey]);

            if (this.bcApi != null)
            {
                if (string.IsNullOrEmpty(query))
                {
                    this.result = this.bcApi.FindAllVideos(this._itemsPerPage, BCSortByType.MODIFIED_DATE, BCSortOrderType.DESC, null, null, MediaDeliveryTypeEnum.DEFAULT, this._pageNumber, true);
                }
                else
                {
                    List<string> customFields = null;

                    if (!string.IsNullOrEmpty(CustomFields))
                    {
                        string[] fields = CustomFields.Split(",".ToCharArray());

                        customFields = new List<string>(fields.Length);

                        foreach (string field in fields)
                        {
                            customFields.Add(field);
                        }
                    }

                    //this.result = this.bcApi.SearchVideos(-1, null, Util.GetVideoSearchFields("*" + query + "*"), null, BCSortOrderType.DESC, false, 
                    //    null, null, MediaDeliveryTypeEnum.DEFAULT);
                    this.result = this.bcApi.FindVideosByText(query, this._itemsPerPage, BCSortByType.MODIFIED_DATE, BCSortOrderType.DESC, null, customFields, MediaDeliveryTypeEnum.DEFAULT, this._pageNumber, true);
                }

                if (this.result != null)
                {
                    this._itemCount = this.result.TotalCount;
                }
            }

            return this.result;
        }

        public void RenderResults()
        {
            string query = Request["keyword"];

            GetResults(query);

            // Render search results
            if (this.result == null)
            {
                if (this.IsPostBack)
                {
                    this.message.InnerHtml = "<p>No videos found</p>";
                }
            }
            else
            {
                this.message.InnerHtml = string.Empty;

                foreach (BCVideo item in result.Videos)
                {
                    this.message.InnerHtml += string.Format("<div class=\"result\" videoId=\"{0}\"><a class=\"videoLink\" href=\"" +
                        this.PlayVideoLink + "\"><div class=\"thumbnail\"><img src=\"{2}\" /></div>{1}</a><a class=\"editLink\" href=\"" +
                        this.EditVideoLink + "\"></a></div>",
                        item.id.ToString(), item.name, item.thumbnailURL);
                }

                this.message.InnerHtml += Util.GetPaging(this._pageNumber, this._itemsPerPage, this._itemCount, Request.FilePath + "?keyword=" + HttpUtility.UrlEncode(query));

                string output = string.Format(
                    @"<script language=""javascript"" type=""text/javascript"" charset=""utf-8"">/*<![CDATA[*/
                        var vcQueryResult = {0};
                        var vcIsAdmin = {1};
                        var vcDefaultPlayerId = {2};
                    /*]]>*/</script>", this.result.QueryResults[0].JsonResult, this.IsAdmin.ToString().ToLower(), this.DefaultVideoPlayerId.ToString());

                this.Controls.Add(new LiteralControl(output));
            }
        }
    }
}
