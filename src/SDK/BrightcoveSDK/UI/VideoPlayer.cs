using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using BrightcoveSDK.Utils;

namespace BrightcoveSDK.UI
{
	[DefaultProperty("VideoID")]
	[ParseChildren(true)]
	[ControlBuilderAttribute(typeof(VideoPlayerControlBuilder))]
	[ToolboxData("<{0}:VideoPlayer runat=server></{0}:VideoPlayer>")]
	public class VideoPlayer : WebControl
	{
		#region Properties

		[Bindable(true), Category("Appearance"), DefaultValue(-1), Localizable(true)]
		public long VideoID {
			get {
				return (ViewState["VideoID"] == null) ? -1 : (long)ViewState["VideoID"];
			}set {
				ViewState["VideoID"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(-1), Localizable(true)]
		public long PlayerID {
			get {
				return (ViewState["PlayerID"] == null) ? -1 : (long)ViewState["PlayerID"];
			}set {
				ViewState["PlayerID"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue("#ffffff"), Localizable(true)]
		public string BackColor {
			get {
				return (ViewState["BackColor"] == null) ? "#ffffff" : (String)ViewState["BackColor"];
			}set {
				ViewState["BackColor"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(0), Localizable(true)]
		public int Width {
			get {
				return (ViewState["Width"] == null) ? 0 : (int)ViewState["Width"];
			}set {
				ViewState["Width"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(0), Localizable(true)]
		public int Height {
			get {
				return (ViewState["Height"] == null) ? 0 : (int)ViewState["Height"];
			}set {
				ViewState["Height"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue("_Player"), Localizable(true)]
		public string PlayerName {
			get {
				return (ViewState["PlayerName"] == null) ? this.ClientID + "_Player" : (String)ViewState["PlayerName"];
			}set {
				ViewState["PlayerName"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(false), Localizable(true)]
		public bool AutoStart {
			get {
				return (ViewState["AutoStart"] == null) ? false : (bool)ViewState["AutoStart"];
			}set {
				ViewState["AutoStart"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(true), Localizable(true)]
		public bool IsVid {
			get {
				return (ViewState["IsVid"] == null) ? true : (bool)ViewState["IsVid"];
			}
			set {
				ViewState["IsVid"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue("transparent"), Localizable(true)]
		public string WMode {
			get {
				return (ViewState["WMode"] == null) ? "Window" : (String)ViewState["WMode"];
			}set {
				ViewState["WMode"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(-1), Localizable(true)]
		public long VideoList {
			get {
				return (ViewState["VideoList"] == null) ? -1 : (long)ViewState["VideoList"];
			}set {
				ViewState["VideoList"] = value;
			}
		}

		/// <summary>
		/// The Resources files include two javascript from brightcove and one embedded javascript file that are used to create the video player on page load.
		/// </summary>
		[Bindable(true), Category("Appearance"), DefaultValue(true), Localizable(true)]
		public bool IncludeJSResources {
			get {
				return (ViewState["IncludeJSResources"] == null) ? true : (bool)ViewState["IncludeJSResources"];
			}
			set {
				ViewState["IncludeJSResources"] = value;
			}
		}

		private List<PlaylistTab> _PlaylistTabs = new List<PlaylistTab>();

		protected string PlaylistTabString {
			get {
				StringBuilder sb = new StringBuilder();
				foreach (PlaylistTab p in PlaylistTabs) {
					if (sb.Length > 0) {
						sb.Append(",");
					}
					sb.Append(p.Value.ToString());
				}
				return sb.ToString();
			}
		}

		/// <summary>
		/// A collection of Playlist IDs for Tabs
		/// </summary>
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public List<PlaylistTab> PlaylistTabs {
			get {
				return _PlaylistTabs;
			}
		}

		private List<PlaylistCombo> _PlaylistCombos = new List<PlaylistCombo>();
	
		protected string PlaylistComboString {
			get {
				StringBuilder sb = new StringBuilder();
				char[] comma = { ',' };
				foreach (PlaylistCombo p in PlaylistCombos) {
					if (sb.Length > 0) {
						sb.Append(",");
					}
					sb.Append(p.Value.ToString());
				}
				return sb.ToString();
			}
		}
		
		/// <summary>
		/// A collection of Playlist IDs for the Combo Box
		/// </summary>
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public List<PlaylistCombo> PlaylistCombos {
			get {
				return _PlaylistCombos;
			}
		}

		#endregion Properties

		protected override void OnPreRender(EventArgs e) {

			//only if you're not going to add them yourself.
			if (IncludeJSResources) {
				//Add Brightcove experiences javascript
				Page.ClientScript.RegisterClientScriptInclude("BCExperiences", "http://admin.brightcove.com/js/BrightcoveExperiences.js");
			}
			base.OnPreRender(e); 
        }

		/// <summary>
		/// Modifies the container to 
		/// </summary>
		/// <param name="writer"></param>
		public override void RenderBeginTag(HtmlTextWriter writer) {

			//Add writer attributes
			writer.AddAttribute("id", "VideoPlayer_" + this.ClientID);

			//Write writer content
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
		}

		/// <summary>
		/// Creates the JavaScript block and Flash div controls and adds them to the page
		/// </summary>
		protected override void CreateChildControls() {

			//Add the flash content panel to the page
			this.Controls.Clear();

			Panel vp = new Panel();
			vp.ID = this.ClientID;
			this.Controls.Add(vp);

			if (!VideoID.Equals(-1) || !PlayerID.Equals(-1)) {

				//wmode 
				WMode qWMode = BrightcoveSDK.WMode.Transparent;
				if (WMode.Equals(BrightcoveSDK.WMode.Opaque.ToString().ToLower()))
					qWMode = BrightcoveSDK.WMode.Opaque;
				if (WMode.Equals(BrightcoveSDK.WMode.Window.ToString().ToLower()))
					qWMode = BrightcoveSDK.WMode.Window;
				
				string uniqueID = "video_" + DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss.FFFF");
				Literal litScript = new Literal();
				if (PlaylistTabs.Any()) {
					litScript.Text = EmbedCode.GetTabbedPlayerEmbedCode(PlayerID, VideoID, PlaylistTabs.GetValues(), Height, Width, BackColor, AutoStart, qWMode, uniqueID);
				} else if (PlaylistCombos.Any()) {
					litScript.Text = EmbedCode.GetComboBoxPlayerEmbedCode(PlayerID, VideoID, PlaylistCombos.GetValues(), Height, Width, BackColor, AutoStart, qWMode, uniqueID);
				} else if (VideoList != -1) {
					EmbedCode.GetVideoListPlayerEmbedCode(PlayerID, VideoID, VideoList, Height, Width, BackColor, AutoStart, qWMode, uniqueID);
				} else {
					litScript.Text = EmbedCode.GetVideoPlayerEmbedCode(PlayerID, VideoID, Height, Width, BackColor, AutoStart, qWMode, uniqueID);
				}
				
				Controls.Add(litScript);
			}
		}
	}

	public class VideoPlayerControlBuilder : ControlBuilder
	{
		public override Type GetChildControlType(String tagName, IDictionary attributes) {

			if (String.Compare(tagName, "PlaylistTab", true) == 0) {
				return typeof(PlaylistTab);
			}
			else if (String.Compare(tagName, "PlaylistCombo", true) == 0) {
				return typeof(PlaylistCombo);
			}

			return null;
		}
	}
	
	public class PlaylistTab
	{
		private long _value;
		public long Value {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}

		public PlaylistTab(long Val) {
			_value = Val;
		}
	}

	public class PlaylistCombo
	{
		private long _value;
		public long Value {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}

		public PlaylistCombo(long Val) {
			_value = Val;
		}
	}

	public static class PlaylistComboExtensions
	{
		public static List<long> GetValues(this List<PlaylistCombo> c) {
			List<long> i = new List<long>();
			foreach(PlaylistCombo p in c){
				i.Add(p.Value);
			}
			return i;
		}
	}

	public static class PlaylistTabExtensions
	{
		public static List<long> GetValues(this List<PlaylistTab> c) {
			List<long> i = new List<long>();
			foreach (PlaylistTab p in c) {
				i.Add(p.Value);
			}
			return i;
		}
	}
}
