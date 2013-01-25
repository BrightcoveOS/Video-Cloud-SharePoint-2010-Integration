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
	[ToolboxData("<{0}:PlaylistAddUpdate runat=server></{0}:PlaylistAddUpdate>")]
	public class PlaylistAddUpdate : CompositeControl
	{
		#region Properties

		private TextBox playlistName;
		private TextBox referenceID;
		private TextBox shortDescription;
		private TextBox filterTags;
		private CheckBoxList videoIDs;
		private DropDownList playlistType;
		
		#endregion Properties 

		public string PlaylistName {
			get {
                EnsureChildControls();
                return playlistName.Text;
            }
            set {
                EnsureChildControls();
                playlistName.Text = value;
            }
		}

		[Bindable(true), Category("Appearance"), DefaultValue(true), Localizable(true)]
		public bool ShowReferenceIDForm {
			get {
				return (ViewState["ShowReferenceIDForm"] == null) ? true : (bool)ViewState["ShowReferenceIDForm"];
			}
			set {
				ViewState["ShowReferenceIDForm"] = value;
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

		[Bindable(true), Category("Appearance"), DefaultValue(true), Localizable(true)]
		public bool ShowShortDescriptionForm {
			get {
				return (ViewState["ShowShortDescriptionForm"] == null) ? true : (bool)ViewState["ShowShortDescriptionForm"];
			}
			set {
				ViewState["ShowShortDescriptionForm"] = value;
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
		public bool ShowFilterTagsForm {
			get {
				return (ViewState["ShowFilterTagsForm"] == null) ? true : (bool)ViewState["ShowFilterTagsForm"];
			}
			set {
				ViewState["ShowFilterTagsForm"] = value;
			}
		}

		public BCCollection<string> FilterTags {
			get {
				EnsureChildControls();
				string[] splitr = {","};
				BCCollection<string> bc = new BCCollection<string>();
				foreach (string s in filterTags.Text.Split(splitr, StringSplitOptions.RemoveEmptyEntries)) {
					bc.Add(s);
				}
				return bc;	
			}
			set {
				EnsureChildControls();
				filterTags.Text = value.ToDelimitedString(",");
			}
		}

		//[Bindable(true), Category("Appearance"), DefaultValue(true), Localizable(true)]
		//public bool ShowVideoIDsForm {
		//    get {
		//        return (ViewState["ShowVideoIDsForm"] == null) ? true : (bool)ViewState["ShowVideoIDsForm"];
		//    }
		//    set {
		//        ViewState["ShowVideoIDsForm"] = value;
		//    }
		//}

		//public BCCollection<long> VideoIDs {
		//    get {
		//        EnsureChildControls();
		//        BCCollection<long> bc = new BCCollection<long>();
		//        foreach (ListItem li in videoIDs.Items) {
		//            if (li.Selected.Equals(true)) {
		//                bc.Add(long.Parse(li.Text));
		//            }
		//        }
		//        return bc;
		//    }
		//    set {
		//        EnsureChildControls();
		//        //longDescription.Text = value;
		//    }
		//}

		[Bindable(true), Category("Appearance"), DefaultValue(true), Localizable(true)]
		public bool ShowPlaylistTypeForm {
			get {
				return (ViewState["ShowPlaylistTypeForm"] == null) ? true : (bool)ViewState["ShowPlaylistTypeForm"];
			}
			set {
				ViewState["ShowPlaylistTypeForm"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(PlaylistTypeEnum.EXPLICIT), Localizable(true)]
		public PlaylistTypeEnum PlaylistType {
			get {
				EnsureChildControls();
				if (playlistType.SelectedValue.Equals(PlaylistTypeEnum.ALPHABETICAL.ToString())) {
					return PlaylistTypeEnum.ALPHABETICAL;
				} 
				else if (playlistType.SelectedValue.Equals(PlaylistTypeEnum.EXPLICIT.ToString())) {
					return PlaylistTypeEnum.EXPLICIT;
				}
				else if (playlistType.SelectedValue.Equals(PlaylistTypeEnum.NEWEST_TO_OLDEST.ToString())) {
					return PlaylistTypeEnum.NEWEST_TO_OLDEST;
				}
				else if (playlistType.SelectedValue.Equals(PlaylistTypeEnum.OLDEST_TO_NEWEST.ToString())) {
					return PlaylistTypeEnum.OLDEST_TO_NEWEST;
				}
				else if (playlistType.SelectedValue.Equals(PlaylistTypeEnum.PLAYS_TOTAL.ToString())) {
					return PlaylistTypeEnum.PLAYS_TOTAL;
				}
				else {
					return PlaylistTypeEnum.PLAYS_TRAILING_WEEK;
				}
			}
			set {
				EnsureChildControls();
				foreach (ListItem li in playlistType.Items) {
					if (li.Value.Equals(value.ToString())) {
						li.Selected = true;
					}
					else {
						li.Selected = false;
					}
				}
			}
		}

		protected override void CreateChildControls() {

			Controls.Clear();

			//Playlist Name Panel
			Panel pnlVideoName = new Panel();
			pnlVideoName.ID = "pnlPlaylistName";
			pnlVideoName.CssClass = "PlaylisName";

			//Playlist name input
			playlistName = new TextBox();
			playlistName.ID = "PlaylistName";
			HtmlGenericControl pnLabel = new HtmlGenericControl();
			pnLabel.TagName = "label";
			pnLabel.InnerText = "Playlist Name *";
			pnLabel.Attributes.Add("for", playlistName.ClientID);
			pnlVideoName.Controls.Add(pnLabel);
			pnlVideoName.Controls.Add(playlistName);

			//add panel to control
			Controls.Add(pnlVideoName);

			//if set to show short description then add it
			if (ShowReferenceIDForm) {

				//Reference ID Panel
				Panel pnlRefID = new Panel();
				pnlRefID.ID = "pnlRefID";
				pnlRefID.CssClass = "RefID";

				//Reference ID input
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

			if (ShowShortDescriptionForm) {
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
				sdLabel.InnerText = "Short Description";
				sdLabel.Attributes.Add("for", shortDescription.ClientID);
				pnlShortDesc.Controls.Add(sdLabel);
				pnlShortDesc.Controls.Add(shortDescription);

				//add panel to control
				Controls.Add(pnlShortDesc);
			}

			//if set to show short description then add it
			if (ShowFilterTagsForm) {

				//Filter Tags Panel
				Panel pnlFilterTags = new Panel();
				pnlFilterTags.ID = "pnlFilterTags";
				pnlFilterTags.CssClass = "FilterTags";

				//Filter Tags input
				filterTags = new TextBox();
				filterTags.ID = "FilterTags";
				HtmlGenericControl ftLabel = new HtmlGenericControl();
				ftLabel.TagName = "label";
				ftLabel.InnerText = "Filter Tags (comma separated)";
				ftLabel.Attributes.Add("for", filterTags.ClientID);
				pnlFilterTags.Controls.Add(ftLabel);
				pnlFilterTags.Controls.Add(filterTags);

				//add panel to control
				Controls.Add(pnlFilterTags);
			}

			//if (ShowVideoIDsForm) {

			//    //Video Ids Panel
			//    Panel pnlVideoIDs = new Panel();
			//    pnlVideoIDs.ID = "pnlVideoIDs";
			//    pnlVideoIDs.CssClass = "VideoIDs";

			//    //Video Ids input
			//    videoIDs = new CheckBoxList();
			//    videoIDs.ID = "VideoIDs";
				
			//    //get all the children videos
			//    //foreach (string s in Enum.GetNames(typeof(BCVideoEconomics))) {
			//    //    economics.Items.Add(new ListItem(s, s));
			//    //}
			//    HtmlGenericControl vidLabel = new HtmlGenericControl();
			//    vidLabel.TagName = "label";
			//    vidLabel.InnerText = "Video IDs";
			//    vidLabel.Attributes.Add("for", videoIDs.ClientID);
			//    pnlVideoIDs.Controls.Add(vidLabel);
			//    pnlVideoIDs.Controls.Add(videoIDs);

			//    //add panel to control
			//    Controls.Add(pnlVideoIDs);
			//}

			if (ShowPlaylistTypeForm) {

				//Playlist Type Panel
				Panel pnlPlaylistType = new Panel();
				pnlPlaylistType.ID = "pnlPlaylistType";
				pnlPlaylistType.CssClass = "PlaylistType";

				//Playlist Type input
				playlistType = new DropDownList();
				playlistType.ID = "PlaylistType";
				foreach (string s in Enum.GetNames(typeof(PlaylistTypeEnum))) {
					playlistType.Items.Add(new ListItem(s, s));
				}
				HtmlGenericControl ptLabel = new HtmlGenericControl();
				ptLabel.TagName = "label";
				ptLabel.InnerText = "Playlist Type";
				ptLabel.Attributes.Add("for", playlistType.ClientID);
				pnlPlaylistType.Controls.Add(ptLabel);
				pnlPlaylistType.Controls.Add(playlistType);

				//add panel to control
				Controls.Add(pnlPlaylistType);
			}
		}
	
		public BCPlaylist GetBCPlaylist(){

			BCPlaylist plist = new BCPlaylist();
			plist.name = this.PlaylistName;
			plist.playlistType = this.PlaylistType;
			plist.referenceId = this.ReferenceID;
			plist.shortDescription = this.ShortDescription;
			//plist.videoIds = this.VideoIDs;
			plist.filterTags = this.FilterTags;

			return plist;
		}

		public void ClearForm() {
			
			//blank out the other fields after upload
			this.PlaylistName = "";
			this.PlaylistType = PlaylistTypeEnum.EXPLICIT;
			this.ReferenceID = "";
			this.ShortDescription = "";
			//this.VideoIDs = new BCCollection<long>();
			this.FilterTags = new BCCollection<string>();
		}
	}
}
