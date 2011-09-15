using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BrightcoveSDK.Media;
using BrightcoveSDK.Containers;

namespace BrightcoveSDK.UI
{
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:VideoAddUpdate runat=server></{0}:VideoAddUpdate>")]
	public class VideoAddUpdate : CompositeControl
	{
		#region Properties

		private TextBox videoName;
		private TextBox referenceID;
		private TextBox shortDescription;
		private TextBox longDescription;
		private TextBox tags;
		private DropDownList economics;
		private FileUpload videoFile;

		public string VideoName {
			get {
                EnsureChildControls();
                return videoName.Text;
            }
            set {
                EnsureChildControls();
                videoName.Text = value;
            }
		}

		[Bindable(true), Category("Appearance"), DefaultValue(true), Localizable(true)]
		public bool ShowReferenceID {
			get {
				return (ViewState["ShowReferenceID"] == null) ? true : (bool)ViewState["ShowReferenceID"];
			}
			set {
				ViewState["ShowReferenceID"] = value;
			}
		}

		public string ReferenceID {
			get {
				EnsureChildControls();
				return referenceID.Text;
			}
			set {
				EnsureChildControls();
				referenceID.Text = value;
			}
		}
				
		public string ShortDescription {
			get {
				EnsureChildControls();
				return shortDescription.Text;
			}
			set {
				EnsureChildControls();
				shortDescription.Text = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(true), Localizable(true)]
		public bool ShowLongDescription {
			get {
				return (ViewState["ShowLongDescription"] == null) ? true : (bool)ViewState["ShowLongDescription"];
			}
			set {
				ViewState["ShowLongDescription"] = value;
			}
		}

		public string LongDescription {
			get {
				EnsureChildControls();
				return longDescription.Text;
			}
			set {
				EnsureChildControls();
				longDescription.Text = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(true), Localizable(true)]
		public bool ShowTags {
			get {
				return (ViewState["ShowTags"] == null) ? true : (bool)ViewState["ShowTags"];
			}
			set {
				ViewState["ShowTags"] = value;
			}
		}

		public BCCollection<string> Tags {
			get {
				EnsureChildControls();
				string[] splitr = {","};
				BCCollection<string> bc = new BCCollection<string>();
				foreach(string s in tags.Text.Split(splitr, StringSplitOptions.RemoveEmptyEntries)){
					bc.Add(s);
				}
				return bc;	
			}
			set {
				EnsureChildControls();
				tags.Text = value.ToDelimitedString(",");
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(true), Localizable(true)]
		public bool ShowEconomics {
			get {
				return (ViewState["ShowEconomics"] == null) ? true : (bool)ViewState["ShowEconomics"];
			}
			set {
				ViewState["ShowEconomics"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(BCVideoEconomics.AD_SUPPORTED), Localizable(true)]
		public BCVideoEconomics Economics {
			get {
				EnsureChildControls();
				if(economics.SelectedValue.Equals(BCVideoEconomics.AD_SUPPORTED.ToString())){
					return BCVideoEconomics.AD_SUPPORTED;
				}
				else {
					return BCVideoEconomics.FREE;
				}	
			}
			set {
				EnsureChildControls();
				foreach(ListItem li in economics.Items){
					if (li.Value.Equals(value.ToString())) {
						li.Selected = true;
					}
					else {
						li.Selected = false;
					}
				}	
			}
		}
				
		#endregion Properties

		#region FileUpload Accessors

		[Bindable(true), Category("Appearance"), DefaultValue(true), Localizable(true)]
		public bool ShowFileUpload {
			get {
				return (ViewState["ShowFileUpload"] == null) ? true : (bool)ViewState["ShowFileUpload"];
			}
			set {
				ViewState["ShowFileUpload"] = value;
			}
		}

		public string FileName {
			get {
				EnsureChildControls();
				return videoFile.FileName;
			}
		}

		public byte[] FileBytes {
			get {
				EnsureChildControls();
				return videoFile.FileBytes;
			}
		}

		#endregion Mapped Accessors
		
		protected override void CreateChildControls() {

			Controls.Clear();

			//Video Name Panel
			Panel pnlVideoName = new Panel();
			pnlVideoName.ID = "pnlVideoName";
			pnlVideoName.CssClass = "VideoName";
				
			//video name input
			videoName = new TextBox();
			videoName.ID = "VideoName";
			HtmlGenericControl fnLabel = new HtmlGenericControl();
			fnLabel.TagName = "label";
			fnLabel.InnerText = "Video Name *";
			fnLabel.Attributes.Add("for", videoName.ClientID);
			pnlVideoName.Controls.Add(fnLabel);
			pnlVideoName.Controls.Add(videoName);
			
			//add panel to control
			Controls.Add(pnlVideoName);

			//if set to show short description then add it
			if (ShowReferenceID) {

				//Short Description Panel
				Panel pnlRefID = new Panel();
				pnlRefID.ID = "pnlRefID";
				pnlRefID.CssClass = "RefID";

				//Short Description input
				referenceID = new TextBox();
				referenceID.ID = "RefID";
				HtmlGenericControl ridLabel = new HtmlGenericControl();
				ridLabel.TagName = "label";
				ridLabel.InnerText = "Reference ID";
				ridLabel.Attributes.Add("for", referenceID.ClientID);
				pnlRefID.Controls.Add(ridLabel);
				pnlRefID.Controls.Add(referenceID);

				//add panel to control
				Controls.Add(pnlRefID);
			}

			//Short Description Panel
			Panel pnlShortDesc = new Panel();
			pnlShortDesc.ID = "pnlShortDesc";
			pnlShortDesc.CssClass = "ShortDesc";

			//Short Description input
			shortDescription = new TextBox();
			shortDescription.ID = "ShortDesc";
			shortDescription.TextMode = TextBoxMode.MultiLine;
			HtmlGenericControl sdLabel = new HtmlGenericControl();
			sdLabel.TagName = "label";
			sdLabel.InnerText = "Short Description *";
			sdLabel.Attributes.Add("for", shortDescription.ClientID);
			pnlShortDesc.Controls.Add(sdLabel);
			pnlShortDesc.Controls.Add(shortDescription);

			//add panel to control
			Controls.Add(pnlShortDesc);
		
			if (ShowLongDescription) {

				//Long Description Panel
				Panel pnlLonDesc = new Panel();
				pnlLonDesc.ID = "pnlLonDesc";
				pnlLonDesc.CssClass = "LongDesc";

				//Long Description input
				longDescription = new TextBox();
				longDescription.ID = "LongDesc";
				longDescription.TextMode = TextBoxMode.MultiLine;
				HtmlGenericControl ldLabel = new HtmlGenericControl();
				ldLabel.TagName = "label";
				ldLabel.InnerText = "Long Description";
				ldLabel.Attributes.Add("for", longDescription.ClientID);
				pnlLonDesc.Controls.Add(ldLabel);
				pnlLonDesc.Controls.Add(longDescription);

				//add panel to control
				Controls.Add(pnlLonDesc);
			}

			if (ShowTags) {

				//Tags Panel
				Panel pnlTags = new Panel();
				pnlTags.ID = "pnlTags";
				pnlTags.CssClass = "Tags";

				//Tags input
				tags = new TextBox();
				tags.ID = "Tags";
				HtmlGenericControl tLabel = new HtmlGenericControl();
				tLabel.TagName = "label";
				tLabel.InnerText = "Tags (comma separated)";
				tLabel.Attributes.Add("for", tags.ClientID);
				pnlTags.Controls.Add(tLabel);
				pnlTags.Controls.Add(tags);

				//add panel to control
				Controls.Add(pnlTags);
			}

			if (ShowEconomics) {

				//Economics Panel
				Panel pnlEcs = new Panel();
				pnlEcs.ID = "pnlEcs";
				pnlEcs.CssClass = "Economics";

				//Economics input
				economics = new DropDownList();
				economics.ID = "Economics";
				foreach (string s in Enum.GetNames(typeof(BCVideoEconomics))) {
					economics.Items.Add(new ListItem(s, s));
				}
				HtmlGenericControl eLabel = new HtmlGenericControl();
				eLabel.TagName = "label";
				eLabel.InnerText = "Economics";
				eLabel.Attributes.Add("for", economics.ClientID);
				pnlEcs.Controls.Add(eLabel);
				pnlEcs.Controls.Add(economics);

				//add panel to control
				Controls.Add(pnlEcs);
			}

			if (ShowFileUpload) {

				//file uploader panel
				Panel pnlVideoFile = new Panel();
				pnlVideoFile.ID = "pnlVideoFile";
				pnlVideoFile.CssClass = "VideoFile";

				//Video File Uploader
				videoFile = new FileUpload();
				videoFile.ID = "VideoFile";
				HtmlGenericControl fuLabel = new HtmlGenericControl();
				fuLabel.TagName = "label";
				fuLabel.InnerText = "Video File *";
				fuLabel.Attributes.Add("for", videoFile.ClientID);
				pnlVideoFile.Controls.Add(fuLabel);
				pnlVideoFile.Controls.Add(videoFile);

				//add panel to control
				Controls.Add(pnlVideoFile);
			}
		}

		public BCVideo GetBCVideo() {

			BCVideo vid = new BCVideo();
			vid.name = this.VideoName;
			vid.referenceId = this.ReferenceID;
			vid.shortDescription = this.ShortDescription;
			vid.longDescription = this.LongDescription;
			vid.tags = this.Tags;
			vid.economics = this.Economics;

			return vid;
		}

		public void ClearForm() {
			
			//blank out the other fields after upload
			this.VideoName = "";
			this.ReferenceID = "";
			this.ShortDescription = "";
			this.LongDescription = "";
			this.Tags = new BCCollection<string>();
			this.Economics = BCVideoEconomics.FREE;
			this.videoFile = new FileUpload();
		}
	}
}
