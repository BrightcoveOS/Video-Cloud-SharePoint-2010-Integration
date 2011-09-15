using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BrightcoveSDK.UI
{
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:UpdatePlayer runat=server></{0}:UpdatePlayer>")]
	public class UpdatePlayer : HyperLink
	{
		#region Properties

		/// <summary>
		/// This will give access to the settings on the specified control.
		/// </summary>
		[Bindable(true), Category("Appearance"), DefaultValue(""), Localizable(true)]
		public string ControlToManage {
			get {
				return (ViewState["ControlToManage"] == null) ? String.Empty : (String)ViewState["ControlToManage"];
			}
			set {
				ViewState["ControlToManage"] = value;
			}
		}
		
		[Bindable(true), Category("Appearance"), DefaultValue(-1), Localizable(true)]
		public long VideoID {
			get {
				return (ViewState["VideoID"] == null) ? -1 : (long)ViewState["VideoID"];
			}
			set {
				ViewState["VideoID"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(-1), Localizable(true)]
		public long PlayerID {
			get {
				return (ViewState["PlayerID"] == null) ? -1 : (long)ViewState["PlayerID"];
			}
			set {
				ViewState["PlayerID"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue("#000000"), Localizable(true)]
		public string BackColor {
			get {
				return (ViewState["BackColor"] == null) ? String.Empty : (String)ViewState["BackColor"];
			}
			set {
				ViewState["BackColor"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(0), Localizable(true)]
		public int Width {
			get {
				return (ViewState["Width"] == null) ? 0 : (int)ViewState["Width"];
			}
			set {
				ViewState["Width"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(0), Localizable(true)]
		public int Height {
			get {
				return (ViewState["Height"] == null) ? 0 : (int)ViewState["Height"];
			}
			set {
				ViewState["Height"] = value;
			}
		}
		
		[Bindable(true), Category("Appearance"), DefaultValue(false), Localizable(true)]
		public bool AutoStart {
			get {
				return (ViewState["AutoStart"] == null) ? false : (bool)ViewState["AutoStart"];
			}
			set {
				ViewState["AutoStart"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(""), Localizable(true)]
		public bool IsVid {
			get {
				return (ViewState["IsVid"] == null) ? false : (bool)ViewState["IsVid"];
			}
			set {
				ViewState["IsVid"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue("transparent"), Localizable(true)]
		public string WMode {
			get {
				return (ViewState["WMode"] == null) ? String.Empty : (String)ViewState["WMode"];
			}
			set {
				ViewState["WMode"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(-1), Localizable(true)]
		public long VideoList {
			get {
				return (ViewState["VideoList"] == null) ? -1 : (long)ViewState["VideoList"];
			}
			set {
				ViewState["VideoList"] = value;
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
		
		protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer) {
			
			// Wire up the onkeypress event handler to the ChangeBackgroundColor() JavaScript function
			Control c = Page.FindControl(ControlToManage);
			StringBuilder onClickCall = new StringBuilder();

			//make sure it's the right object
			if (c != null && c.GetType().ToString().Equals("BrightcoveSDK.UI.VideoPlayer")) {
				VideoPlayer vp = (VideoPlayer)c;
			
				//check video id
				if (VideoID.Equals(-1)) {
					onClickCall.Append(vp.VideoID);
				}
				else {
					onClickCall.Append(VideoID);
				}
				onClickCall.Append(", ");

				//check for player id
				if (PlayerID.Equals(-1)) {
					onClickCall.Append(vp.PlayerID);
				}
				else {
					onClickCall.Append(PlayerID);
				}

				//check for player name
				onClickCall.Append(", '" + vp.PlayerName + "', ");

				//check for auto start
				if (AutoStart.Equals(false)) {
					onClickCall.Append(vp.AutoStart.ToString().ToLower());
				}
				else {
					onClickCall.Append(AutoStart.ToString().ToLower());
				}
				onClickCall.Append(", '");

				//check for back color
				if (BackColor.Equals("#000000")) {
					onClickCall.Append(vp.BackColor);
				}
				else {
					onClickCall.Append(BackColor);
				}
				onClickCall.Append("', ");

				//check for width
				if (Width.Equals(0)) {
					onClickCall.Append(vp.Width.ToString());
				}
				else {
					onClickCall.Append(Width.ToString());
				}
				onClickCall.Append(", ");

				//check for Height
				if (Height.Equals(0)) {
					onClickCall.Append(vp.Height.ToString());
				}
				else {
					onClickCall.Append(Height.ToString());
				}
				onClickCall.Append(", ");

				//check for IsVid
				if (IsVid.Equals(true)) {
					onClickCall.Append(vp.IsVid.ToString().ToLower());
				}
				else {
					onClickCall.Append(IsVid.ToString().ToLower());
				}
				onClickCall.Append(", '");

				//check for WMode
				if (WMode.Equals("")) {
					onClickCall.Append(vp.WMode);
				}
				else {
					onClickCall.Append(WMode);
				}

				//append for ClientID
				onClickCall.Append("', '" + vp.ClientID + "', '" + PlaylistTabString + "', '" + PlaylistComboString + "', '" + VideoList.ToString() + "'");

				writer.AddAttribute("onclick", "javascript:addPlayer(" + onClickCall.ToString() + ");return false;");

				base.AddAttributesToRender(writer);
			}
			else {
				StringBuilder error = new StringBuilder();
				error.Append("The ControlToManage must be specified or point to a valid VideoPlayer.");

				if(c == null){
					error.Append("\n The ControlToManage was null.");
				}
				else if (!c.GetType().ToString().Equals("BrightcoveSDK.UI.VideoPlayer")) {
					error.Append("\n The ControlToManage type was " + c.GetType().ToString() + ".");
				}
				throw new ArgumentException(error.ToString());
			}
		}
	}
}
