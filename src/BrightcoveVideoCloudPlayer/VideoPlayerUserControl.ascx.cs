using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using BrightcoveVideoCloudIntegration;
using Microsoft.SharePoint;

namespace BrightcoveVideoCloudIntegration.VideoPlayer
{
    public partial class VideoPlayerUserControl : VideoCloudWebPartUserControl
    {
        public string BackgroundColor;
        public string PlayerWidth;
        public string PlayerHeight;
        public bool AutoStart;
        public string PlayerId
        {
            get
            {
                if (string.IsNullOrEmpty(this._playerId))
                {
                    if (string.IsNullOrEmpty(this.PlaylistId))
                    {
                        return this.DefaultVideoPlayerId;
                    }
                    else
                    {
                        return this.DefaultPlaylistPlayerId;
                    }
                }
                else
                {
                    return this._playerId;
                }
            }

            set
            {
                this._playerId = value;
            }
        }

        public string VideoId;
        public string PlaylistId;

        private string _playerId;

    }
}
