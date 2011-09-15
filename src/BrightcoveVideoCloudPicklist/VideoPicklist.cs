using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace BrightcoveVideoCloudIntegration.VideoPicklist
{
    [ToolboxItemAttribute(false)]
    public class VideoPicklist : VideoCloudWebPart
    {
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            VideoPicklistUserControl userControl = (VideoPicklistUserControl)Page.LoadControl(@"~/_CONTROLTEMPLATES/BrightcoveVideoCloudIntegration/BrightcoveVideoCloudPicklist/VideoPicklistUserControl.ascx");

            userControl.InitApiConnection(this.configProvider);
            userControl.Deprecated_Page_Load();
            Controls.Add(userControl);
        }
    }
}
