using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint;

namespace BrightcoveVideoCloudIntegration.VideoPlayer
{
    [ToolboxItemAttribute(false)]
    public class VideoPlayer : VideoCloudWebPart
    {
        public const string VideoChooser = "/_layouts/BrightcoveVideoCloudIntegration/Chooser.aspx?async_chooser=VideoId&IsDlg=1";
        public const string PlaylistChooser = "/_layouts/BrightcoveVideoCloudIntegration/Chooser.aspx?async_chooser=PlaylistId&IsDlg=1";

        private string backgroundColor = "#FFFFFF";
        [WebBrowsable(true),
            Category("Brightcove Configuration"),
            Personalizable(PersonalizationScope.Shared),
            DefaultValue("http://videocloud"),
            WebDisplayName("Background Color"),
            WebDescription("Value of background color for video player.")]
        public string BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        private string playerWidth = "640";
        [WebBrowsable(true),
            Category("Brightcove Configuration"),
            Personalizable(PersonalizationScope.Shared),
            DefaultValue("http://videocloud"),
            WebDisplayName("Player Width"),
            WebDescription("Value of width for video player.")]
        public string PlayerWidth
        {
            get { return playerWidth; }
            set { playerWidth = value; }
        }

        private string playerHeight = "390";
        [WebBrowsable(true),
            Category("Brightcove Configuration"),
            Personalizable(PersonalizationScope.Shared),
            DefaultValue("http://videocloud"),
            WebDisplayName("Player Height"),
            WebDescription("Value of height for video player.")]
        public string PlayerHeight
        {
            get { return playerHeight; }
            set { playerHeight = value; }
        }

        private bool autoStart = false;
        [WebBrowsable(true),
            Category("Brightcove Configuration"),
            Personalizable(PersonalizationScope.Shared),
            DefaultValue("http://videocloud"),
            WebDisplayName("Auto Start"),
            WebDescription("Will the video automatically start playing?")]
        public bool AutoStart
        {
            get { return autoStart; }
            set { autoStart = value; }
        }

        private string playerId;
        [WebBrowsable(true),
            Category("Brightcove Configuration"),
            Personalizable(PersonalizationScope.Shared),
            DefaultValue("http://videocloud"),
            WebDisplayName("Player ID"),
            WebDescription("Value of your Player ID for video player.")]
        public string PlayerId
        {
            get { return playerId; }
            set { playerId = value; }
        }

        private string videoId;
        [WebBrowsable(true),
            Category("Brightcove Configuration"),
            Personalizable(PersonalizationScope.Shared),
            DefaultValue("http://videocloud"),
            WebDisplayName("Video ID"),
            WebDescription("Value of Video ID for video player.")]
        [HtmlDesignerAttribute(VideoPlayer.VideoChooser, DialogFeatures = "dialogHeight:370px;dialogWidth:330px;help:no;status:no;resizable:yes")]
        public string VideoId
        {
            get { return videoId; }
            set { videoId = value; }
        }

        private string playlistId = string.Empty;
        [WebBrowsable(true),
            Category("Brightcove Configuration"),
            Personalizable(PersonalizationScope.Shared),
            DefaultValue("http://videocloud"),
            WebDisplayName("Playlist ID"),
            WebDescription("Value of Playlist ID for video player.")]
        [HtmlDesignerAttribute(VideoPlayer.PlaylistChooser, DialogFeatures = "dialogHeight:370px;dialogWidth:330px;help:no;status:no;resizable:yes")]
        public string PlaylistId
        {
            get { return playlistId; }
            set { playlistId = value; }
        }

        //protected override string GetCustomBuilder(string propertyName)
        //{
        //    if ( (propertyName == "VideoId") || (propertyName == "PlaylistId") )
        //    {
        //        string url = SPContext.Current.Web.Url + "/_layouts/BrightcoveVideoCloudIntegration/Chooser.aspx?IsDlg=1&" + 
        //            VideoCloudWebPart.QueryStringKeyAsyncChooserText + "=" + propertyName;

        //        return url;
        //    }

        //    return base.GetCustomBuilder(propertyName);
        //}

        //protected override string GetCustomBuilder(string propertyName)
        //{
        //    if (propertyName == "VideoId")
        //    {
        //        return "VideoChooser.aspx";
        //    }
        //    return base.GetCustomBuilder(propertyName);
        //}

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            VideoPlayerUserControl userControl = (VideoPlayerUserControl)Page.LoadControl(@"~/_CONTROLTEMPLATES/BrightcoveVideoCloudIntegration/BrightcoveVideoCloudPlayer/VideoPlayerUserControl.ascx");
            long vid = 0;

            userControl.InitApiConnection(this.configProvider);
            userControl.BackgroundColor = this.backgroundColor;
            userControl.PlayerWidth = this.playerWidth;
            userControl.PlayerHeight = this.playerHeight;
            userControl.PlayerId = this.playerId;

            if (this.Page.Request.QueryString["autostart"] == "true")
            {
                userControl.AutoStart = true;
            }
            else
            {
                userControl.AutoStart = this.autoStart;
            }

            if (long.TryParse(this.Page.Request.QueryString["videoid"], out vid))
            {
                userControl.VideoId = vid.ToString();
            }
            else
            {
                userControl.VideoId = this.videoId;
            }

            if (long.TryParse(this.Page.Request.QueryString["playlistid"], out vid))
            {
                userControl.PlaylistId = vid.ToString();
            }
            else
            {
                userControl.PlaylistId = this.playlistId;
            }

            Controls.Add(userControl);
        }
    }
}
