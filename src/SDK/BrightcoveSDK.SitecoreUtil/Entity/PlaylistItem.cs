using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using BrightcoveSDK;
using BrightcoveSDK.Containers;

namespace BrightcoveSDK.SitecoreUtil.Entity
{
	public class PlaylistItem
	{

		public Item playlistItem;

		public PlaylistItem(Item i) {
			playlistItem = i;
		}

		#region Editable Fields

		public string PlaylistName {
			get {
				return playlistItem.Fields["Name"].Value;
			}
			set {
				playlistItem.Fields["Name"].Value = value;
			}
		}

		public string ReferenceID {
			get {
				return playlistItem.Fields["Reference Id"].Value;
			}
			set {
				playlistItem.Fields["Reference Id"].Value = value;
			}
		}

		public string ShortDescription {
			get {
				return playlistItem.Fields["Short Description"].Value;
			}
			set {
				playlistItem.Fields["Short Description"].Value = value;
			}
		}

		public BCCollection<string> FilterTags {
			get {
				BCCollection<string> bc = new BCCollection<string>();
				string[] splitr = { "," };
				foreach (string s in playlistItem.Fields["Filter Tags"].Value.Split(splitr, StringSplitOptions.RemoveEmptyEntries)) {
					bc.Add(s);
				}
				return bc;
			}
			set {
				playlistItem.Fields["Filter Tags"].Value = value.ToDelimitedString(",");
			}
		}

		public PlaylistTypeEnum PlaylistType {
			get {
				string val = playlistItem.Fields["Playlist Type"].Value;
				if (val.Equals(PlaylistTypeEnum.ALPHABETICAL.ToString())) {
					return PlaylistTypeEnum.ALPHABETICAL;
				} else if (val.Equals(PlaylistTypeEnum.EXPLICIT.ToString())) {
					return PlaylistTypeEnum.EXPLICIT;
				} else if (val.Equals(PlaylistTypeEnum.NEWEST_TO_OLDEST.ToString())) {
					return PlaylistTypeEnum.NEWEST_TO_OLDEST;
				} else if (val.Equals(PlaylistTypeEnum.OLDEST_TO_NEWEST.ToString())) {
					return PlaylistTypeEnum.OLDEST_TO_NEWEST;
				} else if (val.Equals(PlaylistTypeEnum.PLAYS_TOTAL.ToString())) {
					return PlaylistTypeEnum.PLAYS_TOTAL;
				} else {
					return PlaylistTypeEnum.PLAYS_TRAILING_WEEK;
				}
			}
			set {
				playlistItem.Fields["Playlist Type"].Value = value.ToString();
			}
		}

		#endregion

		#region Non-Editable Fields

		public long PlaylistID {
			get {
				return long.Parse(playlistItem.Fields["ID"].Value);
			}
		}

		public string ThumbnailURL {
			get {
				return playlistItem.Fields["Thumbnail URL"].Value;
			}
			//set {
			//    this.Fields["Thumbnail URL"].Value = value;
			//}
		}

		public BCCollection<string> VideoIds {
			get {
				BCCollection<string> bc = new BCCollection<string>();
				string[] splitr = { "," };
				foreach (string s in playlistItem.Fields["Video Ids"].Value.Split(splitr, StringSplitOptions.RemoveEmptyEntries)) {
					bc.Add(s);
				}
				return bc;
			}
			//set {
			//    this.Fields["Video Ids"].Value = value.ToDelimitedString(",");
			//}
		}

		public long AccountId {
			get {
				return long.Parse(playlistItem.Fields["Account Id"].Value);
			}
		}

		#endregion
	}
}
