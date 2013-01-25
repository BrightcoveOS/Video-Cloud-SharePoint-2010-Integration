using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BrightcoveSDK;
using BrightcoveSDK.JSON;
using BrightcoveSDK.Containers;
using BrightcoveSDK.Media;
using BrightcoveSDK.Entities.Containers;
using Microsoft.SharePoint;

namespace BrightcoveVideoCloudIntegration
{
    class Util
    {
        private static List<VideoFields> videoWriteFields = null;
        private static List<VideoFields> videoSearchFields = null;
        private static List<string> playlistBrowseFields = null;
        
        public const string PagingKey = "p";
        public const string Brightcove_AdministratorsGroup = "Brightcove Administrators";

        public static List<BrightcoveSDK.VideoFields> VideoWriteFields
        {
            get
            {
                if (videoWriteFields == null)
                {
                    videoWriteFields = new List<VideoFields>();
                    videoWriteFields.Add(VideoFields.CUSTOMFIELDS);
                    videoWriteFields.Add(VideoFields.ECONOMICS);
                    videoWriteFields.Add(VideoFields.ENDDATE);
                    videoWriteFields.Add(VideoFields.ID);
                    videoWriteFields.Add(VideoFields.ITEMSTATE);
                    videoWriteFields.Add(VideoFields.LINKTEXT);
                    videoWriteFields.Add(VideoFields.LINKURL);
                    videoWriteFields.Add(VideoFields.LONGDESCRIPTION);
                    videoWriteFields.Add(VideoFields.NAME);
                    videoWriteFields.Add(VideoFields.REFERENCEID);
                    videoWriteFields.Add(VideoFields.SHORTDESCRIPTION);
                    videoWriteFields.Add(VideoFields.STARTDATE);
                    videoWriteFields.Add(VideoFields.TAGS);
                    videoWriteFields.Add(VideoFields.THUMBNAILURL);
                    videoWriteFields.Add(VideoFields.VIDEOSTILLURL);
                }

                return videoWriteFields;
            }
        }

        public static List<string> PlaylistBrowseFields
        {
            get
            {
                if (playlistBrowseFields == null)
                {
                    PlaylistFields[] fields = (PlaylistFields[]) Enum.GetValues(typeof(PlaylistFields));

                    playlistBrowseFields = new List<string>(fields.Length);

                    for (int i = 0; i < fields.Length; i ++)
                    {
                        playlistBrowseFields.Add(fields[i].ToString());
                    }
                }

                return playlistBrowseFields;
            }
        }

        public static List<BrightcoveSDK.VideoFields> VideoSearchFields
        {
            get
            {
                if (videoSearchFields == null)
                {
                    videoSearchFields = new List<VideoFields>();
                    //videoSearchFields.Add(VideoFields.LINKTEXT);
                    //videoSearchFields.Add(VideoFields.LINKURL);
                    videoSearchFields.Add(VideoFields.LONGDESCRIPTION);
                    videoSearchFields.Add(VideoFields.NAME);
                    videoSearchFields.Add(VideoFields.SHORTDESCRIPTION);
                    //videoSearchFields.Add(VideoFields.TAGS);
                    //videoSearchFields.Add(VideoFields.THUMBNAILURL);
                    //videoSearchFields.Add(VideoFields.VIDEOSTILLURL);
                }

                return videoSearchFields;
            }
        }

        public static Dictionary<BrightcoveSDK.VideoFields, string> GetVideoSearchFields(string query)
        {
            Dictionary<BrightcoveSDK.VideoFields, string> hash = new Dictionary<VideoFields, string>();

            foreach (VideoFields field in Util.VideoSearchFields)
            {
                hash.Add(field, query);
            }

            return hash;
        }

        public static string NonEmpty(object text)
        {
            return NonEmpty((string)text);
        }

        public static string NonEmpty(string text)
        {
            if (text == null)
            {
                return string.Empty;
            }

            return text;
        }

        public static string FixParam(string text)
        {
            string result = NonEmpty(text);

            return result.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "\\r");
        }

        public static string[] GetAllVideos(BCAPI api, int pageNumber, string query)
        {
            string[] result = null;

            if (api != null)
            {
                List<BrightcoveSDK.VideoFields> videoSearchFields = new List<VideoFields>();
                int itemsPerPage = 50;
                int itemCount = 0;
                BCQueryResult videos = null;

                videoSearchFields.Add(VideoFields.NAME);

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.Trim();
                }
                else
                {
                    query = string.Empty;
                }

                if (query.Length == 0)
                {
                    videos = api.FindAllVideos(itemsPerPage, BCSortByType.MODIFIED_DATE, BCSortOrderType.DESC, null, null, MediaDeliveryTypeEnum.DEFAULT, pageNumber, true);
                }
                else
                {
                    videos = api.FindVideosByText(query, itemsPerPage, BCSortByType.MODIFIED_DATE, BCSortOrderType.DESC, null, null, MediaDeliveryTypeEnum.DEFAULT, pageNumber, true);
                }

                if (videos != null)
                {
                    itemCount = videos.TotalCount;
                }

                result = new string[videos.TotalCount];

                for (int i = 0; i < videos.Videos.Count; i++)
                {
                    BCVideo video = videos.Videos[i];

                    if (i < itemsPerPage)
                    {
                        result[i] = string.Format(@"{{ ""id"":{0}, ""name"":'{1}', ""thumbnailURL"":'{2}' }}",
                            video.id, Util.FixParam(video.name), Util.FixParam(video.thumbnailURL));
                    }
                    else
                    {
                        result[i] = "null";
                    }
                }
            }

            return result;
        }

        public static string[] GetAllPlaylists(BCAPI api, int pageNumber, string query)
        {
            string[] result = null;

            if (api != null)
            {
                int itemsPerPage = 50;
                int itemCount = 0;
                BCQueryResult playlists = null;

                // Cannot search playlists yet, but in the future use "query" -ACA 9/12/2011
                if (!string.IsNullOrEmpty(query))
                {
                    query = query.Trim().ToLower();
                }
                else
                {
                    query = string.Empty;
                }

                playlists = api.FindAllPlaylists(itemsPerPage, BCSortByType.MODIFIED_DATE, BCSortOrderType.DESC, null, null, Util.PlaylistBrowseFields, MediaDeliveryTypeEnum.DEFAULT, pageNumber, true);

                if (playlists != null)
                {
                    itemCount = playlists.TotalCount;
                }

                result = new string[playlists.TotalCount];

                for (int i = 0; i < playlists.Playlists.Count; i++)
                {
                    if (i < itemsPerPage)
                    {
                        BCPlaylist playlist = playlists.Playlists[i];

                        result[i] = string.Format(@"{{ ""id"":{0}, ""name"":'{1}', ""thumbnailURL"":'{2}' }}",
                            playlist.id, Util.FixParam(playlist.name), Util.FixParam(playlist.thumbnailURL));
                    }
                    else
                    {
                        result[i] = "null";
                    }
                }
            }

            return result.ToArray();
        }

        public static int ParseInt(string text)
        {
            int result = 0;

            if (text != null)
            {
                int.TryParse(text, out result);
            }

            return result;
        }

        public static long ParseLong(string text)
        {
            long result = 0;

            long.TryParse(text, out result);

            return result;
        }

        public static string GetPaging(int pageNumber, int itemsPerPage, int itemCount, string link)
        {
            int lowIndex = Math.Max((pageNumber * itemsPerPage) + 1, 1);
            int highIndex = Math.Min(((pageNumber + 1) * itemsPerPage), itemCount);
            string result = string.Empty;

            if (!link.Contains("?"))
            {
                link += "?paging=on";
            }

            if (lowIndex > 1) 
            {
                result += string.Format("<a class=\"linkPrev\" href=\"{0}&{2}={1}\">prev</a>", link, (pageNumber - 1), PagingKey);
            }

            if ( (lowIndex > 1) || (highIndex < itemCount) )
            {
                if (lowIndex == highIndex)
                {
                    result += string.Format("<span class=\"itemRange\"><nobr>showing {0}</nobr></span>", lowIndex, highIndex);
                }
                else
                {
                    result += string.Format("<span class=\"itemRange\"><nobr>showing {0} - {1}</nobr></span>", lowIndex, highIndex);
                }
            }

            if (highIndex < itemCount)
            {
                result += string.Format("<a class=\"linkNext\" href=\"{0}&{2}={1}\">next</a>", link, (pageNumber + 1), PagingKey);
            }
                
            if (!result.Equals(string.Empty))
            {
                result = "<div class=\"paging\">" + result + "</div>";
            }

            return result;
        }

        public static bool IsUserAnAdmin(SPWeb web)
        {
            SPUser user = web.CurrentUser;
            SPGroup group = null;

            try { group = web.SiteGroups[Util.Brightcove_AdministratorsGroup]; } catch {}

            if (group == null)
            {
                try
                {
                    SPUser ownerUser = user;
                    SPUser defaultUser = null;
                    web.EnsureUser(user.LoginName);
                    web.AllowUnsafeUpdates = true;
                    web.SiteGroups.Add(Util.Brightcove_AdministratorsGroup, web.SiteAdministrators[0], defaultUser, "Brightcove Video Cloud account administrators group");
                    group = web.SiteGroups[Util.Brightcove_AdministratorsGroup];
                    group.AddUser(web.SiteAdministrators[0]);
                    web.Update();
                    web.AllowUnsafeUpdates = false;

                    return true;
                }
                catch (SPException e)
                {
                    // Do nothing
                }
            }
            else
            {
                try
                {
                    if (user.Groups[Util.Brightcove_AdministratorsGroup].Name.Equals(group.Name))
                    {
                        return true;
                    }
                } catch {}
            }

            return false;
        }
    }
}
