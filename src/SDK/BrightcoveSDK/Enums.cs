using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrightcoveSDK
{
	#region Public Enums

    public enum Alignment { TOP_LEFT, BOTTOM_LEFT, TOP_RIGHT, BOTTOM_RIGHT }

	public enum AccountType { Video, Audio }

	public enum JSONType { Create, Update }

	public enum ActionType { READ, WRITE };
	
	public enum BCEncodeType { MP4, FLV, UNDEFINED };

	public enum BCObjectType { videos, playlists };

    public enum BCSortByType { PUBLISH_DATE, CREATION_DATE, MODIFIED_DATE, PLAYS_TOTAL, PLAYS_TRAILING_WEEK };

	public enum BCSortOrderType { ASC, DESC };

	public enum UploadStatusEnum { UPLOADING, PROCESSING, COMPLETE, ERROR, UNDEFINED };
		
	public enum ItemStateEnum { ACTIVE, INACTIVE, DELETED };

	public enum PlaylistTypeEnum { EXPLICIT, OLDEST_TO_NEWEST, NEWEST_TO_OLDEST, ALPHABETICAL, PLAYS_TOTAL, PLAYS_TRAILING_WEEK };

	public enum PlaylistFields {	ID, REFERENCEID, NAME, SHORTDESCRIPTION, VIDEOIDS, VIDEOS, THUMBNAILURL, FILTERTAGS, PLAYLISTTYPE, ACCOUNTID }

	public enum PlayerPlaylistType { None, ComboBox, Tabbed, VideoList }

	public enum VideoFields { 
										ID, NAME, SHORTDESCRIPTION, LONGDESCRIPTION, CREATIONDATE, PUBLISHEDDATE, 
										LASTMODIFIEDDATE, STARTDATE, ENDDATE, LINKURL, LINKTEXT, TAGS, VIDEOSTILLURL, 
										THUMBNAILURL, REFERENCEID, LENGTH, ECONOMICS, ITEMSTATE, PLAYSTOTAL, PLAYSTRAILINGWEEK, VERSION,
										CUEPOINTS, SUBMISSIONINFO, CUSTOMFIELDS, RELEASEDATE, FLVURL, RENDITIONS, GEOFILTERED, 
										GEORESTRICTED, GEOFILTEREXCLUDE, EXCLUDELISTEDCOUNTRIES, GEOFILTEREDCOUNTRIES, 
										ALLOWEDCOUNTRIES, ACCOUNTID, FLVFULLLENGTH, VIDEOFULLLENGTH 
										}

	public enum VideoCodecEnum { UNDEFINED, NONE, SORENSON, ON2, H264 };

	public enum CuePointType { AD = 0, CODE = 1, CHAPTER = 2 };

	public enum BCVideoEconomics { FREE, AD_SUPPORTED };

	public enum ImageTypeEnum { THUMBNAIL, VIDEO_STILL, SYNDICATION_STILL, BACKGROUND, LOGO, LOGO_OVERLAY };

	public enum VideoTypeEnum { FLV_PREVIEW, FLV_FULL, FLV_BUMPER, DIGITAL_MASTER };

	public enum ItemCollection { total_count, items, page_number, page_size };

    public enum MediaDeliveryTypeEnum { HTTP, DEFAULT }

	public enum WMode { Window, Transparent, Opaque }

	#endregion Public Enums
}
