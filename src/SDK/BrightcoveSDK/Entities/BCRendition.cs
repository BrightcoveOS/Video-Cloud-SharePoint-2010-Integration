using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BrightcoveSDK.Media
{
	
	/// <summary>
	/// The Rendition object represents one of the dynamic delivery 
	/// renditions of a video. A Video should have not more than 10 Renditions.
	/// </summary>
	[DataContract]
	public class BCRendition : BCObject, IComparable<BCRendition>
	{
		/// <summary>
		/// The URL of the rendition file.
		/// </summary> 
		[DataMember]
		public string url { get; set; }

		/// <summary>
		/// The rendition's encoding rate, in bits per second.
		/// </summary> 
		[DataMember]
		public int encodingRate { get; set; }
		
		/// <summary>
		/// The rendition's display height, in pixels.
		/// </summary> 
		[DataMember]
		public int frameHeight { get; set; }

		/// <summary>
		/// The rendition's display width, in pixels.
		/// </summary> 
		[DataMember]
		public int frameWidth { get; set; }

		/// <summary>
		/// The file size of the rendition, in bytes.
		/// </summary> 
		[DataMember]
		public long size { get; set; }
		
		/// <summary>
		/// Required, for remote assets. The complete path to the file hosted on 
		/// the remote server. If the file is served using progressive download, 
		/// then you must include the file name and extension for the file. You can also 
		/// use a URL that re-directs to a URL that includes the file name and extension. 
		/// If the file is served using Flash streaming, use the remoteStreamName 
		/// attribute to provide the stream name.
		/// </summary>
		[DataMember]
		public long remoteUrl { get; set; }

		/// <summary>
		/// required for streaming remote assets only. A stream name for Flash 
		/// streaming appended to the value of the remoteUrl property.
		/// </summary> 
		[DataMember]
		public string remoteStreamName { get; set; }

		/// <summary>
		/// Required. The length of the remote video asset in milliseconds.
		/// </summary> 
		[DataMember]
		public long videoDuration { get; set; }

		[DataMember(Name = "videoCodec")]
		private string codecType { get; set; }

		/// <summary>
		/// Required. Valid values are SORENSON, ON2, and H264.
		/// </summary> 
		public VideoCodecEnum videoCodec {
			get {
				if (codecType.Equals(VideoCodecEnum.H264.ToString())) {
					return VideoCodecEnum.H264;
				}
				else if (codecType.Equals(VideoCodecEnum.NONE.ToString())) {
					return VideoCodecEnum.NONE;
				}
				else if (codecType.Equals(VideoCodecEnum.ON2.ToString())) {
					return VideoCodecEnum.ON2;
				}
				else if (codecType.Equals(VideoCodecEnum.SORENSON.ToString())) {
					return VideoCodecEnum.SORENSON;
				}
				else {
					return VideoCodecEnum.UNDEFINED;
				}
			}
			set {
				codecType = value.ToString();
			}
		}
						
		#region IComparable Comparators

		public int CompareTo(BCRendition other) {
			return url.CompareTo(other.url);
		}

		#endregion
	}
}
