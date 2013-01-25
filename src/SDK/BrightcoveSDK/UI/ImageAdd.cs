using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BrightcoveSDK.Media;
using System.Web;

namespace BrightcoveSDK.UI
{	
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:ImageAdd runat=server></{0}:ImageAdd>")]
	public class ImageAdd : CompositeControl
	{
		#region Properties

		private TextBox imageName;
		private TextBox referenceID;
		private DropDownList type;
		private CheckBox resize;
		private FileUpload imageFile;

		#endregion Properties

		[Bindable(true), Category("Appearance"), DefaultValue(true), Localizable(true)]
		public bool ShowImageName {
			get {
				return (ViewState["ShowImageName"] == null) ? true : (bool)ViewState["ShowImageName"];
			}
			set {
				ViewState["ShowImageName"] = value;
			}
		}

		public string ImageName {
			get {
				EnsureChildControls();
				return imageName.Text;
			}
			set {
				EnsureChildControls();
				imageName.Text = value;
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

		public ImageTypeEnum Type {
			get {
				EnsureChildControls();
				if (type.Text.Equals(ImageTypeEnum.BACKGROUND.ToString())) {
					return ImageTypeEnum.BACKGROUND;
				}
				else if (type.Text.Equals(ImageTypeEnum.LOGO.ToString())) {
					return ImageTypeEnum.LOGO;
				}
				else if (type.Text.Equals(ImageTypeEnum.LOGO_OVERLAY.ToString())) {
					return ImageTypeEnum.LOGO_OVERLAY;
				}
				else if (type.Text.Equals(ImageTypeEnum.SYNDICATION_STILL.ToString())) {
					return ImageTypeEnum.SYNDICATION_STILL;
				}
				else if (type.Text.Equals(ImageTypeEnum.THUMBNAIL.ToString())) {
					return ImageTypeEnum.THUMBNAIL;
				}
				else {
					return ImageTypeEnum.VIDEO_STILL;
				}
			}
			set {
				EnsureChildControls();
				type.Text = value.ToString();
			}
		}

		public bool Resize {
			get {
				EnsureChildControls();
				return resize.Checked;
			}
			set {
				resize.Checked = value;
			}
		}

		#region FileUpload Accessors
		
		public string FileName {
			get {
				EnsureChildControls();
				return imageFile.FileName;
			}
		}

		public byte[] FileBytes {
			get {
				EnsureChildControls();
				return imageFile.FileBytes;
			}
		}

		#endregion Mapped Accessors

		protected override void CreateChildControls() {

			Controls.Clear();

			if (ShowImageName) {
				//Image Name Panel
				Panel pnlImageName = new Panel();
				pnlImageName.ID = "pnlImageName";
				pnlImageName.CssClass = "ImageName";

				//Image Name input
				imageName = new TextBox();
				imageName.ID = "ImageName";
				HtmlGenericControl nameLabel = new HtmlGenericControl();
				nameLabel.TagName = "label";
				nameLabel.InnerText = "Image Name";
				nameLabel.Attributes.Add("for", imageName.ClientID);
				pnlImageName.Controls.Add(nameLabel);
				pnlImageName.Controls.Add(imageName);

				//add panel to control
				Controls.Add(pnlImageName);
			}

			//if set to show reference ID then add it
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

			//Type Panel
			Panel pnlType = new Panel();
			pnlType.ID = "pnlType";
			pnlType.CssClass = "Type";

			//Type input
			type = new DropDownList();
			type.ID = "Type";
			foreach (string s in Enum.GetNames(typeof(ImageTypeEnum))) {
				type.Items.Add(new ListItem(s, s));
			}
			HtmlGenericControl tLabel = new HtmlGenericControl();
			tLabel.TagName = "label";
			tLabel.InnerText = "Image Type";
			tLabel.Attributes.Add("for", type.ClientID);
			pnlType.Controls.Add(tLabel);
			pnlType.Controls.Add(type);

			//add panel to control
			Controls.Add(pnlType);

			//Resize Panel
			Panel pnlResize = new Panel();
			pnlResize.ID = "pnlResize";
			pnlResize.CssClass = "Resize";

			//Resize input
			resize = new CheckBox();
			resize.ID = "Resize";
			HtmlGenericControl reLabel = new HtmlGenericControl();
			reLabel.TagName = "label";
			reLabel.InnerText = "Resize Image";
			reLabel.Attributes.Add("for", resize.ClientID);
			pnlResize.Controls.Add(reLabel);
			pnlResize.Controls.Add(resize);

			//add panel to control
			Controls.Add(pnlResize);
			
			//file uploader panel
			Panel pnlImageFile = new Panel();
			pnlImageFile.ID = "pnlImageFile";
			pnlImageFile.CssClass = "ImageFile";

			//Video File Uploader
			imageFile = new FileUpload();
			imageFile.ID = "ImageFile";
			HtmlGenericControl imgLabel = new HtmlGenericControl();
			imgLabel.TagName = "label";
			imgLabel.InnerText = "Image File *";
			imgLabel.Attributes.Add("for", imageFile.ClientID);
			pnlImageFile.Controls.Add(imgLabel);
			pnlImageFile.Controls.Add(imageFile);

			//add panel to control
			Controls.Add(pnlImageFile);
		}

		public BCImage GetBCImage() {

			BCImage img = new BCImage();
			img.displayName = this.ImageName;
			img.referenceId = this.ReferenceID;
			img.type = this.Type;

			return img;
		}

		public void ClearForm() {

			//blank out the other fields after upload
			this.ImageName = "";
			this.ReferenceID = "";
			this.Resize = false;
			this.Type = ImageTypeEnum.THUMBNAIL;
			this.imageFile = new FileUpload();
		}
	}
}
