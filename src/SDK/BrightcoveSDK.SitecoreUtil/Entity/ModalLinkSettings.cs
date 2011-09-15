using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Data;
using BrightcoveSDK.SitecoreUtil.Extensions;

namespace BrightcoveSDK.SitecoreUtil.Entity
{
    public class ModalLinkSettings
    {
        public Item thisItem;

        public ID ID {
            get {
                return thisItem.ID;
            }
        }

        #region Modal Link Settings

        ///<summary>
        /// Domain
        ///</summary>
        private string _Domain = null;
        public string Domain {
            get {
                if (_Domain == null) {
                    _Domain = thisItem["Domain"];
                }
                return _Domain;
            }
        }

        #endregion Modal Link Settings

        #region Constructors

		public ModalLinkSettings() {
		}

        public ModalLinkSettings(Item i) {
			thisItem = i;
		}

		#endregion

        #region Static Methods

        /// <summary>
        /// This will return a ModalLinkSettings Item from the content tree whose domain value is what you specified. 
        /// </summary>
        /// <param name="domain">The domain value in the Modal Link Settings Item. "default" is the default value</param>
        /// <returns>ModalLinkSettings item. if there are no matches or you don't specify a domain the item whose domain field is "default" will be selected.</returns>
        public static ModalLinkSettings GetModalLinkSettings() {
            return GetModalLinkSettings("default", Sitecore.Context.Database);
        }
        public static ModalLinkSettings GetModalLinkSettings(string domain){
            return GetModalLinkSettings(domain, Sitecore.Context.Database);
        }
        public static ModalLinkSettings GetModalLinkSettings(string domain, Database DB) {
            Item settings = DB.GetItem(Constants.BrightcoveSettingsID);
            if (settings != null) {
                List<Item> i = settings.ChildrenByTemplateRecursive(Constants.ModalLinkSettingsTemplate);
                if (i.Count > 0) {
                    Item j = i.Where(setting => setting["Domain"] == domain).FirstOrDefault();
                    if (j != null) {
                        return new ModalLinkSettings(j);
                    } else {
                        //only try default if it wasn't already attempted
                        if (domain != "default") {
                            Item k = i.Where(setting => setting["Domain"] == "default").FirstOrDefault();
                            if (k != null) {
                                return new ModalLinkSettings(k);
                            }
                        }
                    }
                }
            }

            return null;
        }

        #endregion Static Methods
    }
}
