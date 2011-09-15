using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using BrightcoveVideoCloudIntegration;

using BrightcoveSDK;
using BrightcoveSDK.JSON;
using BrightcoveSDK.Containers;
using BrightcoveSDK.Media;
using BrightcoveSDK.Entities.Containers;
using Microsoft.SharePoint;

namespace BrightcoveVideoCloudIntegration.VideoPlaylist
{
    public partial class VideoPlaylistUserControl : VideoCloudWebPartUserControl
    {
        private long _playlistId;

        public string PlaylistId
        {
            get
            {
                return _playlistId.ToString();
            }

            set
            {
                long.TryParse(value, out this._playlistId);
            }
        }

        private string _playlistName;
        private string _playlistType;
        private string _playlistTags;
        private string _videoIdList;
        private bool _changedPlaylist = false;
        private int _itemCount = 0;
        private int _itemsPerPage = 50;

        protected string PlaylistName { get { return this._playlistName; } }
        protected string PlaylistType { get { return this._playlistType; } }
        protected string PlaylistTags { get { return this._playlistTags; } }
        protected string VideoIdListString { get { return this._videoIdList; } }
        protected bool ChangedPlaylist { get { return this._changedPlaylist; } }

        public string[] VideoIdList;
        public string PlayVideoLink;
        public string EditPlaylistLink;

        public override void Deprecated_Page_Load()
        {
            if (this.bcApi == null)
            {
                return;
            }

            this._changedPlaylist = false;
            bool isNewPlaylist = false;

            if (!string.IsNullOrEmpty(Request.Form["playlistId"]))
            {
                PlaylistId = Request.Form["playlistId"];
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["playlistid"]))
            {
                PlaylistId = Request.QueryString["playlistid"];
            }

            if (!string.IsNullOrEmpty(Request["cancel"]))
            {
                Response.Redirect(Request.FilePath);

                return;
            }

            if (this.IsPostBack)
            {
                BCPlaylist playlistObj = null;

                if (string.IsNullOrEmpty(PlaylistId) || (PlaylistId == "0"))
                {
                    if (!string.IsNullOrEmpty(Request.Form["playlistName"]))
                    {
                        playlistObj = AddPlaylist();
                        this._changedPlaylist = true;
                        isNewPlaylist = true;
                    }
                }

                if (!this.IsAdmin)
                {
                    message.InnerHtml = "<p class=\"error\">ERROR: user " + this.CurrentUser.LoginName + " is not an administrator</p>";
                }
            
                if (this.IsAdmin && (this._playlistId > 0) )
                {
                    if (!string.IsNullOrEmpty(Request.Form["deletePlaylist"]))
                    {
                        RPCResponse response = this.bcApi.DeletePlaylist(this._playlistId);

                        if (response.error.message != null)
                        {
                            message.InnerHtml = "<p class=\"error\">ERROR: " + response.error.message + "</p>";
                            this._playlistId = 0;
                        }
                        else
                        {
                            this._changedPlaylist = true;
                        }
                    }
                    else if (!string.IsNullOrEmpty(Request.Form["videos"]))
                    {
                        if (!UpdatePlaylist(isNewPlaylist, playlistObj))
                        {
                            this._playlistId = 0;
                        }

                        this._changedPlaylist = true;
                    }
                    else
                    {
                        this._changedPlaylist = true;
                    }
                }

                if (this._changedPlaylist)
                {
                    Response.Redirect(Request.FilePath + "?close=1");

                    return;
                }
                else
                {
                    this._videoIdList = GetVideosInPlaylist(PlaylistId);
                }
            }
            else
            {
                this._videoIdList = GetVideosInPlaylist(PlaylistId);

                if (this._playlistId > 0)
                {
                    return;
                }
            }

            int pageNumber = Util.ParseInt(Request.QueryString[Util.PagingKey]);
            BCQueryResult result = this.bcApi.FindAllPlaylists(this._itemsPerPage, BCSortByType.MODIFIED_DATE, BCSortOrderType.DESC, null, null, Util.PlaylistBrowseFields, MediaDeliveryTypeEnum.DEFAULT, pageNumber, true);

            if ((result != null) && (result.Playlists.Count > 0))
            {
                this._itemCount = result.TotalCount;

                foreach (BCPlaylist item in result.Playlists)
                {
                    message.InnerHtml += string.Format(
                        @"<div class=""playlistRow"" playlistId=""{0}"">
                            <input type=""checkbox"" value=""{0}"" style=""display:none;"" />
                            <div class=""thumbnail""><a href=""{8}""><img src=""{7}"" border=""0""/></a></div>
                            <div class=""playlist"">
                                <a href=""{8}"">{1}</a><br />
                                <span class=""playlistVideoCount"" val=""{3}"">{3}</span>
                                <span class=""playlistType"" val=""{2}"">{6}</span>
                                <span class=""playlistTags"" val=""{4}"">{4}</span>
                            </div><a class=""playlistEdit"" href=""{5}"">Edit</a>
                        </div>",
                        item.id, item.name, item.playlistType.ToString(), item.videos.Count.ToString(), 
                        item.filterTags.ToDelimitedString(","), string.Format(this.EditPlaylistLink, item.id),
                        GetPlaylistTypeDisplay(item.playlistType.ToString()), item.thumbnailURL, string.Format(this.PlayVideoLink, item.id));
                }

                message.InnerHtml += Util.GetPaging(pageNumber, this._itemsPerPage, this._itemCount, Request.FilePath);
            }
            else
            {
                message.InnerHtml += "No playlists";
            }
        }

        protected string GetPlaylistTypeDisplay(string type)
        {
            string typeDisp = type;

            if (type == "EXPLICIT")
            {
                typeDisp = "Manual";
            }
            else if (type == "NEWEST_TO_OLDEST")
            {
                typeDisp = "Newest to Oldest";
            }
            else if (type == "OLDEST_TO_NEWEST")
            {
                typeDisp = "Oldest to Newest";
            }
            else if (type == "ALPHABETICAL")
            {
                typeDisp = "Alphabetical";
            }
            else if (type == "PLAYS_TOTAL")
            {
                typeDisp = "Plays Total";
            }
            else if (type == "PLAYS_TRAILING_WEEK")
            {
                typeDisp = "Plays Trailing Week";
            }

            return typeDisp;
        }

        protected string GetPlaylistOptions()
        {
            string[] types = Enum.GetNames(typeof(PlaylistTypeEnum));
            string result = string.Empty;
            string manualType = PlaylistTypeEnum.EXPLICIT.ToString();

            foreach (string type in types)
            {
                string typeDisp = GetPlaylistTypeDisplay(type);

                if (!string.IsNullOrEmpty(this._playlistType))
                {
                    if (type.Equals(manualType))
                    {
                        if (this._playlistType != type)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (this._playlistType == manualType)
                        {
                            continue;
                        }
                    }
                }

                if (type == this._playlistType)
                {
                    result += "<option value=\"" + type + "\" selected=\"on\">" + typeDisp + "</option>";
                }
                else
                {
                    result += "<option value=\"" + type + "\">" + typeDisp + "</option>";
                }
            }

            return result;
        }

        public BCPlaylist AddPlaylist()
        {
            BCPlaylist playlist = null;
            RPCResponse<long> response = null;

            playlist = new BCPlaylist();
            playlist.name = Request.Form["playlistName"];
            playlist.playlistType = (PlaylistTypeEnum)Enum.Parse(typeof(PlaylistTypeEnum), Request.Form["playlistType"], true);
            SetTags(playlist);

            for (int i = 0; (i < this.writeTokenList.Length) && (i < 5); i++)
            {
                response = this.bcApi.CreatePlaylist(playlist);

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

            if (response.error.message != null)
            {
                message.InnerHtml = "<p class=\"error\">ERROR: " + response.error.message + "</p>";
            }
            else
            {
                PlaylistId = response.result.ToString();
                message.InnerHtml = "SUCCESS - " + PlaylistId;
            }

            return playlist;
        }

        public void SetTags(BCPlaylist playlist)
        {
            if (playlist.playlistType != PlaylistTypeEnum.EXPLICIT)
            {
                string tags = Request.Form["tags"];

                if (!string.IsNullOrEmpty(tags))
                {
                    string[] list = tags.Split(",".ToCharArray());

                    foreach (string item in list)
                    {
                        playlist.filterTags.Add(item.Trim());
                    }
                }
            }
        }

        public bool UpdatePlaylist(bool isNewPlaylist, BCPlaylist playlist)
        {
            string[] videos = Request.Form["videos"].Split(",".ToCharArray());
            RPCResponse<BCPlaylist> response = null;

            if (playlist == null)
            {
                playlist = new BCPlaylist();
            }

            playlist.id = this._playlistId;

            if (!isNewPlaylist)
            {
                if (!string.IsNullOrEmpty(Request.Form["playlistName"]))
                {
                    playlist.name = Request.Form["playlistName"];
                }

                playlist.playlistType = (PlaylistTypeEnum)Enum.Parse(typeof(PlaylistTypeEnum), Request.Form["playlistType"], true);
                SetTags(playlist);
            }

            // Remove all the videos, then add the ones that are specified
            playlist.videoIds.RemoveRange(0, playlist.videoIds.Count);

            if (playlist.playlistType.Equals(PlaylistTypeEnum.EXPLICIT))
            {
                foreach (string v in videos)
                {
                    if (!string.IsNullOrEmpty(v))
                    {
                        playlist.videoIds.Add(long.Parse(v));
                    }
                }
            }

            for (int i = 0; (i < this.writeTokenList.Length) && (i < 5); i++)
            {
                try
                {
                    response = this.bcApi.UpdatePlaylist(playlist);

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
                catch { }
            }

            if (response == null) 
            {
                message.InnerHtml = "<p class=\"error\">ERROR: Unknown</p>";

                return false;
            }
            else if (response.error.message != null)
            {
                message.InnerHtml = "<p class=\"error\">ERROR: " + response.error.message + "</p>";

                return false;
            }
            else
            {
                message.InnerHtml = "SUCCESS - " + playlist.id.ToString();

                return true;
            }
        }

        public string GetVideosInPlaylist(string playlistId)
        {
            string result = string.Empty;
            BCPlaylist playlist = null;
            string[] selectedVideoList = null;

            try
            {
                if (string.IsNullOrEmpty(playlistId) || (this._playlistId <= 0))
                {
                    selectedVideoList = new string[] { string.Empty };
                }
                else
                {
                    playlist = this.bcApi.FindPlaylistById(long.Parse(playlistId));
                    this._playlistName = playlist.name;
                    this._playlistType = playlist.playlistType.ToString();
                    this._playlistTags = playlist.filterTags.ToDelimitedString(",");
                    selectedVideoList = new string[playlist.videos.Count];

                    for (int i = 0; i < playlist.videoIds.Count; i++)
                    {
                        long video = playlist.videoIds[i];
                        BCVideo videoData = playlist.videos[i];

                        if (!result.Equals(string.Empty))
                        {
                            result += ",";
                        }

                        result += video.ToString();
                        selectedVideoList[i] = string.Format(@"{{ ""id"":{0}, ""name"":'{1}', ""thumbnailURL"":'{2}' }}",
                            videoData.id, videoData.name, videoData.thumbnailURL);
                    }
                }

                this._videoIdList = result;

                string output = string.Format(
                    @"<script language=""javascript"" type=""text/javascript"" charset=""utf-8"">/*<![CDATA[*/
                        var vcPlaylistVideos = [{0}];
                    /*]]>*/</script>", string.Join(",", selectedVideoList));

                message.InnerHtml += output;
            }
            catch
            {
                // Do nothing
            }

            return result;
        }
    }
}
