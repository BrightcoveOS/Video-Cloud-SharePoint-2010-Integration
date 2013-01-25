using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Sitecore.Diagnostics;
using Sitecore;
using Sitecore.Events;
using Sitecore.Resources.Media;
using Sitecore.Web;
using Sitecore.Configuration;
using Sitecore.SecurityModel;
using System.IO;
using BrightcoveSDK.UI;
using Sitecore.Data.Items;
using System.Web.UI;
using Sitecore.Data;
using BrightcoveSDK.SitecoreUtil.Entity;
using BrightcoveSDK.Extensions;
using System.Collections.Specialized;
using BrightcoveSDK.Utils;

namespace BrightcoveSDK.SitecoreUtil.Handlers
{
	public class BrightcoveVideoHandler : System.Web.UI.Page, IHttpHandler
	{		
		// Override the ProcessRequest method.
		public void ProcessRequest(HttpContext context) {
			Assert.ArgumentNotNull(context, "context");
			DoProcessRequest(context);
		}
		
		// Methods
		private void DoProcessRequest(HttpContext context) {
			Assert.ArgumentNotNull(context, "context");
			
			//return DoProcessRequest(context, request, media);
			if (context != null) {
				NameValueCollection nvc = context.Request.QueryString;
				
				//player
				long qPlayer = 0;
				if(nvc.HasKey("player")) 
					long.TryParse(nvc.Get("player"), out qPlayer);
				PlayerItem p = PlayerLibraryItem.GetPlayer(qPlayer);
                if (p == null) { context.Response.Write("The player is null"); return; }

				//video 
				long qVideo = 0;
				if(nvc.HasKey("video")) 
					long.TryParse(nvc.Get("video"), out qVideo);
								
				//playlist ids
				long qPlaylist = 0;
				List<long> qPlaylistIds = new List<long>();
				if(nvc.HasKey("playlists")) {
					string[] playlistids = context.Request.QueryString.Get("playlists").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
					//if you only want a single
					if (p.PlaylistType.Equals(PlayerPlaylistType.None)) {
						if (playlistids.Any())
							long.TryParse(playlistids[0], out qPlaylist);
					} else { //you're looking for multiple
						foreach (string id in playlistids) {
							long plist = -1;
							if (long.TryParse(id, out plist))
								qPlaylistIds.Add(plist);
						}
					}
				}

				//auto start
				bool qAutoStart = false;
				if(nvc.HasKey("autoStart")) 
					bool.TryParse(nvc.Get("autoStart"), out qAutoStart);
				
				//bg color
				string qBgColor = (nvc.HasKey("bgcolor")) ? nvc.Get("bgcolor") : "";
				
				//wmode 
				WMode qWMode = WMode.Transparent;
				if (nvc.HasKey("wmode")) {
					string wmode = nvc.Get("wmode").ToLower();
					if (wmode.Equals(WMode.Opaque.ToString().ToLower()))
						qWMode = WMode.Opaque;
					if (wmode.Equals(WMode.Window.ToString().ToLower()))
						qWMode = WMode.Window;
				}
				
                StringBuilder sb = new StringBuilder();
				sb.AppendLine("<html><head>");
                sb.AppendLine("</head><body>");
				
				string uniqueID = "video_" + DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss.FFFF");
				switch (p.PlaylistType) {
                    case PlayerPlaylistType.VideoList:
                        sb.AppendLine(EmbedCode.GetVideoListPlayerEmbedCode(qPlayer, qVideo, qPlaylist, p.Height, p.Width, qBgColor, qAutoStart, qWMode, uniqueID));
						break;
                    case PlayerPlaylistType.Tabbed:
						sb.AppendLine(EmbedCode.GetTabbedPlayerEmbedCode(qPlayer, qVideo, qPlaylistIds, p.Height, p.Width, qBgColor, qAutoStart, qWMode, uniqueID));
						break;
					case PlayerPlaylistType.ComboBox:
                        sb.AppendLine(EmbedCode.GetComboBoxPlayerEmbedCode(qPlayer, qVideo, qPlaylistIds, p.Height, p.Width, qBgColor, qAutoStart, qWMode, uniqueID));
						break;
					case PlayerPlaylistType.None:
						sb.AppendLine(EmbedCode.GetVideoPlayerEmbedCode(qPlayer, qVideo, p.Height, p.Width, qBgColor, qAutoStart, qWMode, uniqueID));
						break;
				}

                sb.AppendLine("</body></html>");
				context.Response.Write(sb.ToString());
			}
			return;
		}

		// Override the IsReusable property.
		public bool IsReusable {
			get { return true; }
		}
	}
}

