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
using BrightcoveSDK.SitecoreUtil.Entity;

namespace BrightcoveSDK.SitecoreUtil.XmlControls
{
	public class InsertVideoForm : DialogForm
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

		//TreePicker = DropTree
		//Combobox = DropList
		//Listview = folder explorer
		//Taskbox = kind of looks like the workbox with list expansion header

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
                    if (p != null) {
                        PlayerDataContext.Folder = p.playerItem.ID.ToString();
                    }
                }
								
				//populate playlists from querystring
				string[] listIDs = WebUtil.GetQueryString("playlists").Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
				foreach (string listID in listIDs) {
                    long pID = -1;
                    if (long.TryParse(listID, out pID)) {
                        //set the folder so it's opened
						PlaylistItem pl = PlaylistLibraryItem.GetPlaylist(pID, masterDB);
                        if (pl != null) {
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
				}catch{}

				//get and set the bgcolor
				string bgcolor = WebUtil.GetQueryString("bgcolor");
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

			//get the selected player
			Item player = masterDB.Items[PlayerTreeview.Value];
			if (player == null || !player.TemplateName.Equals(Constants.PlayerTemplate)) {
				SheerResponse.Alert("Select a player.", new string[0]);
				return;
			}
			PlayerItem vpl = new PlayerItem(player);

			//get the selected video
			Item video = masterDB.Items[VideoTreeview.Value];
			string videoid = "";
			if (video != null && video.TemplateName.Equals(Constants.VideoTemplate)) {
				VideoItem vid = new VideoItem(video);
                videoid = vid.VideoID.ToString(); 
			}
			
			//get the selected playlists
			Item[] playlists = this.PlaylistTreeview.GetSelectedItems();
			
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

			//use settings to determine what kind of modal window to use like thickbox or prettyphoto
			//id = {3EE8D1E1-1421-4546-8127-4D576FB8DA5F}
            ModalLinkSettings settings = ModalLinkSettings.GetModalLinkSettings(HttpContext.Current.Request.Url.Host.ToLower(), masterDB);
            StringBuilder sbAttr = new StringBuilder();
			StringBuilder sbQstring = new StringBuilder();
			if (settings != null) {
				foreach(Item child in settings.thisItem.GetChildren()){
					if(child.TemplateName.Equals(Constants.LinkAttributeTemplate)){
						sbAttr.Append(" " + child["Key"] + "=\"" + child["Value"] + "\"");
					}
					else if(child.TemplateName.Equals(Constants.LinkQuerystringTemplate)){
						sbQstring.Append("&" + child["Key"] + "=" + child["Value"]);
					}
				}
			}
			
			//selected text is the link text
			string selectedText = HttpUtility.UrlDecode(WebUtil.GetQueryString("selectedText"));
			if (selectedText.Contains("href=")) {
				selectedText = selectedText.Split('>')[1];
				selectedText = selectedText.Split('<')[0];
			}
			if (selectedText.Equals("")) {
				VideoItem vd = new VideoItem(video);
				selectedText = "Click To Watch " + vd.VideoName;
			}
			
			//build link then send it back
			StringBuilder mediaUrl = new StringBuilder();
			mediaUrl.Append("<a href=\"/BrightcoveVideo.ashx?video=" + videoid + "&player=" + vpl.PlayerID);
			mediaUrl.Append("&playlists=" + playlistStr.ToString() + "&autoStart=" + chkAutoStart.Checked.ToString().ToLower() + "&bgcolor=" + txtBGColor.Value + "&wmode=" + WMode.SelectedItem.Header);
			mediaUrl.Append(sbQstring.ToString());
            mediaUrl.Append("&height=" + (vpl.Height + 20).ToString() + "&width=" + (vpl.Width + 20).ToString() + "\"" + sbAttr.ToString() + ">" + selectedText + "</a>");
						
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
