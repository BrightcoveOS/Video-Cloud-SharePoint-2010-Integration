using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace BrightcoveVideoCloudIntegration.VideoEditor
{
    [ToolboxItemAttribute(false)]
    public class VideoEditor : VideoCloudWebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/BrightcoveVideoCloudIntegration/BrightcoveVideoCloudEditor/VideoEditorUserControl.ascx";

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            VideoEditorUserControl control = (VideoEditorUserControl)Page.LoadControl(_ascxPath);

            control.InitApiConnection(this.configProvider);
            control.Deprecated_Page_Load();
            Controls.Add(control);
        }
    }
}
