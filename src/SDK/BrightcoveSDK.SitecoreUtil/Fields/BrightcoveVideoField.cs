using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore;
using System.Web.UI;
using Sitecore.Text;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Data.Items;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;

namespace BrightcoveSDK.SitecoreUtil.Fields
{
	internal class BrightcoveVideoField : Sitecore.Web.UI.HtmlControls.Control
	{

		#region Varibles

		private string fieldName = string.Empty;
		private string itemID = string.Empty;
		private string source = string.Empty;
		private bool isEvent = true;

		#endregion Variables

		#region Properties

		public string FieldName {
			get {
				return fieldName;
			}
			set {
				fieldName = value;
			}
		}

		public string ItemID {
			get {
				return itemID;
			}
			set {
				itemID = value;
			}
		}

		public string Source {
			get {
				return StringUtil.GetString(source);
			}
			set {
				if (value.IndexOf('&') > -1) {
					source = value.Substring(0, value.IndexOf('&'));
					if (value.ToLower().IndexOf("separator", value.IndexOf('&')) > -1) {
						string[] parameters = value.Split('&');
						for (int i = 1; i < parameters.Length; i++) {
							if (parameters[i].ToLower().IndexOf("separator") > -1) {
								Separator = parameters[i].Substring((parameters[i].IndexOf("=") > -1) ? parameters[i].IndexOf("=") + 1 : 0);
							}
						}
					}
				} else {
					source = value;
				}
			}
		}

		public string Separator {
			get {
				if (ViewState[this.ClientID + "_separator"] != null) {
					return ViewState[this.ClientID + "_separator"].ToString();
				} else {
					return ", ";
				}
			}
			set {
				ViewState[this.ClientID + "_separator"] = value;
			}
		}

		public bool TrackModified {
			get {
				return base.GetViewStateBool("TrackModified", true);
			}
			set {
				base.SetViewStateBool("TrackModified", value, true);
			}
		}
		#endregion Properties

		public BrightcoveVideoField() {
			this.Class = "scContentControl";
			base.Activation = true;
		}

		protected override void OnLoad(EventArgs e) {
			Assert.ArgumentNotNull(e, "e");
			
			if (!Sitecore.Context.ClientPage.IsEvent) {
				
				isEvent = false;
				Sitecore.Shell.Applications.ContentEditor.Checklist list = new Sitecore.Shell.Applications.ContentEditor.Checklist();
				this.Controls.Add(list);
				list.ID = GetID("list");
				list.Source = this.Source;
				list.ItemID = ItemID;
				list.FieldName = FieldName;
				list.TrackModified = TrackModified;
				list.Disabled = this.Disabled;
				list.Value = this.Value;


				Sitecore.Shell.Applications.ContentEditor.Text text = new Sitecore.Shell.Applications.ContentEditor.Text();
				this.Controls.AddAt(0, text);
				text.ID = GetID("text");
				text.ReadOnly = true;
				text.Disabled = this.Disabled;

				Button b = new Button();
				this.Controls.Add(b);
				b.ID = GetID("button");
				b.Disabled = this.Disabled;
				b.Click = "checklist:selectplayer";
				b.Header = "click me";

				this.Controls.Add(new LiteralControl(Sitecore.Resources.Images.GetSpacer(0x18, 16)));
			} else {
				Sitecore.Shell.Applications.ContentEditor.Checklist list = FindControl(GetID("list")) as Sitecore.Shell.Applications.ContentEditor.Checklist;
				if (list != null) {
					ListString valueList = new ListString();
					foreach (DataChecklistItem item in list.Items) {
						if (item.Checked) {
							valueList.Add(item.ItemID);
						}
					}
					if (this.Value != valueList.ToString()) {
						this.TrackModified = list.TrackModified;
						//this.SetModified();
					}
					this.Value = valueList.ToString();
				}
			}
			base.OnLoad(e);

			if (!Sitecore.Web.UI.XamlSharp.Xaml.XamlControl.AjaxScriptManager.IsEvent) {
				SheerResponse.Alert("hey2", true);
			}
		}

		[HandleMessage("checklist:selectplayer")]
		protected void MessageButton(Message message) {
		
			string url = "/sitecore/shell/default.aspx?xmlcontrol=RichText.InsertVideo&la=";// + scLanguage + "&video=" + videoid + "&player=" + playerid + "&playlists=" + playlistids + "&selectedText=" + escape(html) + "&wmode=" + wmode + "&bgcolor=" + bgcolor + "&autostart=" + autostart
			SheerResponse.ShowModalDialog(url, "600", "500", "selectplayer", true);
			
			//SheerResponse.Alert("hey", true);
		}

		//protected override void OK_Click() {
		//    ReadValuesIntoResponse();
		//    SheerResponse.SetDialogValue(this.ScheduleValue.XmlValue.ToString());
		//    base.OK_Click();
		//}

		//if (!Sitecore.Web.UI.XamlSharp.Xaml.XamlControl.AjaxScriptManager.IsEvent) {
		//    Sitecore.Shell.Applications.ContentEditor.Checklist list = FindControl(GetID("list")) as Sitecore.Shell.Applications.ContentEditor.Checklist;
		//    Sitecore.Shell.Applications.ContentEditor.Text text = FindControl(GetID("text")) as Sitecore.Shell.Applications.ContentEditor.Text;
			
		//    list.CheckAll();
		//}

		public override void HandleMessage(Sitecore.Web.UI.Sheer.Message message) {

			if (message["id"] == this.ID) {
				Sitecore.Shell.Applications.ContentEditor.Checklist list = FindControl(GetID("list")) as Sitecore.Shell.Applications.ContentEditor.Checklist;
				Sitecore.Shell.Applications.ContentEditor.Text text = FindControl(GetID("text")) as Sitecore.Shell.Applications.ContentEditor.Text;
				if (list != null) {
					string messageText;
					if ((messageText = message.Name) == null) {
						return;
					}

					if (messageText != "checklist:checkall") {
						if (messageText == "checklist:uncheckall") {
							list.UncheckAll();
						} else if (messageText == "checklist:invert") {
							list.Invert();
						}
					}
					else if (messageText != "checklist:selectplayer") {
						list.CheckAll();
					} else if (messageText != "checklist:clear") {
					} else {
						list.CheckAll();
					}
				}
			}

			base.HandleMessage(message);
		}

		protected override void OnPreRender(EventArgs e) {
			Sitecore.Shell.Applications.ContentEditor.Checklist list = FindControl(GetID("list")) as Sitecore.Shell.Applications.ContentEditor.Checklist;
			Sitecore.Shell.Applications.ContentEditor.Text text = FindControl(GetID("text")) as Sitecore.Shell.Applications.ContentEditor.Text;
			FillTextBox(text, list);

			if (!isEvent) {
				if (list != null) {
					for (int i = 0; i < list.Items.Length; i++) {
						list.Items[i].ServerProperties["Click"] = string.Format("{0}.ListItemClick", this.ID);
					}
				}
			}

			base.OnPreRender(e);
		}

		private void FillTextBox(Sitecore.Shell.Applications.ContentEditor.Text text, Sitecore.Web.UI.HtmlControls.Checklist list) {
			foreach (ChecklistItem i in list.Items) {
				if (i.Checked) {
					if (text.Value.Length > 0) {
						text.Value += Separator;
					}
					text.Value += i.Name;
				}
			}
		}

		public void ListItemClick() {
			Sitecore.Shell.Applications.ContentEditor.Checklist list = FindControl(GetID("list")) as Sitecore.Shell.Applications.ContentEditor.Checklist;
			Sitecore.Context.ClientPage.ClientResponse.SetReturnValue(true);
		}
	}
}
