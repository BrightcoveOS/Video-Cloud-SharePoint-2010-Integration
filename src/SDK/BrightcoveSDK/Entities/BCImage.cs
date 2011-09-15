using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web;

namespace BrightcoveSDK.Media
{	
	/// <summary>
	/// This object represents metadata about an image file in your account. 
	/// Images are associated with videos as thumbnail images or video still images. 
	/// An image can be a JPEG, GIF, or PNG-formatted image.
	/// </summary>
	[DataContract]
	public class BCImage : BCObject, IComparable<BCImage>
	{	
		/// <summary>
		/// A number that uniquely identifies this Image, assigned by Brightcove when the Image is created.
		/// </summary>
		[DataMember]
		public long id { get; set; }

		/// <summary>
		/// A user-specified id that uniquely identifies this Image. ReferenceID can be used as a foreign-key to identify this Image in another system. 
		/// </summary> 
		[DataMember]
		public string referenceId { get; set; }

		[DataMember(Name="type")]
		private string imageType { get; set; }
		
		/// <summary>
		/// Either THUMBNAIL or VIDEO_STILL. The type is writable and required when you create an Image; it cannot subsequently be changed.
		/// </summary> 
		public ImageTypeEnum type {
			get {
				if (imageType.Equals(ImageTypeEnum.THUMBNAIL.ToString())) {
					return ImageTypeEnum.THUMBNAIL;
				}
				else if (imageType.Equals(ImageTypeEnum.VIDEO_STILL.ToString())) {
					return ImageTypeEnum.VIDEO_STILL;
				}
				else if (imageType.Equals(ImageTypeEnum.BACKGROUND.ToString())) {
					return ImageTypeEnum.BACKGROUND;
				}
				else if (imageType.Equals(ImageTypeEnum.LOGO.ToString())) {
					return ImageTypeEnum.LOGO;
				}
				else if (imageType.Equals(ImageTypeEnum.LOGO_OVERLAY.ToString())) {
					return ImageTypeEnum.LOGO_OVERLAY;
				}
				else if (imageType.Equals(ImageTypeEnum.SYNDICATION_STILL.ToString())) {
					return ImageTypeEnum.SYNDICATION_STILL;
				}
				else {
					return ImageTypeEnum.VIDEO_STILL;
				}
			}
			set {
				imageType = value.ToString();
			}
		}

		/// <summary>
		/// The URL of a remote image file. This property is required if you are not uploading a file for the image asset.
		/// </summary> 
		[DataMember]
		public string remoteUrl { get; set; }
		
		/// <summary>
		/// The name of the asset, which will be displayed in the Media module.
		/// </summary> 
		[DataMember]
		public string displayName { get; set; }

		#region IComparable Comparators

		public int CompareTo(BCImage other) {
			return id.CompareTo(other.id);
		}

		#endregion

	}

	public static class BCImageExtensions {

		#region Extension Methods

		public static string ToJSON(this BCImage image) {
			
			//Build Image in JSON 
			string jsonImage = "{\"displayName\": \"" + image.displayName + "\"";
			if(!string.IsNullOrEmpty(image.referenceId)){
				jsonImage += ", \"referenceId\": \"" + image.referenceId + "\"";
			}
			jsonImage += ", \"type\": " + image.type.ToString() + "}";
			
			return jsonImage;
		}

		#endregion
	}
}
