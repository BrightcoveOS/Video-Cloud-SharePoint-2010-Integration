using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using BrightcoveSDK.Entities;
using BrightcoveSDK;
using BrightcoveSDK.Entities.Containers;
using BrightcoveSDK.Media;
using Microsoft.SharePoint;

namespace BrightcoveVideoCloudIntegration
{
    public partial class VideoCloudWebPartUserControl : UserControl
    {
        private static readonly char[] tokenDelim = ",".ToCharArray();
        private int _isAdmin = 0;

        protected BCAPI bcApi;
        protected AccountConfigElement accountConfig;
        protected string[] readTokenList;
        protected string[] writeTokenList;
        protected string DefaultVideoPlayerId;
        protected string DefaultPlaylistPlayerId;

        public SPUser CurrentUser
        {
            get
            {
                return SPContext.Current.Web.CurrentUser;
            }
        }

        public bool IsAdmin
        {
            get
            {
                if (this._isAdmin == 0)
                {
                    if (Util.IsUserAnAdmin(SPContext.Current.Web))
                    {
                        this._isAdmin = 1;
                    }
                    else
                    {
                        this._isAdmin = -1;
                    }
                }

                if (this._isAdmin > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void ResetApiConnection()
        {
            if (this.accountConfig == null)
            {
                this.bcApi = null;

                return;
            }

            try
            {
                BrightcoveConfig config = new BrightcoveConfig();
                config.Accounts.Add(this.accountConfig);
                this.bcApi = new BCAPI(config.Accounts[0]);
            }
            catch
            {
                // Do nothing
            }
        }

        public void InitApiConnection(IVideoCloudConfig configProvider)
        {
            if (configProvider == null)
            {
                return;
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(configProvider.PublisherId) ||
                    string.IsNullOrEmpty(configProvider.ReadToken) ||
                    string.IsNullOrEmpty(configProvider.WriteToken))
                    {
                        return;
                    }
                }
                catch { return; }
            }

            try
            {
                this.accountConfig = new AccountConfigElement("brightcove");
                this.accountConfig.PublisherID = Convert.ToInt64(configProvider.PublisherId);
                this.readTokenList = configProvider.ReadToken.Split(tokenDelim);
                this.writeTokenList = configProvider.WriteToken.Split(tokenDelim);

                if (this.readTokenList.Length > 0)
                {
                    this.accountConfig.ReadToken.Value = this.readTokenList[0];
                }
                else
                {
                    this.accountConfig.ReadToken.Value = configProvider.ReadToken;
                }

                if (this.writeTokenList.Length > 0)
                {
                    this.accountConfig.WriteToken.Value = this.writeTokenList[0];
                }
                else
                {
                    this.accountConfig.WriteToken.Value = configProvider.WriteToken;
                }

                this.accountConfig.ReadURL.Value = configProvider.ReadUrl;
                this.accountConfig.WriteURL.Value = configProvider.WriteUrl;

                // Set default values for player
                this.DefaultVideoPlayerId = configProvider.DefaultVideoPlayerId;
                this.DefaultPlaylistPlayerId = configProvider.DefaultPlaylistPlayerId;
            }
            catch
            {
                // Alert user there is a problem with their site settings
                //throw new Exception("Brightcove Configuration is not setup.  Please enter your Brightcove Video Cloud account settings in this site's Site Settings.");
                return;
            }

            ResetApiConnection();
        }

        public BCAPI CreateAPI(IVideoCloudConfig configProvider)
        {
            InitApiConnection(configProvider);

            return this.bcApi;
        }

        public virtual void Deprecated_Page_Load()
        {
            // Do nothing
        }
    }
}
