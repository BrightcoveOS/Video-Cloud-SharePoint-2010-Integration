using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

using BrightcoveSDK.Entities;
using BrightcoveSDK;
using BrightcoveSDK.Entities.Containers;
using BrightcoveSDK.Media;

namespace BrightcoveVideoCloudIntegration.VideoConfig
{
    [ToolboxItemAttribute(false)]
    public class VideoConfig : WebPart, IVideoCloudConfig
    {
        // Value of your Publisher ID for Brightcove Video Cloud
        public string PublisherId
        {
            get { return SPContext.Current.Web.AllProperties[VideoCloudWebPart.KeyPublisherId].ToString(); }
            set { SPContext.Current.Web.AllProperties[VideoCloudWebPart.KeyPublisherId] = value; }
        }

        // Value of your read token for Brightcove Video Cloud
        public string ReadToken
        {
            get { return SPContext.Current.Web.AllProperties[VideoCloudWebPart.KeyReadToken].ToString(); }
            set { SPContext.Current.Web.AllProperties[VideoCloudWebPart.KeyReadToken] = value; }
        }

        // Value of your write token for Brightcove Video Cloud
        public string WriteToken
        {
            get { return SPContext.Current.Web.AllProperties[VideoCloudWebPart.KeyWriteToken].ToString(); }
            set { SPContext.Current.Web.AllProperties[VideoCloudWebPart.KeyWriteToken] = value; }
        }

        // Value of your Read URL for Brightcove Video Cloud
        public string ReadUrl
        {
            get { return SPContext.Current.Web.AllProperties[VideoCloudWebPart.KeyReadUrl].ToString(); }
            set { SPContext.Current.Web.AllProperties[VideoCloudWebPart.KeyReadUrl] = value; }
        }

        // Value of your Write URL for Brightcove Video Cloud
        public string WriteUrl
        {
            get { return SPContext.Current.Web.AllProperties[VideoCloudWebPart.KeyWriteUrl].ToString(); }
            set { SPContext.Current.Web.AllProperties[VideoCloudWebPart.KeyWriteUrl] = value; }
        }

        // Default value for Video Player ID for Brightcove Video Cloud
        public string DefaultVideoPlayerId
        {
            get { return SPContext.Current.Web.AllProperties[VideoCloudWebPart.KeyVideoPlayerId].ToString(); }
            set { SPContext.Current.Web.AllProperties[VideoCloudWebPart.KeyVideoPlayerId] = value; }
        }

        // Default value for Playlist Player ID for Brightcove Video Cloud
        public string DefaultPlaylistPlayerId
        {
            get { return SPContext.Current.Web.AllProperties[VideoCloudWebPart.KeyPlaylistPlayerId].ToString(); }
            set { SPContext.Current.Web.AllProperties[VideoCloudWebPart.KeyPlaylistPlayerId] = value; }
        }

        public IVideoCloudConfig VideoCloudConfigProvider()
        {
            return this;
        }
    }
}
