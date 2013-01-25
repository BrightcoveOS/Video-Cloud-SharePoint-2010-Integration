using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Data;
using BrightcoveSDK.SitecoreUtil.Extensions;
using Sitecore.Configuration;
using System.Xml;
using BrightcoveSDK.Extensions;
using BrightcoveSDK.HTTP;
using BrightcoveSDK.Media;
using BrightcoveSDK.JSON;
using System.Configuration;

namespace BrightcoveSDK.SitecoreUtil.Entity
{
	public class AccountItem : Item {
		
		public string AccountName;
		public string AccountType;
		public string ReadToken;
		public string ReadURL;
		public string WriteToken;
		public string WriteURL;

		public AccountItem(ID itemID, ItemData data, Database database) : base(itemID, data, database) {

			BrightcoveConfig bc = (BrightcoveConfig)ConfigurationManager.GetSection("brightcove");
			foreach (AccountConfigElement a in bc.Accounts) {
				if (a.PublisherID.Equals(this.PublisherID)) {
					AccountName = a.Name; 
					AccountType = a.Type.ToString();
					ReadToken = a.ReadToken.Value;
					ReadURL = a.ReadURL.Value;
					WriteToken = a.WriteToken.Value;
					WriteURL = a.WriteURL.Value;
				}
			}
		}

		public void Parse(XmlNode node) {
			
		}

		public long PublisherID {
			get {
				if (this.Fields["Publisher ID"].Value.Equals("")) {
					return 0;
				}
				else {
					return long.Parse(this.Fields["Publisher ID"].Value);
				}
			}
			set {
				this.Fields["Publisher ID"].Value = value.ToString();
			}
		}

		public PlaylistLibraryItem PlaylistLib {
			get {
				return new PlaylistLibraryItem(this.ChildByTemplate("Playlist Library"));
			}
		}

		public VideoLibraryItem VideoLib {
			get {
				return new VideoLibraryItem(this.ChildByTemplate("Video Library"));
			}
		}

		public PlayerLibraryItem PlayerLib {
			get {
				return new PlayerLibraryItem(this.ChildByTemplate("Video Player Library"));
			}
		}
	}
}
