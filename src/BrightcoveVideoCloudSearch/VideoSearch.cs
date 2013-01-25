using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace BrightcoveVideoCloudIntegration.VideoSearch
{
    [ToolboxItemAttribute(false)]
    public class VideoSearch : VideoCloudWebPart
    {
        private string playVideoLink = "javascript:void OpenPopUpPage('./VideoPlayer.aspx?videoid={0}&autostart=true',null,675,420)";
        [WebBrowsable(true),
                    Category("Brightcove Configuration"),
                    Personalizable(PersonalizationScope.Shared),
                    DefaultValue("http://videocloud"),
                    WebDisplayName("Play Video Link"),
                    WebDescription("Page link formatter for playing a given video id.")]
        public string PlayVideoLink
        {
            get { return playVideoLink; }
            set { playVideoLink = value; }
        }

        private string editVideoLink = "javascript:void OpenPopUpPage('./VideoEditor.aspx?videoid={0}',null,730,780)";
        [WebBrowsable(true),
                    Category("Brightcove Configuration"),
                    Personalizable(PersonalizationScope.Shared),
                    DefaultValue("http://videocloud"),
                    WebDisplayName("Edit Video Link"),
                    WebDescription("Page link formatter for editing a given video id.")]
        public string EditVideoLink
        {
            get { return editVideoLink; }
            set { editVideoLink = value; }
        }

        private string customFields;
        [WebBrowsable(true),
                    Category("Brightcove Configuration"),
                    Personalizable(PersonalizationScope.Shared),
                    DefaultValue("http://videocloud"),
                    WebDisplayName("Custom Field(s) - comma separated"),
                    WebDescription("Custom fields available for your Brightcove Video Cloud account.")]
        public string CustomFields
        {
            get { return customFields; }
            set { customFields = value; }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            VideoSearchUserControl userControl = (VideoSearchUserControl)Page.LoadControl(@"~/_CONTROLTEMPLATES/BrightcoveVideoCloudIntegration/BrightcoveVideoCloudSearch/VideoSearchUserControl.ascx");
            
            userControl.InitApiConnection(this.configProvider);
            userControl.PlayVideoLink = this.playVideoLink;
            userControl.EditVideoLink = this.editVideoLink;
            userControl.CustomFields = this.customFields;
            userControl.Deprecated_Page_Load();
            Controls.Add(userControl);
        }
    }
}
