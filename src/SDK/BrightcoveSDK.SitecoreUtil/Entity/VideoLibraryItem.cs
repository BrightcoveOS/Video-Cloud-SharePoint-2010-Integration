using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Data;
using System.Web;
using Sitecore.Data.Fields;
using BrightcoveSDK.SitecoreUtil.Extensions;
using BrightcoveSDK.SitecoreUtil.Entity;

namespace BrightcoveSDK.SitecoreUtil
{
	public class VideoLibraryItem
	{
		public Item videoLibraryItem;

		public VideoLibraryItem(Item i) {
			videoLibraryItem = i;
		}

		public List<VideoItem> Videos {
			get {
                List<Item> v = this.videoLibraryItem.ChildrenByTemplateRecursive("Brightcove Video");
				List<VideoItem> videos = new List<VideoItem>();
				foreach (Item i in v) {
					videos.Add(new VideoItem(i));
				}
				return videos;
			}
        }

        #region Static Methods

		public static VideoItem GetVideo(long VideoID) {
            return GetVideo(VideoID, Sitecore.Context.Database);
        }
		public static VideoItem GetVideo(long VideoID, Database DB) {
            Item i = DB.GetItem(Constants.BrightcoveLibID);
            if (i != null) {
                Item j = i.ChildrenByTemplateRecursive(Constants.VideoTemplate).Where(video => video["ID"] == VideoID.ToString()).ToList().FirstOrDefault();
                if (j != null) {
					return new VideoItem(j);
                }
            }
            return null;
        }

        #endregion Static Methods 
    }
}
