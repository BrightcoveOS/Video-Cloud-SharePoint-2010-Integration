using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebPartPages;
using BrightcoveVideoCloudIntegration;

namespace BrightcoveVideoCloudIntegration.Features.Demo
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("6ed9588d-f355-4733-ab5b-1d18a26b3fbe")]
    public class DemoEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
           
            VideoPlayer.VideoPlayer player = null;

            // Is the admin group created yet?
            using (SPWeb checkWeb = (SPWeb)properties.Feature.Parent)
            {
                if (Util.IsUserAnAdmin(checkWeb))
                {
                    // The user is in the admin group
                }
            }

            // Search
            //AddWebParts(properties, "Demo/SearchResults.aspx", MakeConsumerWebPart(new VideoSearch.VideoSearch(), "Brightcove Video Cloud Search"));

            // Player: Previewing
            //AddWebParts(properties, "Demo/VideoPlayer.aspx", MakeConsumerWebPart(new VideoPlayer.VideoPlayer(), "Brightcove Video Cloud Player"));

            // Add playlist player
            //player = new VideoPlayer.VideoPlayer();
            //player.PlayerWidth = "960";
            //player.PlayerHeight = "450";
            //AddWebParts(properties, "Demo/PlaylistPlayer.aspx", MakeConsumerWebPart(player, "Brightcove Video Cloud Player"));

            // Articles: Publishing
            player = new VideoPlayer.VideoPlayer();
            player.PlayerWidth = "320";
            player.PlayerHeight = "195";
            AddWebParts(properties, "Demo/Articles.aspx", new System.Web.UI.WebControls.WebParts.WebPart[] { MakeConsumerWebPart(player, "Brightcove Video Cloud Player") }, new string[] { "Header"});

            // Playlist
            AddWebParts(properties, "Demo/VideoPlaylist.aspx", new System.Web.UI.WebControls.WebParts.WebPart[] { 
                MakeConsumerWebPart(new VideoPlaylist.VideoPlaylist(), "Brightcove Video Cloud Playlist"), 
                MakeConsumerWebPart(new VideoPicklist.VideoPicklist(), "Brightcove Video Cloud Picklist", true) });

            // Playlist Editor
            AddWebParts(properties, "Demo/PlaylistEditor.aspx", new System.Web.UI.WebControls.WebParts.WebPart[] { 
                MakeConsumerWebPart(new VideoPlaylist.VideoPlaylist(), "Brightcove Video Cloud Playlist"), 
                MakeConsumerWebPart(new VideoPicklist.VideoPicklist(), "Brightcove Video Cloud Picklist", true) });

            // Editor
            //AddWebParts(properties, "Demo/VideoEditor.aspx", MakeConsumerWebPart(new VideoEditor.VideoEditor(), "Brightcove Video Cloud Editor")); 

            // Upload
            //AddWebParts(properties, "Demo/VideoUpload.aspx", MakeConsumerWebPart(new VideoEditor.VideoEditor(), "Brightcove Video Cloud Editor"));

        }

        private static VideoConfig.VideoConfig MakeProviderWebPart()
        {
            VideoConfig.VideoConfig provider = new VideoConfig.VideoConfig();

            provider.ChromeType = PartChromeType.None;
            provider.AllowConnect = true;
            provider.ExportMode = WebPartExportMode.All;
            provider.Title = "Brightcove Video Cloud Config";
            provider.PublisherId = "";  // Add your publisher ID here
            provider.ReadToken = "";    // Add your read tokens here, comma separated
            provider.WriteToken = "";   // Add your write tokens here, comma separated

            return provider;
        }

        private static System.Web.UI.WebControls.WebParts.WebPart MakeConsumerWebPart(System.Web.UI.WebControls.WebParts.WebPart consumer, string title)
        {
            return MakeConsumerWebPart(consumer, title, false);
        }

        private static System.Web.UI.WebControls.WebParts.WebPart MakeConsumerWebPart(System.Web.UI.WebControls.WebParts.WebPart consumer, string title, bool noChrome)
        {
            if (noChrome)
            {
                consumer.ChromeType = PartChromeType.None;
            }

            consumer.Title = title;
            //consumer.AllowConnect = true;
            //consumer.ExportMode = WebPartExportMode.All;

            return consumer;
        }

        private static void ConnectWebParts(SPFeatureReceiverProperties properties, string url, System.Web.UI.WebControls.WebParts.WebPart provider,
            System.Web.UI.WebControls.WebParts.WebPart consumer)
        {
            ConnectWebParts(properties, url, provider, new System.Web.UI.WebControls.WebParts.WebPart[] { consumer });
        }

        private static void ConnectWebParts(SPFeatureReceiverProperties properties, string url, System.Web.UI.WebControls.WebParts.WebPart provider, 
            System.Web.UI.WebControls.WebParts.WebPart[] consumer)
        {
            SPWeb web = (SPWeb) properties.Feature.Parent;
            SPFile file = web.GetFile(url);
            SPLimitedWebPartManager manager = file.GetLimitedWebPartManager(PersonalizationScope.Shared);
            ProviderConnectionPoint providerPoint = null;
            ConsumerConnectionPoint consumerPoint = null;
            int index = 6;

            while (manager.WebParts.Count > 0)
            {
                manager.DeleteWebPart(manager.WebParts[0]);
            }

            manager.AddWebPart(consumer[0], "MiddleColumn", 2);
            manager.AddWebPart(provider, "MiddleColumn", 4);

            for (int i = 1; i < consumer.Length; i ++)
            {
                System.Web.UI.WebControls.WebParts.WebPart webpart = consumer[i];

                manager.AddWebPart(webpart, "MiddleColumn", index);
                index += 2;
            }

            providerPoint = manager.GetProviderConnectionPoints(provider)[0];
            consumerPoint = manager.GetConsumerConnectionPoints(consumer[0])[0];
            manager.SPConnectWebParts(provider, providerPoint, consumer[0], consumerPoint);
            file.Update();

            if (manager.Web != null)
            {
                manager.Web.Dispose();
            }

            manager.Dispose();
        }

        private static void AddWebParts(SPFeatureReceiverProperties properties, string url, System.Web.UI.WebControls.WebParts.WebPart webPart)
        {
            AddWebParts(properties, url, new System.Web.UI.WebControls.WebParts.WebPart[] { webPart });
        }

        private static void AddWebParts(SPFeatureReceiverProperties properties, string url, System.Web.UI.WebControls.WebParts.WebPart[] webPart)
        {
            string[] zones = new string[webPart.Length];

            for (int i = 0; i < webPart.Length; i++)
            {
                zones[i] = "MiddleColumn";
            }

            AddWebParts(properties, url, webPart, zones);
        }

        private static void AddWebParts(SPFeatureReceiverProperties properties, string url, System.Web.UI.WebControls.WebParts.WebPart[] webPart, string[] zones)
        {
            SPWeb web = (SPWeb)properties.Feature.Parent;
            SPFile file = web.GetFile(url);
            SPLimitedWebPartManager manager = file.GetLimitedWebPartManager(PersonalizationScope.Shared);
            int index = 2;

            while (manager.WebParts.Count > 0)
            {
                manager.DeleteWebPart(manager.WebParts[0]);
            }

            for (int i = 0; i < webPart.Length; i++)
            {
                manager.AddWebPart(webPart[i], zones[i], index);
                index += 2;
            }

            file.Update();

            if (manager.Web != null)
            {
                manager.Web.Dispose();
            }

            manager.Dispose();
        }

        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
