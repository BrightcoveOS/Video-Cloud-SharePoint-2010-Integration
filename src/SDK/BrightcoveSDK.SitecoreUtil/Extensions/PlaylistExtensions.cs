using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrightcoveSDK.Media;
using Sitecore.Data.Items;
using System.Collections;
using System.Web;
using System.Text.RegularExpressions;
using BrightcoveSDK.SitecoreUtil.Entity;
using BrightcoveSDK.SitecoreUtil.Entity.Container;

namespace BrightcoveSDK.SitecoreUtil.Extensions
{
	public static class BCPlaylistExtensions
	{

		public static UpdateInsertPair<PlaylistItem> ImportToSitecore(this AccountItem account, BCPlaylist Playlist, UpdateType utype) {
			List<BCPlaylist> Playlists = new List<BCPlaylist>();
			Playlists.Add(Playlist);
			return ImportToSitecore(account, Playlists, utype);
		}

		public static UpdateInsertPair<PlaylistItem> ImportToSitecore(this AccountItem account, List<BCPlaylist> Playlists, UpdateType utype) {

			UpdateInsertPair<PlaylistItem> uip = new UpdateInsertPair<PlaylistItem>();

			//set all BCVideos into hashtable for quick access
			Hashtable ht = new Hashtable();
			foreach (PlaylistItem exPlay in account.PlaylistLib.Playlists) {
				if (!ht.ContainsKey(exPlay.playlistItem.ToString())) {
					//set as string, Item pair
					ht.Add(exPlay.PlaylistID.ToString(), exPlay);
				}
			}

			//Loop through the data source and add them
			foreach (BCPlaylist play in Playlists) {

				try {
					//remove access filter
					using (new Sitecore.SecurityModel.SecurityDisabler()) {

						PlaylistItem currentItem;

						//if it exists then update it
						if (ht.ContainsKey(play.id.ToString()) && (utype.Equals(UpdateType.BOTH) || utype.Equals(UpdateType.UPDATE))) {
							currentItem = (PlaylistItem)ht[play.id.ToString()];

							//add it to the new items
							uip.UpdatedItems.Add(currentItem);

							using (new EditContext(currentItem.playlistItem, true, false)) {
								SetPlaylistFields(ref currentItem.playlistItem, play);
							}
						}
							//else just add it
						else if (!ht.ContainsKey(play.id.ToString()) && (utype.Equals(UpdateType.BOTH) || utype.Equals(UpdateType.NEW))) {
							//Create new item
							TemplateItem templateType = account.Database.Templates["Modules/Brightcove/Brightcove Playlist"];
							currentItem = new PlaylistItem(account.PlaylistLib.playlistLibraryItem.Add(play.name.StripInvalidChars(), templateType));

							//add it to the new items
							uip.NewItems.Add(currentItem);

							using (new EditContext(currentItem.playlistItem, true, false)) {
								SetPlaylistFields(ref currentItem.playlistItem, play);
							}
						}
					}
				} catch (System.Exception ex) {
					throw new Exception("Failed on playlist: " + play.name + ". " + ex.ToString());
				}
			}

			return uip;
		}

		private static void SetPlaylistFields(ref Item currentItem, BCPlaylist play) {

			//Set the appropriate field values for the new item
			currentItem.Fields["Name"].Value = play.name;
			currentItem.Fields["Reference Id"].Value = play.referenceId;
			currentItem.Fields["Short Description"].Value = play.shortDescription;
			string vidIdList = "";
			foreach (long vidId in play.videoIds) {
				if (vidIdList.Length > 0) {
					vidIdList += ",";
				}
				vidIdList += vidId.ToString();
			}
			currentItem.Fields["Video Ids"].Value = vidIdList;
			currentItem.Fields["ID"].Value = play.id.ToString();
			currentItem.Fields["Thumbnail URL"].Value = play.thumbnailURL;
			string filterTags = "";
			if (play.filterTags != null) {
				foreach (string tag in play.filterTags) {
					if (filterTags.Length > 0) {
						filterTags += ",";
					}
					filterTags += tag;
				}
			}
			currentItem.Fields["Filter Tags"].Value = filterTags;
			currentItem.Fields["Playlist Type"].Value = play.playlistType.ToString();
			currentItem.Fields["Account Id"].Value = play.accountId.ToString();
		}
	}
}
