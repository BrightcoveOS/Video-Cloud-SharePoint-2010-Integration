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
	[ToolboxData("<{0}:RemovePlayer runat=server></{0}:RemovePlayer>")]
	public class RemovePlayer : HyperLink
	{
		[Bindable(true), Category("Appearance"), DefaultValue(""), Localizable(true)]
		public string ControlToManage {
			get {
				return (ViewState["ControlToManage"] == null) ? String.Empty : (String)ViewState["ControlToManage"];
			}
			set {
				ViewState["ControlToManage"] = value;
			}
		}

		protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer) {

			// Wire up the onkeypress event handler to the ChangeBackgroundColor() JavaScript function
			VideoPlayer vp = (VideoPlayer)Page.FindControl(ControlToManage);
			writer.AddAttribute("onclick", "javascript:removePlayer('" + vp.PlayerName + "');return false;");
							
			base.AddAttributesToRender(writer);
		}
	}
}
