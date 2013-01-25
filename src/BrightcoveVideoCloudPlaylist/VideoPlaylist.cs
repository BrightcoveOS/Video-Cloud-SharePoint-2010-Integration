using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace BrightcoveVideoCloudIntegration.VideoPlaylist
{
    [ToolboxItemAttribute(false)]
    public class VideoPlaylist : VideoCloudWebPart
    {
        // Value of your Playlist ID
        private string playlistId;
        public string PlaylistId
        {
            get { return playlistId; }
            set { playlistId = value; }
        }

        // List of Video IDs for this playlist
        private string[] videoIdList;
        public string VideoIdList
        {
            get
            {
                if (videoIdList == null)
                {
                    return string.Empty;
                }
                else
                {
                    return String.Join(",", videoIdList);
                }
            }

            set
            {
                if (value != null)
                {
                    videoIdList = value.Split(",".ToCharArray());
                }
            }
        }

        private string playVideoLink = "javascript:void OpenPopUpPage('./PlaylistPlayer.aspx?playlistid={0}&autostart=true',null,1000,500)";
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

        private string editPlaylistLink = "javascript:void OpenPopUpPage('./PlaylistEditor.aspx?playlistid={0}',null,725,490)";
        [WebBrowsable(true),
                    Category("Brightcove Configuration"),
                    Personalizable(PersonalizationScope.Shared),
                    DefaultValue("http://videocloud"),
                    WebDisplayName("Edit Playlist Link"),
                    WebDescription("Page link formatter for editing a given playlist id.")]
        public string EditPlaylistLink
        {
            get { return editPlaylistLink; }
            set { editPlaylistLink = value; }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            VideoPlaylistUserControl userControl = (VideoPlaylistUserControl)Page.LoadControl(@"~/_CONTROLTEMPLATES/BrightcoveVideoCloudIntegration/BrightcoveVideoCloudPlaylist/VideoPlaylistUserControl.ascx");
            //long vid = 0;

            userControl.InitApiConnection(this.configProvider);
            userControl.PlaylistId = this.playlistId;
            userControl.VideoIdList = this.videoIdList;
            userControl.PlayVideoLink = this.playVideoLink;
            userControl.EditPlaylistLink = this.editPlaylistLink;
            userControl.Deprecated_Page_Load();

            Controls.Add(userControl);
        }
    }
}
