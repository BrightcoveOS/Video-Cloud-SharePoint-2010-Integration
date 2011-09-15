using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrightcoveSDK.Extensions;

namespace BrightcoveSDK.Utils
{
	public static class EmbedCode
	{
		#region GetEmbedCode

		//based on just player
		public static string GetVideoPlayerEmbedCode(long PlayerID, long VideoID, int height, int width, string BackgroundColor, bool AutoStart, WMode WMode, string objectTagID) {
			return GetEmbedCode(PlayerID, VideoID, PlayerPlaylistType.None, -1, null, height, width, BackgroundColor, AutoStart, WMode, objectTagID);
		}

		//based on list of ids
		public static string GetTabbedPlayerEmbedCode(long PlayerID, long VideoID, List<long> PlaylistIDs, int height, int width, string BackgroundColor, bool AutoStart, WMode WMode, string objectTagID) {
			return GetEmbedCode(PlayerID, VideoID, PlayerPlaylistType.Tabbed, -1, PlaylistIDs, height, width, BackgroundColor, AutoStart, WMode, objectTagID);
		}

		//based on single playlist
		public static string GetVideoListPlayerEmbedCode(long PlayerID, long VideoID, long PlaylistID, int height, int width, string BackgroundColor, bool AutoStart, WMode WMode, string objectTagID) {
			return GetEmbedCode(PlayerID, VideoID, PlayerPlaylistType.VideoList, PlaylistID, null, height, width, BackgroundColor, AutoStart, WMode, objectTagID);
		}

		//based on video	
		public static string GetComboBoxPlayerEmbedCode(long PlayerID, long VideoID, List<long> PlaylistIDs, int height, int width, string BackgroundColor, bool AutoStart, WMode WMode, string objectTagID) {
			return GetEmbedCode(PlayerID, VideoID, PlayerPlaylistType.ComboBox, -1, PlaylistIDs, height, width, BackgroundColor, AutoStart, WMode, objectTagID);
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
		private static string GetEmbedCode(long PlayerID, long VideoID, PlayerPlaylistType PlaylistType, long PlaylistID, List<long> PlaylistIDs, int height, int width, string BackgroundColor, bool AutoStart, WMode WMode, string objectTagID) {

			StringBuilder embed = new StringBuilder();

			if (PlayerID != null) {
				//this one works
				embed.AppendLine("<!-- Start of Brightcove Player -->");
				embed.AppendLine("<div style=\"display:none\"></div>");
				embed.AppendLine("<!-- By use of this code snippet, I agree to the Brightcove Publisher T and C found at https://accounts.brightcove.com/en/terms-and-conditions/. -->");
				embed.AppendLine("<script language=\"JavaScript\" type=\"text/javascript\" src=\"http://admin.brightcove.com/js/BrightcoveExperiences.js\"></script>");
				embed.AppendLine("<object id=\"" + objectTagID + "\" class=\"BrightcoveExperience\">");
				embed.AppendLine("<param name=\"bgcolor\" value=\"" + BackgroundColor + "\" />");
				embed.AppendLine("<param name=\"width\" value=\"" + width.ToString() + "\" />");
				embed.AppendLine("<param name=\"height\" value=\"" + height.ToString() + "\" />");
				embed.AppendLine("<param name=\"playerID\" value=\"" + PlayerID + "\" />");

				//add in video ids or playlist ids
				if (PlaylistType.Equals(PlayerPlaylistType.None) && VideoID != null) {
					embed.AppendLine("<param name=\"@videoPlayer\" value=\"" + VideoID + "\"/>");
				} else if (PlaylistType.Equals(PlayerPlaylistType.Tabbed) && PlaylistIDs != null) {
					embed.AppendLine("<param name=\"@playlistTabs\" value=\"" + PlaylistIDs.ToDelimString(",") + "\"/>");
				} else if (PlaylistType.Equals(PlayerPlaylistType.ComboBox) && PlaylistIDs != null) {
					embed.AppendLine("<param name=\"@playlistCombo\" value=\"" + PlaylistIDs.ToDelimString(",") + "\"/>");
				} else if (PlaylistType.Equals(PlayerPlaylistType.VideoList) && PlaylistID != -1) {
					embed.AppendLine("<param name=\"@videoList\" value=\"" + PlaylistID.ToString() + "\"/>");
				}

				embed.AppendLine("<param name=\"isVid\" value=\"true\" />");
				embed.AppendLine("<param name=\"autoStart\" value=\"" + AutoStart.ToString().ToLower() + "\" />");
				embed.AppendLine("<param name=\"isUI\" value=\"true\" />");
				embed.AppendLine("<param name=\"dynamicStreaming\" value=\"true\" />");
				embed.AppendLine("<param name=\"wmode\" value=\"" + WMode.ToString() + "\" /> ");
				embed.AppendLine("</object>");
				embed.AppendLine("<script type=\"text/javascript\">brightcove.createExperiences();</script>");
				embed.AppendLine("<!-- End of Brightcove Player -->");
			}
			return embed.ToString();
		}

		#endregion GetEmbedCode
	}
}
