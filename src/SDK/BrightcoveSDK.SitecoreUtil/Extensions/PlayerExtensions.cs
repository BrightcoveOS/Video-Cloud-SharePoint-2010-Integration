using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrightcoveSDK.Extensions;
using BrightcoveSDK.SitecoreUtil;
using Sitecore.Data;
using Sitecore.Data.Items;
using BrightcoveSDK.SitecoreUtil.Entity;

namespace BrightcoveSDK.SitecoreUtil.Extensions
{
	public static class PlayerExtensions
	{
		private static string CreateEmbedID() {
			Random r = new Random();
			return "BrightcoveVideo_" + r.Next(1001).ToString();
		}

        #region GetEmbedCode
		
        //based on just player
		public static string GetEmbedCode(this PlayerItem player) {
			return GetEmbedCode(player, "#ffffff");
		}
		public static string GetEmbedCode(this PlayerItem player, string BackgroundColor) {
			return GetEmbedCode(player, BackgroundColor, false);
		}
		public static string GetEmbedCode(this PlayerItem player, string BackgroundColor, bool AutoStart) {
			return GetEmbedCode(player, BackgroundColor, AutoStart, WMode.Window);
		}
		public static string GetEmbedCode(this PlayerItem player, string BackgroundColor, bool AutoStart, WMode WMode) {
			return GetEmbedCode(player, BackgroundColor, AutoStart, WMode, CreateEmbedID());
		}
		public static string GetEmbedCode(this PlayerItem player, string BackgroundColor, bool AutoStart, WMode WMode, string objectTagID) {
			return GetEmbedCode(player, null, -1, null, BackgroundColor, AutoStart, WMode, objectTagID);
		}
		
		//based on list of ids
		public static string GetEmbedCode(this PlayerItem player, List<long> PlaylistIDs) {
			return GetEmbedCode(player, PlaylistIDs, "#ffffff");
		}
		public static string GetEmbedCode(this PlayerItem player, List<long> PlaylistIDs, string BackgroundColor) {
			return GetEmbedCode(player, PlaylistIDs, BackgroundColor, false);
		}
		public static string GetEmbedCode(this PlayerItem player, List<long> PlaylistIDs, string BackgroundColor, bool AutoStart) {
			return GetEmbedCode(player, PlaylistIDs, BackgroundColor, AutoStart, WMode.Window);
		}
		public static string GetEmbedCode(this PlayerItem player, List<long> PlaylistIDs, string BackgroundColor, bool AutoStart, WMode WMode) {
			return GetEmbedCode(player, PlaylistIDs, BackgroundColor, AutoStart, WMode, CreateEmbedID());
		}
		public static string GetEmbedCode(this PlayerItem player, List<long> PlaylistIDs, string BackgroundColor, bool AutoStart, WMode WMode, string objectTagID) {
			return GetEmbedCode(player, null, -1, PlaylistIDs, BackgroundColor, AutoStart, WMode, objectTagID);
		}

		//based on single playlist
		public static string GetEmbedCode(this PlayerItem player, long PlaylistID) {
			return GetEmbedCode(player, PlaylistID, "#ffffff");
		}
		public static string GetEmbedCode(this PlayerItem player, long PlaylistID, string BackgroundColor) {
			return GetEmbedCode(player, PlaylistID, BackgroundColor, false);
		}
		public static string GetEmbedCode(this PlayerItem player, long PlaylistID, string BackgroundColor, bool AutoStart) {
			return GetEmbedCode(player, PlaylistID, BackgroundColor, AutoStart, WMode.Window);
		}
		public static string GetEmbedCode(this PlayerItem player, long PlaylistID, string BackgroundColor, bool AutoStart, WMode WMode) {
			return GetEmbedCode(player, PlaylistID, BackgroundColor, AutoStart, WMode, CreateEmbedID());
		}
		public static string GetEmbedCode(this PlayerItem player, long PlaylistID, string BackgroundColor, bool AutoStart, WMode WMode, string objectTagID) {
			return GetEmbedCode(player, null, PlaylistID, null, BackgroundColor, AutoStart, WMode, objectTagID);
		}

		//based on video		
		public static string GetEmbedCode(this PlayerItem player, VideoItem video) {
			return GetEmbedCode(player, video, "#ffffff");
		}
		public static string GetEmbedCode(this PlayerItem player, VideoItem video, string BackgroundColor) {
			return GetEmbedCode(player, video, BackgroundColor, false);
		}
		public static string GetEmbedCode(this PlayerItem player, VideoItem video, string BackgroundColor, bool AutoStart) {
			return GetEmbedCode(player, video, BackgroundColor, AutoStart, WMode.Window);
		}
		public static string GetEmbedCode(this PlayerItem player, VideoItem video, string BackgroundColor, bool AutoStart, WMode WMode) {
			return GetEmbedCode(player, video, BackgroundColor, AutoStart, WMode, CreateEmbedID());
		}
		public static string GetEmbedCode(this PlayerItem player, VideoItem video, string BackgroundColor, bool AutoStart, WMode WMode, string objectTagID) {
			return GetEmbedCode(player, video, -1, null, BackgroundColor, AutoStart, WMode, objectTagID);
		}

		/// <summary>
		/// This will build an html object tag based on the information provided
		/// </summary>
		/// <param name="player">The player defined under BrightcoveSDK.SitecoreUtil</param>
		/// <param name="objectTagID">The HTML Object ID Tag</param>
		/// <param name="video">The video defined under BrightcoveSDK.SitecoreUtil</param>
		/// <param name="PlaylistID">A Playlist ID for a single playlist video player</param>
		/// <param name="PlaylistIDs">The List of Playlist IDs for a multi playlist video player</param>
		/// <param name="BackgroundColor">The Hex Value in the form: #ffffff</param>
		/// <param name="AutoStart">A flag to cause the video to automatically start playing</param>
		/// <param name="WMode">The wmode </param>
		/// <returns></returns>
		private static string GetEmbedCode(this PlayerItem player, VideoItem video, long PlaylistID, List<long> PlaylistIDs, string BackgroundColor, bool AutoStart, WMode WMode, string objectTagID) {

			StringBuilder embed = new StringBuilder();

			if (player != null) {
				//this one works
				embed.AppendLine("<!-- Start of Brightcove Player -->");
				embed.AppendLine("");
				embed.AppendLine("<div style=\"display:none\"></div>");
				embed.AppendLine("<script language=\"JavaScript\" type=\"text/javascript\" src=\"http://admin.brightcove.com/js/BrightcoveExperiences.js\"></script>");
				embed.AppendLine("<object id=\"" + objectTagID + "\" class=\"BrightcoveExperience\">");
				embed.AppendLine("<param name=\"bgcolor\" value=\"" + BackgroundColor + "\" />");
				embed.AppendLine("<param name=\"width\" value=\"" + player.Width.ToString() + "\" />");
				embed.AppendLine("<param name=\"height\" value=\"" + player.Height.ToString() + "\" />");
				embed.AppendLine("<param name=\"playerID\" value=\"" + player.PlayerID + "\" />");

				//add in video ids or playlist ids
				if (player.PlaylistType.Equals(PlayerPlaylistType.None) && video != null) {
					embed.AppendLine("<param name=\"@videoPlayer\" value=\"" + video.VideoID + "\"/>");
				} else if (player.PlaylistType.Equals(PlayerPlaylistType.Tabbed) && PlaylistIDs != null) {
					embed.AppendLine("<param name=\"@playlistTabs\" value=\"" + PlaylistIDs.ToDelimString(",") + "\"/>");
				} else if (player.PlaylistType.Equals(PlayerPlaylistType.ComboBox) && PlaylistIDs != null) {
					embed.AppendLine("<param name=\"@playlistCombo\" value=\"" + PlaylistIDs.ToDelimString(",") + "\"/>");
				} else if (player.PlaylistType.Equals(PlayerPlaylistType.VideoList) && PlaylistID != -1) {
					embed.AppendLine("<param name=\"@videoList\" value=\"" + PlaylistID.ToString() + "\"/>");
				}

				embed.AppendLine("<param name=\"isVid\" value=\"true\" />");
				embed.AppendLine("<param name=\"autoStart\" value=\"" + AutoStart.ToString().ToLower() + "\" />");
				embed.AppendLine("<param name=\"isUI\" value=\"true\" />");
				embed.AppendLine("<param name=\"dynamicStreaming\" value=\"true\" />");
				embed.AppendLine("<param name=\"wmode\" value=\"" + WMode.ToString() + "\" /> ");
				embed.AppendLine("</object>");
				embed.AppendLine("");
				embed.AppendLine("<!-- End of Brightcove Player -->");
			}
			return embed.ToString();
        }

        #endregion GetEmbedCode
    }
}
