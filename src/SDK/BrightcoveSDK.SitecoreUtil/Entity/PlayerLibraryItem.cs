using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Data;
using System.Web;
using Sitecore.Data.Fields;
using BrightcoveSDK;
using BrightcoveSDK.SitecoreUtil.Extensions;
using BrightcoveSDK.SitecoreUtil.Entity;

namespace BrightcoveSDK.SitecoreUtil
{
	public class PlayerLibraryItem
	{
		public Item playerLibraryItem;

		public PlayerLibraryItem(Item i) {
			playerLibraryItem = i;
		}

		public List<PlayerItem> Players {
			get {
                List<Item> p = this.playerLibraryItem.ChildrenByTemplateRecursive("Brightcove Video Player");
				List<PlayerItem> players = new List<PlayerItem>();
				foreach (Item i in p) {
					players.Add(new PlayerItem(i));
				}
				return players;
			}
        }

        #region Static Methods

        public static PlayerItem GetPlayer(long PlayerID) {
            return GetPlayer(PlayerID, Sitecore.Context.Database);
        }
		public static PlayerItem GetPlayer(long PlayerID, Database DB) {
            Item i = DB.GetItem(Constants.BrightcoveLibID);
            if (i != null) {
                Item j = i.ChildrenByTemplateRecursive(Constants.PlayerTemplate).Where(player => player["Player ID"] == PlayerID.ToString()).ToList().FirstOrDefault();
                if (j != null) {
                    return new PlayerItem(j);
                }
            }
            return null;
        }

        #endregion Static Methods
    }
}
