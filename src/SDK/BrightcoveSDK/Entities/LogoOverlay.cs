using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BrightcoveSDK.Media
{
    [DataContract]
    public class LogoOverlay
    {
        
        /// <summary>
        /// A number that uniquely identifies the LogoOverlay. This id is automatically assigned by Brightcove when the LogoOverlay is created.
        /// </summary>
        [DataMember]
        public long id { get; set; }

        private BCImage _image;
        /// <summary>
        /// An Image object, defined by its id or referenceId, with type=LOGO_OVERLAY.
        /// </summary>
        [DataMember]
        public BCImage image { 
            get{
                if (_image == null) {
                    _image = new BCImage();
                }
                return _image;
            }
            set {
                _image = value;
            }
        }

        /// <summary>
        /// Optional. A text that is displayed when the viewer mouses over the logo overlay.
        /// </summary>
        [DataMember]
        public string tooltip { get; set; }

        /// <summary>
        /// Optional. A URL to go to if the viewer clicks on the logo overlay.
        /// </summary>
        [DataMember]
        public string linkURL { get; set; }

        [DataMember(Name = "alignment")]
        private string _alignment { get; set; }

        /// <summary>
        /// Optional. A LogoOverlayAlignmentEnum representing the orientation of the logo overlay relative to the video display. One of the following values: TOP_LEFT, BOTTOM_LEFT, TOP_RIGHT, or BOTTOM_RIGHT. The default is BOTTOM_RIGHT.
        /// </summary>
        public Alignment alignment {
            get {
                if (_alignment == null) {
                    return Alignment.BOTTOM_RIGHT;
                } else if (_alignment.Equals(Alignment.BOTTOM_LEFT.ToString())) {
                    return Alignment.BOTTOM_LEFT;
                } else if (_alignment.Equals(Alignment.TOP_LEFT.ToString())) {
                    return Alignment.TOP_LEFT;
                } else if (_alignment.Equals(Alignment.TOP_RIGHT.ToString())) {
                    return Alignment.TOP_RIGHT;
                } else {
                    return Alignment.BOTTOM_RIGHT;
                }
            }
            set {
                _alignment = value.ToString();
            }
        }
    }
}
