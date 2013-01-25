using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrightcoveSDK.Media;
using Sitecore.Data.Items;
using System.Collections;
using System.Web;
using System.Text.RegularExpressions;
using BrightcoveSDK.SitecoreUtil.Entity.Container;
using BrightcoveSDK.SitecoreUtil.Entity;

namespace BrightcoveSDK.SitecoreUtil.Extensions
{
	public static class BCVideoExtensions
	{

		public static UpdateInsertPair<VideoItem> ImportToSitecore(this AccountItem account, BCVideo Video, UpdateType utype) {
			List<BCVideo> Videos = new List<BCVideo>();
			Videos.Add(Video);
			return ImportToSitecore(account, Videos, utype);
		}

		/// <summary>
		/// This method will import / update a list of videos into the Brightcove Video Library
		/// </summary>
		/// <param name="Videos">
		/// The Videos to import / update
		/// </param>
		/// <returns>
		/// returns a list of the new videos imported
		/// </returns>
		public static UpdateInsertPair<VideoItem> ImportToSitecore(this AccountItem account, List<BCVideo> Videos, UpdateType utype) {

			UpdateInsertPair<VideoItem> uip = new UpdateInsertPair<VideoItem>();

			//set all BCVideos into hashtable for quick access
			Hashtable ht = new Hashtable();
			foreach (VideoItem exVid in account.VideoLib.Videos) {
				if (!ht.ContainsKey(exVid.VideoID.ToString())) {
					//set as string, Item pair
					ht.Add(exVid.VideoID.ToString(), exVid);
				}
			}

			//Loop through the data source and add them
			foreach (BCVideo vid in Videos) {

				try {
					//remove access filter
					using (new Sitecore.SecurityModel.SecurityDisabler()) {

						VideoItem currentItem;

						//if it exists then update it
						if (ht.ContainsKey(vid.id.ToString()) && (utype.Equals(UpdateType.BOTH) || utype.Equals(UpdateType.UPDATE))) {
							currentItem = (VideoItem)ht[vid.id.ToString()];

							//add it to the new items
							uip.UpdatedItems.Add(currentItem);

							using (new EditContext(currentItem.videoItem, true, false)) {
								SetVideoFields(ref currentItem.videoItem, vid);
							}
						}
							//else just add it
						else if (!ht.ContainsKey(vid.id.ToString()) && (utype.Equals(UpdateType.BOTH) || utype.Equals(UpdateType.NEW))) {
							//Create new item
							TemplateItem templateType = account.Database.Templates["Modules/Brightcove/Brightcove Video"];

							currentItem = new VideoItem(account.VideoLib.videoLibraryItem.Add(vid.name.StripInvalidChars(), templateType));

							//add it to the new items
							uip.NewItems.Add(currentItem);

							using (new EditContext(currentItem.videoItem, true, false)) {
								SetVideoFields(ref currentItem.videoItem, vid);
							}
						}
					}
				} catch (System.Exception ex) {
					//HttpContext.Current.Response.Write(vid.name + "<br/>");
					throw new Exception("Failed on video: " + vid.name + ". " + ex.ToString());
				}
			}

			return uip;
		}

		private static void SetVideoFields(ref Item currentItem, BCVideo vid) {

			//Set the appropriate field values for the new item
			currentItem.Fields["Name"].Value = vid.name;
			currentItem.Fields["Short Description"].Value = vid.shortDescription;
			currentItem.Fields["Long Description"].Value = vid.longDescription;
			currentItem.Fields["Reference Id"].Value = vid.referenceId;
			currentItem.Fields["Economics"].Value = vid.economics.ToString();
			currentItem.Fields["ID"].Value = vid.id.ToString();
			currentItem.Fields["Creation Date"].Value = vid.creationDate.ToDateFieldValue();
			currentItem.Fields["Published Date"].Value = vid.publishedDate.ToDateFieldValue();
			currentItem.Fields["Last Modified Date"].Value = vid.lastModifiedDate.ToDateFieldValue();
			currentItem.Fields["Link URL"].Value = vid.linkURL;
			currentItem.Fields["Link Text"].Value = vid.linkText;
			string taglist = "";
			foreach (string tag in vid.tags) {
				if (taglist.Length > 0) {
					taglist += ",";
				}
				taglist += tag;
			}
			currentItem.Fields["Tags"].Value = taglist;
			currentItem.Fields["Video Still URL"].Value = vid.videoStillURL;
			currentItem.Fields["Thumbnail URL"].Value = vid.thumbnailURL;
			currentItem.Fields["Length"].Value = vid.length;
			currentItem.Fields["Plays Total"].Value = vid.playsTotal.ToString();
			currentItem.Fields["Plays Trailing Week"].Value = vid.playsTrailingWeek.ToString();
		}
	}

}
