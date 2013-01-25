using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;
using System.Web.UI.WebControls.WebParts;
using System.Web;

namespace BrightcoveVideoCloudIntegration.Layouts.BrightcoveVideoCloudIntegration
{
    public partial class Chooser : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string chooserWeb = HttpUtility.UrlDecode(Request.Cookies["vcCurrentWeb"].Value);

                Page.Response.Expires = 0;
                Page.Response.Cache.SetNoStore();
                Page.Response.AppendHeader("Pragma", "no-cache");

                if (!string.IsNullOrEmpty(chooserWeb) && (chooserWeb != SPContext.Current.Web.Url)) {
                    string url = chooserWeb;

                    if (Request.QueryString[VideoCloudWebPart.QueryStringKeyAsyncChooserText] == "PlaylistId")
                    {
                        url += VideoPlayer.VideoPlayer.PlaylistChooser;
                    }
                    else {
                        url += VideoPlayer.VideoPlayer.VideoChooser;
                    }

                    Response.Redirect(url);
                }
            }
            catch { }
        }
    }
}
