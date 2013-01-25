using System;
using System.IO;
using System.Web;
using System.Web.UI;
using BrightcoveSDK;
using BrightcoveSDK.Containers;
using BrightcoveSDK.Entities.Containers;
using BrightcoveSDK.JSON;
using BrightcoveSDK.Media;
using Microsoft.SharePoint;


namespace BrightcoveVideoCloudIntegration.VideoEditor
{
    public partial class VideoEditorUserControl : VideoCloudWebPartUserControl
    {
        protected BCVideo video;
        private string vcErrorCode;

        private void SetVideo(long videoId)
        {
            if ( (videoId > 0) && (this.bcApi != null) )
            {
                this.video = this.bcApi.FindVideoById(videoId, Util.VideoWriteFields);
                this.video.id = videoId;
            }

            if (this.video == null)
            {
                this.video = new BCVideo();
            }

            if (this.IsAdmin && this.IsPostBack)
            {
                this.video = new BCVideo();
                this.video.id = videoId;
                this.video.name = Request.Form["name"];
                this.video.shortDescription = Request.Form["shortDesc"];
                this.video.longDescription = Request.Form["longDesc"];

                if (Request.Form["isActive"] == "Yes")
                {
                    this.video.itemState = ItemStateEnum.ACTIVE;
                }
                else if (Request.Form["isActive"] == "No")
                {
                    this.video.itemState = ItemStateEnum.INACTIVE;
                }

                this.video.linkURL = Request.Form["linkUrl"];
                this.video.linkText = Request.Form["linkText"];
                this.video.videoStillURL = Request.Form["videoStillUrl"];
                this.video.thumbnailURL = Request.Form["thumbnailUrl"];
                this.video.referenceId = Request.Form["referenceId"];
                try { this.video.startDate = BCVideo.DateToUnix(DateTime.Parse(Request.Form["startDate"])); } catch { }
                try { this.video.endDate = BCVideo.DateToUnix(DateTime.Parse(Request.Form["endDate"])); } catch { }

                // Validate start and end dates
                if (!string.IsNullOrEmpty(this.video.startDate) && !string.IsNullOrEmpty(this.video.endDate))
                {
                    if (long.Parse(this.video.startDate) > long.Parse(this.video.endDate))
                    {
                        // Notify user of the validatioin error
                        // TBD
                    }
                }

                // Economics
                if (Request.Form["economics"] == BCVideoEconomics.AD_SUPPORTED.ToString())
                {
                    this.video.economics = BCVideoEconomics.AD_SUPPORTED;
                }
                else if (Request.Form["economics"] == BCVideoEconomics.FREE.ToString())
                {
                    this.video.economics = BCVideoEconomics.FREE;
                }

                // Custom Fields
                if (!string.IsNullOrEmpty(Request.Form["customFields"]))
                {
                    string[] list = Request.Form["customFields"].Split(",".ToCharArray());

                    if (this.video.customFields == null)
                    {
                        this.video.customFields = new CustomFields();
                    }
                    else
                    {
                        this.video.customFields.Values.Clear();
                    }

                    foreach (string item in list)
                    {
                        string[] pair = item.Split(":".ToCharArray());
                        string key = pair[0].Trim();

                        if (!key.Equals(string.Empty))
                        {
                            this.video.customFields.Values.Add(key, pair[1].Trim());
                        }
                    }
                }

                // Tags
                if (this.video.tags == null)
                {
                    this.video.tags = new BCCollection<string>();
                }
                else
                {
                    this.video.tags.Clear();
                }

                if (!string.IsNullOrEmpty(Request.Form["tags"]))
                {
                    string[] list = Request.Form["tags"].Split(",".ToCharArray());

                    foreach (string item in list)
                    {
                        if (!item.Equals(string.Empty))
                        {
                            this.video.tags.Add(item.Trim());
                        }
                    }
                }
            }
        }

        public override void Deprecated_Page_Load()
        {
            long vid = 0;
            bool doFormLoad = true;
            vcErrorCode = "0"; ;

            if (!this.IsAdmin)
            {
                message.InnerHtml = "<p class=\"error\">ERROR: user " + this.CurrentUser.LoginName + " is not an administrator</p>";
                doFormLoad = false;
            }

            long.TryParse(Request["videoid"], out vid);
            this.Page.Form.Enctype = "multipart/form-data";

            // Check for deletion
            if (Request.Form["btnCancel"] == "Cancel")
            {
                doFormLoad = false;
            }
            else if (this.IsAdmin && (vid > 0) && (Request.Form["btnDelete"] == "Delete"))
            {
                RPCResponse response = this.bcApi.DeleteVideo(vid);

                if (string.IsNullOrEmpty(response.result))
                {
                    message.InnerHtml = "<p class=\"success\">Your changes have been saved. Allow up to five minutes for the changes to propagate through the Brightcove Video Cloud.</p>";
                }
                else
                {
                    message.InnerHtml = "<p class=\"error\">ERROR: Your changes could not be saved. " + response.result + "</p>";
                }

                doFormLoad = false;
            }
            else
            {
                this.SetVideo(vid);
            }

            if (doFormLoad && this.IsPostBack)
            {
                int fileIndex = 1;
                HttpPostedFile file = null;

                file = Request.Files["file" + fileIndex.ToString()];

                if ( (file != null) && !string.IsNullOrEmpty(file.FileName))
                {
                    message.InnerHtml = string.Empty;

                    do
                    {
                        // Check for 2GB or smaller file size
                        long maxSize = (long) 2 * 1024 * 1024 * 1024;

                        if (file.InputStream.Length > maxSize)
                        {
                            message.InnerHtml = "<p class=\"error\">ERROR: File size is greater than 2GB</p>";
                        }
                        else
                        {
                            byte[] buffer = new byte[file.InputStream.Length];

                            file.InputStream.Read(buffer, 0, buffer.Length);

                            if (vid <= 0)
                            {
                                RPCResponse<long> response = null;

                                for (int i = 0; (i < this.writeTokenList.Length) && (i < 5); i++)
                                {
                                    response = this.bcApi.CreateVideo(this.video, file.FileName, buffer, BCEncodeType.UNDEFINED, true);

                                    // Detect error: 213, ConcurrentWritesExceededError
                                    if (response.error.code == "213")
                                    {
                                        this.accountConfig.WriteToken.Value = writeTokenList[i];
                                        this.ResetApiConnection();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                if (response.result <= 0)
                                {
                                    vid = 0;
                                    message.InnerHtml += "<p class=\"error\">ERROR: " + response.error.message + "</p>";
                                    vcErrorCode = response.error.code;
                                }
                                else
                                {
                                    vid = Convert.ToInt64(response.result);
                                    this.video.id = vid;
                                    message.InnerHtml += "SUCCESS - " + vid.ToString() + "<br />";

                                    // Update thumbnail image
                                    if (!this.AddImage(Request.Files["file_thumbnailUrl"], ImageTypeEnum.THUMBNAIL, this.video.thumbnailURL))
                                    {
                                        message.InnerHtml += ", thumbnail file upload failed";
                                    }

                                    // Update video still image
                                    if (!this.AddImage(Request.Files["file_videoStillUrl"], ImageTypeEnum.VIDEO_STILL, this.video.videoStillURL))
                                    {
                                        message.InnerHtml += ", video still file upload failed";
                                    }
                                }

                                // Reset the video ID
                                vid = 0;
                            }
                        }

                        fileIndex ++;
                        file = Request.Files["file" + fileIndex.ToString()];
                    } while ( (file != null) && !string.IsNullOrEmpty(file.FileName));
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.video.name) &&
                        !string.IsNullOrEmpty(this.video.shortDescription))
                    {
                        // Only update the video if valid
                        this.bcApi.UpdateVideo(this.video);
                        message.InnerHtml = "SUCCESS - " + vid.ToString();
                    }
                }


                if (vid > 0)
                {
                    // Update thumbnail image
                    if (!this.AddImage(Request.Files["file_thumbnailUrl"], ImageTypeEnum.THUMBNAIL, this.video.thumbnailURL))
                    {
                        message.InnerHtml += ", thumbnail file upload failed";
                    }

                    // Update video still image
                    if (!this.AddImage(Request.Files["file_videoStillUrl"], ImageTypeEnum.VIDEO_STILL, this.video.videoStillURL))
                    {
                        message.InnerHtml += ", video still file upload failed";
                    }
                }
            }

            if (this.video != null)
            {
                if (string.IsNullOrEmpty(this.video.name))
                {
                    this.video.name = " ";
                }

                string output = string.Format(
@"<script language=""javascript"" type=""text/javascript"" charset=""utf-8"" defer=""defer"">/*<![CDATA[*/
    var vcVideoResult = {0};
    var vcIsAdmin = {1};
    var vcErrorCode = {2};

    if (vcVideoResult != null)
    {{
        var video = document.forms[0];

        if (vcVideoResult.name)
        {{
            if (vcVideoResult.name == ' ')
            {{
                vcVideoResult.name = '';
            }}

            video['name'].value = vcVideoResult.name;
        }}

        if (vcVideoResult.shortDescription) video['shortDesc'].value = vcVideoResult.shortDescription;
        if (vcVideoResult.longDescription) video['longDesc'].value = vcVideoResult.longDescription;

        if (vcVideoResult.itemState)
        {{
            if (vcVideoResult.itemState != 'ACTIVE')
            {{
                video['isActive'].selectedIndex = 1;
            }}
        }}

        if (vcVideoResult.linkURL) video['linkUrl'].value = vcVideoResult.linkURL;
        if (vcVideoResult.linkText) video['linkText'].value = vcVideoResult.linkText;
        if (vcVideoResult.videoStillURL) video['videoStillUrl'].value = vcVideoResult.videoStillURL;
        if (vcVideoResult.thumbnailURL) video['thumbnailUrl'].value = vcVideoResult.thumbnailURL;
        if (vcVideoResult.referenceId) video['referenceId'].value = vcVideoResult.referenceId;
        if (vcVideoResult.economics)
        {{
            if (vcVideoResult.economics == 'FREE')
            {{
                video['economics'].selectedIndex = 1;
            }}
            else if (vcVideoResult.economics == 'AD_SUPPORTED')
            {{
                video['economics'].selectedIndex = 2;
            }}
        }}
        if (vcVideoResult.startDate) video['startDate'].value = new Date(new Number(vcVideoResult.startDate)).format('M/dd/yyyy');
        if (vcVideoResult.endDate) video['endDate'].value = new Date(new Number(vcVideoResult.endDate)).format('M/dd/yyyy');

        if (vcVideoResult.customFields)
        {{
            for (var key in vcVideoResult.customFields)
            {{
                video['customFields'].value += key + ':' + vcVideoResult.customFields[key] + ',';
            }}
        }}

        if (vcVideoResult.tags) video['tags'].value = vcVideoResult.tags;
    }}
/*]]>*/</script>", this.video.ToJSON().Replace(": AD_SUPPORTED", ": \"AD_SUPPORTED\"").Replace(": FREE", ": \"FREE\""), this.IsAdmin.ToString().ToLower(), vcErrorCode);

                this.Controls.Add(new LiteralControl(output));
            }
        }

        protected bool AddImage(HttpPostedFile file, ImageTypeEnum imageType, string remoteUrl)
        {
            if ((file != null) && !string.IsNullOrEmpty(file.FileName))
            {
                byte[] buffer = new byte[file.InputStream.Length];
                BCImage image = new BCImage();
                RPCResponse<BCImage> result = null;

                file.InputStream.Read(buffer, 0, buffer.Length);
                image.type = imageType;
                image.remoteUrl = remoteUrl;
                image.id = this.video.id;
                image.displayName = Path.GetFileName(file.FileName);;
                result = this.bcApi.AddImage(image, image.displayName, buffer, this.video.id, true);

                if ((result.error.code != null) || (result.error.message != null))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

