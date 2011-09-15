using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Web.UI.Sheer;
using Sitecore.Text;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Shell.Framework;
using Sitecore.Web.UI.Pages;
using Sitecore.Web;
using Sitecore.Data;
using Sitecore.IO;
using Sitecore.Resources.Media;
using Sitecore.Resources;
using System.Web.UI;
using Sitecore.Shell;
using System.IO;
using System.Collections.Specialized;
using System.Web;
using System.Drawing;
using Sitecore.Configuration;
using Sitecore.Web.UI.WebControls;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Links;
using BrightcoveSDK;
using BrightcoveSDK.Media;
using scEditor = Sitecore.Shell.Applications.ContentEditor;
using scWeb = Sitecore.Web;
using BrightcoveSDK.SitecoreUtil.Entity;
//using scControls = Sitecore.Controls;

namespace BrightcoveSDK.SitecoreUtil.XmlControls
{
	public class EmbedVideoForm : DialogForm
	{
		// Fields
		protected DataContext VideoDataContext;
		protected DataContext PlayerDataContext;
		protected DataContext PlaylistDataContext;
		protected TreePicker VideoTreeview;
		protected TreePicker PlayerTreeview;
		protected TreeviewEx PlaylistTreeview;
		protected Scrollbox SelectedList;
		protected Checkbox chkAutoStart;
		protected Edit txtBGColor;
		protected Combobox WMode;
        protected Database masterDB;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e) {
			Assert.ArgumentNotNull(e, "e");
			base.OnLoad(e);

            masterDB = Sitecore.Client.ContentDatabase;

            if (!Context.ClientPage.IsEvent) {
				this.Mode = WebUtil.GetQueryString("mo");

				VideoDataContext.GetFromQueryString();
				PlayerDataContext.GetFromQueryString();
				PlaylistDataContext.GetFromQueryString();

				//populate video from querystring
				long vidID = -1;
                if (long.TryParse(WebUtil.GetQueryString("video"), out vidID)) {
                    VideoItem v = VideoLibraryItem.GetVideo(vidID, masterDB);
                    if (v != null) {
                        VideoDataContext.Folder = v.videoItem.ID.ToString();
                    }
				}

				//populate player from querystring
				long playID = -1;
				if (long.TryParse(WebUtil.GetQueryString("player"), out playID)) {
					PlayerItem p = PlayerLibraryItem.GetPlayer(playID, masterDB);
                    if(p != null){
                        PlayerDataContext.Folder = p.playerItem.ID.ToString();
					} 
				}

				//populate playlists from querystring
				List<string> listIDs = WebUtil.GetQueryString("playlisttabs").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
				listIDs.AddRange(WebUtil.GetQueryString("playlistcombo").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList());
				listIDs.Add(WebUtil.GetQueryString("videolist"));
                foreach (string listID in listIDs) {
                    long pID = -1;
                    if (long.TryParse(listID, out pID)) {
						//set the folder so it's opened
						PlaylistItem pl = PlaylistLibraryItem.GetPlaylist(pID, masterDB);
                        if(pl != null){
                            PlaylistDataContext.Folder = pl.playlistItem.ID.ToString();
							//set selected items
							PlaylistTreeview.SelectedIDs.Add(listID);
						} 
					}
				}

				//setup the drop list of wmode
				Item wmodeRoot = masterDB.Items[PlayerDataContext.Root + "/Settings/WMode"];
				string wmode = WebUtil.GetQueryString("wmode");
				if (wmodeRoot != null) {
					foreach (Item wmodeItem in wmodeRoot.Children) {
						ListItem listitem = new ListItem();
						listitem.Header = wmodeItem.Name;
						listitem.Value = wmodeItem.ID.ToString();
						listitem.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("I");
						listitem.Selected = (wmodeItem.DisplayName.ToLower().Equals(wmode.ToLower())) ? true : false;
						WMode.Controls.Add(listitem);
					}
				}

				//get and set the autostart
				string autostart = WebUtil.GetQueryString("autostart");
				try {
					chkAutoStart.Checked = (autostart == "") ? false : bool.Parse(autostart);
				} catch { }

				//get and set the bgcolor
				string bgcolor = HttpUtility.UrlDecode(WebUtil.GetQueryString("bgcolor"));
				try {
					txtBGColor.Value = (bgcolor == "") ? "#ffffff" : bgcolor;
				} catch { txtBGColor.Value = "#ffffff"; }
			}
		}

		/// <summary>
		/// this makes sure you've selected an item from the video treeview
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		protected override void OnOK(object sender, EventArgs args) {
			Assert.ArgumentNotNull(sender, "sender");
			Assert.ArgumentNotNull(args, "args");

			//get the selected playlist
			Item player = masterDB.Items[PlayerTreeview.Value];
			if (player == null || !player.TemplateName.Equals(Constants.PlayerTemplate)) {
				SheerResponse.Alert("Select a player.", new string[0]);
				return;
			}
            
			//get the selected video
			Item video = masterDB.Items[VideoTreeview.Value];
			string videoid = "";
			if (video != null && video.TemplateName.Equals(Constants.VideoTemplate)) {
				VideoItem vid = new VideoItem(video);
                videoid = vid.VideoID.ToString();
			}
			
			//get the selected playlists
			Item[] playlists = this.PlaylistTreeview.GetSelectedItems();
			PlayerItem vpl = new PlayerItem(player);
					
			//set the playlists
			StringBuilder playlistStr = new StringBuilder();
			int plistCount = 0;
			foreach (Item p in playlists) {
                if (p.TemplateName.Equals(Constants.PlaylistTemplate)) {
					PlaylistItem pl = new PlaylistItem(p);
                    if (playlistStr.Length > 0) {
						playlistStr.Append(",");
					}
					playlistStr.Append(pl.PlaylistID.ToString());
					plistCount++;
				}
			}

			//check if the player can handle the playlists selected
			if (vpl.PlaylistType.Equals(PlayerPlaylistType.None) && plistCount > 0) {
				SheerResponse.Alert("This player does not support playlists.\nTo deselect, select the Brightcove Media item.", new string[0]);
				return;
			} else if (vpl.PlaylistType.Equals(PlayerPlaylistType.VideoList) && plistCount > 1) {
				SheerResponse.Alert("This player only supports one playlist.", new string[0]);
				return;
			} else if ((vpl.PlaylistType.Equals(PlayerPlaylistType.VideoList) ||
				vpl.PlaylistType.Equals(PlayerPlaylistType.ComboBox) ||
				vpl.PlaylistType.Equals(PlayerPlaylistType.Tabbed)) && !videoid.Equals("")) {
				SheerResponse.Alert("This player does not support videos. \nTo deselect, select the Brightcove Media item.", new string[0]);
				return;
			}
			
			//build link then send it back
			StringBuilder mediaUrl = new StringBuilder();
			mediaUrl.Append("<bc:VideoPlayerWebControl runat=\"server\" class=\"BrightcoveVideo\" xmlns:bc=\"http://www.sitecore.net/xhtml\" ");
			mediaUrl.Append("player=\"" + vpl.PlayerID + "\" ");
			mediaUrl.Append("autostart=\"" + chkAutoStart.Checked.ToString().ToLower() + "\" ");
			mediaUrl.Append("bgcolor=\"" + txtBGColor.Value + "\" ");
			mediaUrl.Append("wmode=\"" + WMode.SelectedItem.Header + "\" ");
			
			//determine what kind of playlist
			if (vpl.PlaylistType.Equals(PlayerPlaylistType.ComboBox)) {
				mediaUrl.Append("playlistcombo=\"" + playlistStr.ToString() + "\" ");
			}else if (vpl.PlaylistType.Equals(PlayerPlaylistType.Tabbed)){
				mediaUrl.Append("playlisttabs=\"" + playlistStr.ToString() + "\" "); 
			}else if (vpl.PlaylistType.Equals(PlayerPlaylistType.VideoList)){
				mediaUrl.Append("videolist=\"" + playlistStr.ToString() + "\" "); 	
			}else if (vpl.PlaylistType.Equals(PlayerPlaylistType.None)){
				mediaUrl.Append("video =\"" + videoid + "\" ");
			}
			
			//close the tag
			mediaUrl.Append("></bc:VideoPlayer>");
			
			//send it back
			if (this.Mode == "webedit") {
				SheerResponse.SetDialogValue(StringUtil.EscapeJavascriptString(mediaUrl.ToString()));
				base.OnOK(sender, args);
			} else {
				SheerResponse.Eval("scClose(" + StringUtil.EscapeJavascriptString(mediaUrl.ToString()) + "," + StringUtil.EscapeJavascriptString(player.DisplayName) + ")");
			}
		}

		// Properties
		protected string Mode {
			get {
				string str = StringUtil.GetString(base.ServerProperties["Mode"]);
				if (!string.IsNullOrEmpty(str)) {
					return str;
				}
				return "shell";
			}
			set {
				Assert.ArgumentNotNull(value, "value");
				base.ServerProperties["Mode"] = value;
			}
		}
	}
}
