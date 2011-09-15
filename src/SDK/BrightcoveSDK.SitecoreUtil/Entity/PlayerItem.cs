using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace BrightcoveSDK.SitecoreUtil.Entity
{
	public class PlayerItem
	{
		public Item playerItem;

		public PlayerItem(Item i) {
			playerItem = i;
		}

		public string Name {
			get {
				return playerItem.Fields["Name"].Value;
			}
			set {
				playerItem.Fields["Name"].Value = value;
			}
		}

		public long PlayerID {
			get {
				return long.Parse(playerItem.Fields["Player ID"].Value);
			}
			set {
				playerItem.Fields["Player ID"].Value = value.ToString();
			}
		}

		public PlayerPlaylistType PlaylistType {
			get {
				string type = playerItem.Fields["Playlist Type"].Value.Replace(" ", "");
				if (type.Equals(PlayerPlaylistType.ComboBox.ToString())) {
					return PlayerPlaylistType.ComboBox;
				} else if (type.Equals(PlayerPlaylistType.Tabbed.ToString())) {
					return PlayerPlaylistType.Tabbed;
				} else if (type.Equals(PlayerPlaylistType.VideoList.ToString())) {
					return PlayerPlaylistType.VideoList;
				} else {
					return PlayerPlaylistType.None;
				}
			}
			set {
				playerItem.Fields["Playlist Type"].Value = value.ToString();
			}
		}

		public string Description {
			get {
				return playerItem.Fields["Description"].Value;
			}
			set {
				playerItem.Fields["Description"].Value = value;
			}
		}

		public int Width {
			get {
				return int.Parse(playerItem.Fields["Width"].Value);
			}
			set {
				playerItem.Fields["Width"].Value = value.ToString();
			}
		}

		public int Height {
			get {
				return int.Parse(playerItem.Fields["Height"].Value);
			}
			set {
				playerItem.Fields["Height"].Value = value.ToString();
			}
		}
	}
}
