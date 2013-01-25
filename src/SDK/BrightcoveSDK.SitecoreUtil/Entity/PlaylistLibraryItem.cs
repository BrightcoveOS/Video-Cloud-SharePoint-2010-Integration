using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Data;
using System.Web;
using BrightcoveSDK.SitecoreUtil.Extensions;
using BrightcoveSDK.SitecoreUtil.Entity;

namespace BrightcoveSDK.SitecoreUtil
{
	public class PlaylistLibraryItem {

		public Item playlistLibraryItem;

		public PlaylistLibraryItem(Item i) {
			playlistLibraryItem = i;
		}

		public List<PlaylistItem> Playlists {
			get {
                List<Item> p = this.playlistLibraryItem.ChildrenByTemplateRecursive("Brightcove Playlist");
				List<PlaylistItem> playlists = new List<PlaylistItem>();
				foreach (Item i in p) {
					playlists.Add(new PlaylistItem(i));
				}
				return playlists;
			}
        }

        #region Static Methods

		public static PlaylistItem GetPlaylist(long PlaylistID) {
            return GetPlaylist(PlaylistID, Sitecore.Context.Database);
        }
		public static PlaylistItem GetPlaylist(long PlaylistID, Database DB) {
            Item i = DB.GetItem(Constants.BrightcoveLibID);
            if(i != null){
                Item j = i.ChildrenByTemplateRecursive(Constants.PlaylistTemplate).Where(playlist => playlist["ID"] == PlaylistID.ToString()).ToList().FirstOrDefault();
                if (j != null) {
					return new PlaylistItem(j);
                }
            }
            return null;
        }
        
        #endregion
    }
}
