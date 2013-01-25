using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BrightcoveSDK.Extensions;
using BrightcoveSDK.Entities.Containers;
using BrightcoveSDK.HTTP;
using BrightcoveSDK.Media;

namespace BrightcoveSDK
{
	public partial class BCAPI
	{
		#region Video Read

		#region Search Videos

		/*
        public BCQueryResult SearchVideos(int howMany, Dictionary<VideoFields, string> required_matches) {
            return SearchVideos(howMany, required_matches, null);
        }

        public BCQueryResult SearchVideos(int howMany, Dictionary<VideoFields, string> required_matches, Dictionary<VideoFields, string> at_least_one_match) {
            return SearchVideos(howMany, required_matches, at_least_one_match, null);
        }

        public BCQueryResult SearchVideos(int howMany, Dictionary<VideoFields, string> required_matches, Dictionary<VideoFields, string> at_least_one_match, Dictionary<VideoFields, string> must_not_match) {
            return SearchVideos(howMany, required_matches, at_least_one_match, must_not_match, BCSortOrderType.ASC);
        }

        public BCQueryResult SearchVideos(int howMany, Dictionary<VideoFields, string> required_matches, Dictionary<VideoFields, string> at_least_one_match, Dictionary<VideoFields, string> must_not_match, BCSortOrderType sortOrder) {
            return SearchVideos(howMany, required_matches, at_least_one_match, must_not_match, sortOrder, true);
        }

        public BCQueryResult SearchVideos(int howMany, Dictionary<VideoFields, string> required_matches, Dictionary<VideoFields, string> at_least_one_match, Dictionary<VideoFields, string> must_not_match, BCSortOrderType sortOrder, bool exact) {
            return SearchVideos(howMany, required_matches, at_least_one_match, must_not_match, sortOrder, exact, null);
        }

        public BCQueryResult SearchVideos(int howMany, Dictionary<VideoFields, string> required_matches, Dictionary<VideoFields, string> at_least_one_match, Dictionary<VideoFields, string> must_not_match, BCSortOrderType sortOrder, bool exact, List<VideoFields> video_fields) {
            return SearchVideos(howMany, required_matches, at_least_one_match, must_not_match, sortOrder, exact, video_fields, null);
        }

        public BCQueryResult SearchVideos(int howMany, Dictionary<VideoFields, string> required_matches, Dictionary<VideoFields, string> at_least_one_match, Dictionary<VideoFields, string> must_not_match, BCSortOrderType sortOrder, bool exact, List<VideoFields> video_fields, List<string> custom_fields) {
            return SearchVideos(howMany, required_matches, at_least_one_match, must_not_match, sortOrder, exact, video_fields, custom_fields, MediaDeliveryTypeEnum.DEFAULT);
        }

        public BCQueryResult SearchVideos(int howMany, BCSortOrderType sortOrder) {
            return SearchVideos(howMany, sortOrder, true);
        }

        public BCQueryResult SearchVideos(int howMany, BCSortOrderType sortOrder, bool exact) {
            return SearchVideos(howMany, null, null, null, sortOrder, exact);
        }

        public BCQueryResult SearchVideos(int howMany, BCSortOrderType sortOrder, List<VideoFields> video_fields) {
            return SearchVideos(howMany, sortOrder, true, video_fields, null);
        }

        public BCQueryResult SearchVideos(int howMany, bool exact, List<VideoFields> video_fields) {
            return SearchVideos(howMany, BCSortOrderType.ASC, exact, video_fields, null);
        }

        public BCQueryResult SearchVideos(int howMany, bool exact, List<VideoFields> video_fields) {
            return SearchVideos(howMany, BCSortOrderType.ASC, exact, video_fields, null);
        }

        public BCQueryResult SearchVideos(int howMany, BCSortOrderType sortOrder, bool exact, List<VideoFields> video_fields) {
            return SearchVideos(howMany, sortOrder, exact, video_fields, null);
        }

        public BCQueryResult SearchVideos(int howMany, BCSortOrderType sortOrder, bool exact, List<VideoFields> video_fields, List<string> custom_fields) {
            return SearchVideos(howMany, sortOrder, exact, video_fields, custom_fields, MediaDeliveryTypeEnum.DEFAULT);
        }

        public BCQueryResult SearchVideos(int howMany, BCSortOrderType sortOrder, bool exact, List<VideoFields> video_fields, List<string> custom_fields, MediaDeliveryTypeEnum media_delivery) {
            return SearchVideos(howMany, null, null, null, sortOrder, exact, video_fields, custom_fields, MediaDeliveryTypeEnum.DEFAULT);
        }
        */

		/// <summary>
		/// 
		/// </summary>
		/// <param name="howMany">
		/// Number of items returned per page. A page is a subset of all of the items that satisfy the request. The maximum page size is 100; if you do not set this argument, or if you set it to an integer > 100, your results will come back as if you had set page_size=100.
		/// </param>
		/// <param name="required_matches">
		/// Specifies the field:value pairs for search criteria that MUST be present in the index in order to return a hit in the result set. The format is fieldName:value. If the field's name is not present, it is assumed to be name and shortDescription.
		/// </param>
		/// <param name="at_least_one_match">
		/// Specifies the field:value pairs for search criteria AT LEAST ONE of which must be present to return a hit in the result set. The format is fieldName:value. If the field's name is not present, it is assumed to be name and shortDescription.
		/// </param>
		/// <param name="must_not_match">
		/// Specifies the field:value pairs for search criteria that MUST NOT be present to return a hit in the result set. The format is fieldName:value. If the field's name is not present, it is assumed to be name and shortDescription.
		/// </param>
		/// <param name="sortOrder">
		/// Specifies the field to sort by, and the direction to sort in. This is specified as: sortFieldName:direction If the direction is not provided, it is assumed to be in ascending order Specify the direction as "asc" for ascending or "desc" for descending.
		/// </param>
		/// <param name="exact">
		/// If true, disables fuzzy search and requires an exact match of search terms. A fuzzy search does not require an exact match of the indexed terms, but will return a hit for terms that are closely related based on language-specific criteria. The fuzzy search is available only if your account is based in the United States.
		/// </param>
		/// <param name="video_fields">
		/// A comma-separated list of the fields you wish to have populated in the Videos  contained in the returned object. If you omit this parameter, the method returns the following fields of the video: id, name, shortDescription, longDescription, creationDate, publisheddate, lastModifiedDate, linkURL, linkText, tags, videoStillURL, thumbnailURL, referenceId, length, economics, playsTotal, playsTrailingWeek. If you use a token with URL access, this method also returns FLVURL, renditions, FLVFullLength, videoFullLength.
		/// </param>
		/// <param name="custom_fields">
		/// A comma-separated list of the custom fields  you wish to have populated in the videos contained in the returned object. If you omit this parameter, no custom fields are returned, unless you include the value 'customFields' in the video_fields parameter.
		/// </param>
		/// <param name="media_delivery">
		/// If universal delivery service  is enabled for your account, set this optional parameter to http to return video by HTTP, rather than streaming. Meaningful only if used together with the video_fields=FLVURL, videoFullLength, or renditions parameters. This is a MediaDeliveryTypeEnum with a value of http or default.
		/// </param>
		/// <returns></returns>
		public BCQueryResult SearchVideos(int howMany, Dictionary<VideoFields, string> required_matches, Dictionary<VideoFields, string> at_least_one_match, Dictionary<VideoFields, string> must_not_match, BCSortOrderType sortOrder, bool exact, List<VideoFields> video_fields, List<string> custom_fields, MediaDeliveryTypeEnum media_delivery) {

			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "search_videos");
			if (required_matches != null) reqparams.Add("all", required_matches.DicToString());
			if (at_least_one_match != null) reqparams.Add("any", at_least_one_match.DicToString());
			if (must_not_match != null) reqparams.Add("none", must_not_match.DicToString());
			reqparams.Add("exact", exact.ToString());
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));
			reqparams.Add("sort_order", sortOrder.ToString());
			if (howMany >= 0) reqparams.Add("page_size", howMany.ToString());

			return MultipleQueryHandler(reqparams, BCObjectType.videos, Account);
		}

		#endregion Search Videos

		#region Find All Videos

		public BCQueryResult FindAllVideos() {
			return FindAllVideos(-1);
		}

		public BCQueryResult FindAllVideos(int howMany) {
			return FindAllVideos(howMany, BCSortByType.CREATION_DATE, BCSortOrderType.ASC, null);
		}

		public BCQueryResult FindAllVideos(BCSortOrderType sortOrder) {
			return FindAllVideos(-1, BCSortByType.CREATION_DATE, sortOrder);
		}

		public BCQueryResult FindAllVideos(BCSortByType sortBy) {
			return FindAllVideos(-1, sortBy, BCSortOrderType.ASC);
		}

		public BCQueryResult FindAllVideos(BCSortByType sortBy, BCSortOrderType sortOrder) {
			return FindAllVideos(-1, sortBy, sortOrder);
		}

		public BCQueryResult FindAllVideos(int howMany, BCSortByType sortBy, BCSortOrderType sortOrder) {
			return FindAllVideos(howMany, sortBy, sortOrder, null);
		}

		public BCQueryResult FindAllVideos(int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields) {
			return FindAllVideos(howMany, sortBy, sortOrder, video_fields, null);
		}

		public BCQueryResult FindAllVideos(int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<string> custom_fields) {
			return FindAllVideos(howMany, sortBy, sortOrder, video_fields, custom_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// This will return a generic search for videos
		/// </summary>
		/// <param name="howMany">
		/// The number of videos to return (-1 will return all) defaults to -1
		/// </param>
		/// <param name="sortBy">
		/// The field by which to sort (defaults to CREATION_DATE)
		/// </param>
		/// <param name="sortOrder">
		/// The direction by which to sort (default to DESC)
		/// </param>
		/// <param name="video_fields">
		/// A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="custom_fields">
		/// A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <returns>
		/// Returns a BCQueryResult item
		/// </returns>
        public BCQueryResult FindAllVideos(int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<string> custom_fields, MediaDeliveryTypeEnum media_delivery)
        {
            return FindAllVideos(howMany, sortBy, sortOrder, video_fields, custom_fields, media_delivery, 0, false);
        }
        
        public BCQueryResult FindAllVideos(int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<string> custom_fields, MediaDeliveryTypeEnum media_delivery, int pageNumber, bool getItemCount)
        {

			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_all_videos");
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));
			reqparams.Add("sort_order", sortOrder.ToString());
			reqparams.Add("sort_by", sortBy.ToString());
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (howMany >= 0) reqparams.Add("page_size", howMany.ToString());
            if (pageNumber > 0) reqparams.Add("page_number", pageNumber.ToString());
            if (getItemCount) reqparams.Add("get_item_count", "true");

			return MultipleQueryHandler(reqparams, BCObjectType.videos, Account);
		}

		#endregion Find All Videos

		#region Find Video By ID

		public BCVideo FindVideoById(long videoId) {
			return FindVideoById(videoId, null);
		}

		public BCVideo FindVideoById(long videoId, List<VideoFields> video_fields) {
			return FindVideoById(videoId, video_fields, null);
		}

		public BCVideo FindVideoById(long videoId, List<VideoFields> video_fields, List<String> custom_fields) {
			return FindVideoById(videoId, video_fields, custom_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// Finds a single video with the specified id.
		/// </summary>
		/// <param name="videoId">
		/// The id of the video you would like to retrieve.
		/// </param>
		/// <param name="video_fields">
		/// A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="custom_fields">
		/// A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <returns>
		/// Returns a BCVideo item
		/// </returns>
		public BCVideo FindVideoById(long videoId, List<VideoFields> video_fields, List<String> custom_fields, MediaDeliveryTypeEnum media_delivery) {

			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_video_by_id");
			reqparams.Add("video_id", videoId.ToString());
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));

			//Get the JSon reader returned from the APIRequest
			QueryResultPair qrp = BCAPIRequest.ExecuteRead(reqparams, Account);

			return JSON.Converter.Deserialize<BCVideo>(qrp.JsonResult);
		}

		#endregion Find Video By ID

		#region Find Related Videos

		public BCQueryResult FindRelatedVideos(long videoId) {
			return FindRelatedVideos(videoId, -1);
		}

		public BCQueryResult FindRelatedVideos(long videoId, int howMany) {
			return FindRelatedVideos(videoId, howMany, null);
		}

		public BCQueryResult FindRelatedVideos(long videoId, int howMany, List<VideoFields> video_fields) {
			return FindRelatedVideos(videoId, howMany, video_fields, null);
		}

		public BCQueryResult FindRelatedVideos(long videoId, int howMany, List<VideoFields> video_fields, List<string> custom_fields) {
			return FindRelatedVideos(videoId, howMany, video_fields, custom_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// Finds videos related to the given video. Combines the name and short description of the given 
		/// video and searches for any partial matches in the name, description, and tags of all videos in 
		/// the Brightcove media library for this account. More precise ways of finding related videos include 
		/// tagging your videos by subject and using the find_videos_by_tags method to find videos that share 
		/// the same tags: or creating a playlist that includes videos that you know are related. 
		/// </summary>
		/// <param name="videoId">
		/// The id of the video we'd like related videos for.
		/// </param>
		/// <param name="howMany">
		/// Number of videos returned (-1 will return all) defaults to -1
		/// </param>
		/// <param name="video_fields">
		/// A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="custom_fields">
		/// A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <returns>
		/// Returns a BCQueryResult item
		/// </returns>
		public BCQueryResult FindRelatedVideos(long videoId, int howMany, List<VideoFields> video_fields, List<string> custom_fields, MediaDeliveryTypeEnum media_delivery) {

			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_related_videos");
			reqparams.Add("video_id", videoId.ToString());
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));
			if (howMany >= 0) reqparams.Add("page_size", howMany.ToString());

			return MultipleQueryHandler(reqparams, BCObjectType.videos, Account);
		}

		#endregion Find Related Videos

		#region Find Videos By IDs

		public BCQueryResult FindVideosByIds(List<long> videoIds) {
			return FindVideosByIds(videoIds, null);
		}

		public BCQueryResult FindVideosByIds(List<long> videoIds, List<VideoFields> video_fields) {
			return FindVideosByIds(videoIds, video_fields, null);
		}

		public BCQueryResult FindVideosByIds(List<long> videoIds, List<VideoFields> video_fields, List<String> custom_fields) {
			return FindVideosByIds(videoIds, video_fields, custom_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// Find multiple videos, given their ids.
		/// </summary>
		/// <param name="videoIds">
		/// The list of video ids for the videos we'd like to retrieve.
		/// </param>
		/// <param name="video_fields">
		/// A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="custom_fields">
		/// A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <returns>
		/// Returns a BCQueryResult item
		/// </returns>
		public BCQueryResult FindVideosByIds(List<long> videoIds, List<VideoFields> video_fields, List<String> custom_fields, MediaDeliveryTypeEnum media_delivery) {

			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_videos_by_ids");
			reqparams.Add("video_ids", Implode(videoIds));
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));
			reqparams.Add("page_size", "-1");

			return MultipleQueryHandler(reqparams, BCObjectType.videos, Account);
		}

		#endregion Find Videos By IDs

		#region Find Video By Reference ID

		public BCVideo FindVideoByReferenceId(String referenceId) {
			return FindVideoByReferenceId(referenceId, null);
		}

		public BCVideo FindVideoByReferenceId(String referenceId, List<VideoFields> video_fields) {
			return FindVideoByReferenceId(referenceId, video_fields, null);
		}

		public BCVideo FindVideoByReferenceId(String referenceId, List<VideoFields> video_fields, List<String> custom_fields) {
			return FindVideoByReferenceId(referenceId, video_fields, custom_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// Find a video based on its publisher-assigned reference id.
		/// </summary>
		/// <param name="referenceId">
		/// The publisher-assigned reference id for the video we're searching for.
		/// </param>
		/// <param name="video_fields">
		/// A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="custom_fields">
		/// A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <returns>
		/// Returns a BCVideo item
		/// </returns>
		public BCVideo FindVideoByReferenceId(String referenceId, List<VideoFields> video_fields, List<String> custom_fields, MediaDeliveryTypeEnum media_delivery) {

			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_video_by_reference_id");
			reqparams.Add("reference_id", referenceId);
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));

			//Get the JSon reader returned from the APIRequest
			string jsonStr = BCAPIRequest.ExecuteRead(reqparams, Account).JsonResult;
			return JSON.Converter.Deserialize<BCVideo>(jsonStr);
		}

		#endregion Find Video By Reference ID

		#region Find Videos By Reference IDs

		public BCQueryResult FindVideosByReferenceIds(List<String> referenceIds) {
			return FindVideosByReferenceIds(referenceIds, null);
		}

		public BCQueryResult FindVideosByReferenceIds(List<String> referenceIds, List<VideoFields> video_fields) {
			return FindVideosByReferenceIds(referenceIds, video_fields, null);
		}

		public BCQueryResult FindVideosByReferenceIds(List<String> referenceIds, List<VideoFields> video_fields, List<String> custom_fields) {
			return FindVideosByReferenceIds(referenceIds, video_fields, custom_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// Find multiple videos based on their publisher-assigned reference ids.
		/// </summary>
		/// <param name="referenceIds">
		/// The list of reference ids for the videos we'd like to retrieve
		/// </param>
		/// <param name="video_fields">
		/// A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="custom_fields">
		/// A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <returns>
		/// Returns a BCQueryResult item
		/// </returns>
		public BCQueryResult FindVideosByReferenceIds(List<String> referenceIds, List<VideoFields> video_fields, List<String> custom_fields, MediaDeliveryTypeEnum media_delivery) {

			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_videos_by_reference_ids");
			reqparams.Add("reference_ids", Implode(referenceIds));
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));

			return MultipleQueryHandler(reqparams, BCObjectType.videos, Account);
		}

		#endregion Find Videos By Reference IDs

		#region Find Videos By User ID

		public BCQueryResult FindVideosByUserId(long userId) {
			return FindVideosByUserId(userId, -1, BCSortByType.CREATION_DATE, BCSortOrderType.ASC, null, null);
		}

		public BCQueryResult FindVideosByUserId(long userId, int howMany) {
			return FindVideosByUserId(userId, howMany, BCSortByType.CREATION_DATE, BCSortOrderType.ASC, null, null);
		}

		public BCQueryResult FindVideosByUserId(long userId, BCSortOrderType sortOrder) {
			return FindVideosByUserId(userId, -1, BCSortByType.CREATION_DATE, sortOrder, null, null);
		}

		public BCQueryResult FindVideosByUserId(long userId, BCSortByType sortBy) {
			return FindVideosByUserId(userId, -1, sortBy, BCSortOrderType.ASC, null, null);
		}

		public BCQueryResult FindVideosByUserId(long userId, BCSortByType sortBy, BCSortOrderType sortOrder) {
			return FindVideosByUserId(userId, -1, sortBy, sortOrder, null, null);
		}

		public BCQueryResult FindVideosByUserId(long userId, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder) {
			return FindVideosByUserId(userId, howMany, sortBy, sortOrder, null, null);
		}

		public BCQueryResult FindVideosByUserId(long userId, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields) {
			return FindVideosByUserId(userId, howMany, sortBy, sortOrder, video_fields, null);
		}

		public BCQueryResult FindVideosByUserId(long userId, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<string> custom_fields) {
			return FindVideosByUserId(userId, howMany, sortBy, sortOrder, video_fields, custom_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// Retrieves the videos uploaded by the specified user id.
		/// </summary>
		/// <param name="userId">
		///  The id of the user whose videos we'd like to retrieve.
		/// </param>
		/// <param name="howMany">
		/// Number of videos returned (-1 will return all) defaults to -1
		/// </param>
		/// <param name="sortBy">
		/// The field by which to sort (defaults to CREATION_DATE)
		/// </param>
		/// <param name="sortOrder">
		/// The direction by which to sort (default to DESC)
		/// </param>
		/// <param name="video_fields">
		/// A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="custom_fields">
		/// A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <returns>
		/// Returns a BCQueryResult item
		/// </returns>
		public BCQueryResult FindVideosByUserId(long userId, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<string> custom_fields, MediaDeliveryTypeEnum media_delivery) {

			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_videos_by_user_id");
			reqparams.Add("user_id", userId.ToString());
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));
			reqparams.Add("sort_order", sortOrder.ToString());
			reqparams.Add("sort_by", sortBy.ToString());
			if (howMany >= 0) reqparams.Add("page_size", howMany.ToString());

			return MultipleQueryHandler(reqparams, BCObjectType.videos, Account);
		}

		#endregion Find Videos By User ID

		#region Find Videos By Campaign ID

		public BCQueryResult FindVideosByCampaignId(long campaignId) {
			return FindVideosByCampaignId(campaignId, -1, BCSortByType.CREATION_DATE, BCSortOrderType.ASC, null, null);
		}

		public BCQueryResult FindVideosByCampaignId(long campaignId, int howMany) {
			return FindVideosByCampaignId(campaignId, howMany, BCSortByType.CREATION_DATE, BCSortOrderType.ASC, null, null);
		}

		public BCQueryResult FindVideosByCampaignId(long campaignId, BCSortOrderType sortOrder) {
			return FindVideosByCampaignId(campaignId, -1, BCSortByType.CREATION_DATE, sortOrder, null, null);
		}

		public BCQueryResult FindVideosByCampaignId(long campaignId, BCSortByType sortBy) {
			return FindVideosByCampaignId(campaignId, -1, sortBy, BCSortOrderType.ASC, null, null);
		}

		public BCQueryResult FindVideosByCampaignId(long campaignId, BCSortByType sortBy, BCSortOrderType sortOrder) {
			return FindVideosByCampaignId(campaignId, -1, sortBy, sortOrder, null, null);
		}

		public BCQueryResult FindVideosByCampaignId(long campaignId, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder) {
			return FindVideosByCampaignId(campaignId, howMany, sortBy, sortOrder, null, null);
		}

		public BCQueryResult FindVideosByCampaignId(long campaignId, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields) {
			return FindVideosByCampaignId(campaignId, howMany, sortBy, sortOrder, video_fields, null);
		}

		public BCQueryResult FindVideosByCampaignId(long campaignId, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<string> custom_fields) {
			return FindVideosByCampaignId(campaignId, howMany, sortBy, sortOrder, video_fields, custom_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// Gets all the videos associated with the given campaign id
		/// </summary>
		/// <param name="campaignId">
		/// The id of the campaign you'd like to fetch videos for.
		/// </param>
		/// <param name="howMany">
		/// Number of videos returned (-1 will return all) defaults to -1
		/// </param>
		/// <param name="sortBy">
		/// The field by which to sort (defaults to CREATION_DATE)
		/// </param>
		/// <param name="sortOrder">
		/// The direction by which to sort (default to DESC)
		/// </param>
		/// <param name="video_fields">
		/// A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="custom_fields">
		/// A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <returns>
		/// Returns a BCQueryResult item
		/// </returns>
		public BCQueryResult FindVideosByCampaignId(long campaignId, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<string> custom_fields, MediaDeliveryTypeEnum media_delivery) {

			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_videos_by_campaign_id");
			reqparams.Add("campaign_id", campaignId.ToString());
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));
			reqparams.Add("sort_order", sortOrder.ToString());
			reqparams.Add("sort_by", sortBy.ToString());
			if (howMany >= 0) reqparams.Add("page_size", howMany.ToString());

			return MultipleQueryHandler(reqparams, BCObjectType.videos, Account);
		}

		#endregion Find Videos By Campaign ID

		#region Find Modified Videos

		public BCQueryResult FindModifiedVideos(DateTime from_date) {
			return FindModifiedVideos(from_date, -1, BCSortOrderType.ASC);
		}

		public BCQueryResult FindModifiedVideos(DateTime from_date, BCSortOrderType sortOrder) {
			return FindModifiedVideos(from_date, -1, BCSortByType.CREATION_DATE, sortOrder);
		}

		public BCQueryResult FindModifiedVideos(DateTime from_date, BCSortByType sortBy) {
			return FindModifiedVideos(from_date, -1, sortBy, BCSortOrderType.ASC);
		}

		public BCQueryResult FindModifiedVideos(DateTime from_date, int howMany) {
			return FindModifiedVideos(from_date, howMany, BCSortOrderType.ASC);
		}

		public BCQueryResult FindModifiedVideos(DateTime from_date, int howMany, BCSortOrderType sortOrder) {
			return FindModifiedVideos(from_date, howMany, BCSortByType.CREATION_DATE, sortOrder);
		}

		public BCQueryResult FindModifiedVideos(DateTime from_date, int howMany, BCSortByType sortBy) {
			return FindModifiedVideos(from_date, howMany, sortBy, BCSortOrderType.ASC);
		}

		public BCQueryResult FindModifiedVideos(DateTime from_date, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder) {
			return FindModifiedVideos(from_date, howMany, sortBy, sortOrder, null);
		}

		public BCQueryResult FindModifiedVideos(DateTime from_date, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields) {
			return FindModifiedVideos(from_date, howMany, sortBy, sortOrder, video_fields, null);
		}

		public BCQueryResult FindModifiedVideos(DateTime from_date, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<string> custom_fields) {
			return FindModifiedVideos(from_date, howMany, sortBy, sortOrder, video_fields, custom_fields, null);
		}

		public BCQueryResult FindModifiedVideos(DateTime from_date, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<string> custom_fields, List<string> filter) {
			return FindModifiedVideos(from_date, howMany, sortBy, sortOrder, video_fields, custom_fields, filter, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// This will find all modified videos
		/// </summary>
		/// <param name="from_date">The date, specified in minutes since January 1st, 1970 00:00:00 GMT, of the oldest Video which you would like returned.</param>
		/// <param name="howMany">Number of items returned per page. A page is a subset of all of the items that satisfy the request. The maximum page size is 25; if you do not set this argument, or if you set it to an integer > 25, your results will come back as if you had set page_size=25.</param>
		/// <param name="sortBy">The field by which to sort the results. A SortByType: One of PUBLISH_DATE, CREATION_DATE, MODIFIED_DATE, PLAYS_TOTAL, PLAYS_TRAILING_WEEK.</param>
		/// <param name="sortOrder">How to order the results: ascending (ASC) or descending (DESC).</param>
		/// <param name="video_fields">A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. If you omit this parameter, the method returns the following fields of the video: id, name, shortDescription, longDescription, creationDate, publisheddate, lastModifiedDate, linkURL, linkText, tags, videoStillURL, thumbnailURL, referenceId, length, economics, playsTotal, playsTrailingWeek. If you use a token with URL access, this method also returns FLVURL, renditions, FLVFullLength, videoFullLength.</param>
		/// <param name="custom_fields">A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. If you omit this parameter, no custom fields are returned, unless you include the value 'customFields' in the video_fields parameter.</param>
		/// <param name="filter">A comma-separated list of filters, specifying which categories of videos you would like returned. Valid filter values are PLAYABLE, UNSCHEDULED, INACTIVE, and DELETED.</param>
		/// <param name="media_delivery">If universal delivery service is enabled for your account, set this optional parameter to http to return video by HTTP, rather than streaming. Meaningful only if used together with the video_fields=FLVURL, videoFullLength, or renditions parameters. This is a MediaDeliveryTypeEnum with a value of http or default.</param>
		/// <returns>Returns a BCQueryResult Item</returns>
		public BCQueryResult FindModifiedVideos(DateTime from_date, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<string> custom_fields, List<string> filter, MediaDeliveryTypeEnum media_delivery) {

			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_modified_videos");
			if (from_date != null) reqparams.Add("from_date", from_date.ToUnixTime());
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));
			if (filter != null) reqparams.Add("filter", Implode(filter));
			reqparams.Add("sort_order", sortOrder.ToString());
			reqparams.Add("sort_by", sortBy.ToString());
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (howMany >= 0) reqparams.Add("page_size", howMany.ToString());

			return MultipleQueryHandler(reqparams, BCObjectType.videos, Account);
		}

		#endregion Find Modified Videos

		#region Find Videos By Text

		public BCQueryResult FindVideosByText(string text) {
			return FindVideosByText(text, -1, BCSortByType.CREATION_DATE, BCSortOrderType.ASC, null, null);
		}

		public BCQueryResult FindVideosByText(string text, int howMany) {
			return FindVideosByText(text, howMany, BCSortByType.CREATION_DATE, BCSortOrderType.ASC, null, null);
		}

		public BCQueryResult FindVideosByText(string text, BCSortOrderType sortOrder) {
			return FindVideosByText(text, -1, BCSortByType.CREATION_DATE, sortOrder, null, null);
		}

		public BCQueryResult FindVideosByText(string text, BCSortByType sortBy) {
			return FindVideosByText(text, -1, sortBy, BCSortOrderType.ASC, null, null);
		}

		public BCQueryResult FindVideosByText(string text, BCSortByType sortBy, BCSortOrderType sortOrder) {
			return FindVideosByText(text, -1, sortBy, sortOrder, null, null);
		}

		public BCQueryResult FindVideosByText(string text, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder) {
			return FindVideosByText(text, howMany, sortBy, sortOrder, null, null);
		}

		public BCQueryResult FindVideosByText(string text, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields) {
			return FindVideosByText(text, howMany, sortBy, sortOrder, video_fields, null);
		}

		public BCQueryResult FindVideosByText(string text, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<string> custom_fields) {
			return FindVideosByText(text, howMany, sortBy, sortOrder, video_fields, custom_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// Searches through all the videos in this account, and returns a collection of videos whose name, short description, or long description contain a match for the specified text. 
		/// </summary>
		/// <param name="text">
		/// The text we're searching for.
		/// </param>
		/// <param name="howMany">
		/// Number of videos returned (-1 will return all) defaults to -1
		/// </param>
		/// <param name="sortBy">
		/// The field by which to sort (defaults to CREATION_DATE)
		/// </param>
		/// <param name="sortOrder">
		/// The direction by which to sort (default to DESC)
		/// </param>
		/// <param name="video_fields">
		/// A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="custom_fields">
		/// A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <returns>
		/// Returns a BCQueryResult item
		/// </returns>
        public BCQueryResult FindVideosByText(string text, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<string> custom_fields, MediaDeliveryTypeEnum media_delivery)
        {
            return FindVideosByText(text, howMany, sortBy, sortOrder, video_fields, custom_fields, media_delivery, 0, false);
        }

        public BCQueryResult FindVideosByText_deprecated(string text, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<string> custom_fields, MediaDeliveryTypeEnum media_delivery)
        {
			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_videos_by_text");
			reqparams.Add("text", text);
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));
			reqparams.Add("sort_order", sortOrder.ToString());
			reqparams.Add("sort_by", sortBy.ToString());
			if (howMany >= 0) reqparams.Add("page_size", howMany.ToString());

			return MultipleQueryHandler(reqparams, BCObjectType.videos, Account);
		}

        /// <summary>
        /// Searches through all the videos in this account, and returns a collection of videos whose name, short description, or long description contain a match for the specified text. 
        /// </summary>
        /// <param name="text">
        /// The text we're searching for.
        /// </param>
        /// <param name="howMany">
        /// Number of videos returned (-1 will return all) defaults to -1
        /// </param>
        /// <param name="sortBy">
        /// The field by which to sort (defaults to CREATION_DATE)
        /// </param>
        /// <param name="sortOrder">
        /// The direction by which to sort (default to DESC)
        /// </param>
        /// <param name="video_fields">
        /// A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
        /// </param>
        /// <param name="custom_fields">
        /// A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
        /// </param>
        /// <returns>
        /// Returns a BCQueryResult item
        /// </returns>
        public BCQueryResult FindVideosByText(string text, int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<string> custom_fields, MediaDeliveryTypeEnum media_delivery, int pageNumber, bool getItemCount)
        {
            Dictionary<String, String> reqparams = new Dictionary<string, string>();
            string encText = HttpUtility.UrlEncode(text);
            string customFieldList = string.Empty;

            //Build the REST parameter list
            reqparams.Add("command", "search_videos");

            if (custom_fields != null)
            {
                foreach (string field in custom_fields)
                {
                    customFieldList += "&any=" + field + ":" + encText;
                }
            }

            // HACK HACK - the API has changed at Brightcove - now takes multiple "any" params, but this SDK does not allow duplicate keys
            reqparams.Add("any=" + encText + customFieldList + "&any", "tag:" + text);

            reqparams.Add("media_delivery", media_delivery.ToString());
            if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
            if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));
            reqparams.Add("sort_order", sortOrder.ToString());
            if (howMany >= 0) reqparams.Add("page_size", howMany.ToString());
            if (pageNumber > 0) reqparams.Add("page_number", pageNumber.ToString());
            if (getItemCount) reqparams.Add("get_item_number", "true");

            return MultipleQueryHandler(reqparams, BCObjectType.videos, Account);
        }

		#endregion Find Videos By Text

		#region Find Videos By Tags

		public BCQueryResult FindVideosByTags(List<String> and_tags, List<String> or_tags) {
			return FindVideosByTags(and_tags, or_tags, -1, BCSortByType.CREATION_DATE, BCSortOrderType.ASC, null);
		}

		public BCQueryResult FindVideosByTags(List<String> and_tags, List<String> or_tags, int pageSize) {
			return FindVideosByTags(and_tags, or_tags, pageSize, BCSortByType.CREATION_DATE, BCSortOrderType.ASC, null);
		}

		public BCQueryResult FindVideosByTags(List<String> and_tags, List<String> or_tags, BCSortByType sortBy) {
			return FindVideosByTags(and_tags, or_tags, -1, sortBy, BCSortOrderType.ASC);
		}

		public BCQueryResult FindVideosByTags(List<String> and_tags, List<String> or_tags, BCSortOrderType sortOrder) {
			return FindVideosByTags(and_tags, or_tags, -1, BCSortByType.CREATION_DATE, sortOrder);
		}

		public BCQueryResult FindVideosByTags(List<String> and_tags, List<String> or_tags, BCSortByType sortBy, BCSortOrderType sortOrder) {
			return FindVideosByTags(and_tags, or_tags, -1, sortBy, sortOrder);
		}

		public BCQueryResult FindVideosByTags(List<String> and_tags, List<String> or_tags, int pageSize, BCSortByType sortBy, BCSortOrderType sortOrder) {
			return FindVideosByTags(and_tags, or_tags, pageSize, sortBy, sortOrder, null);
		}

		public BCQueryResult FindVideosByTags(List<String> and_tags, List<String> or_tags, int pageSize, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields) {
			return FindVideosByTags(and_tags, or_tags, pageSize, sortBy, sortOrder, video_fields, null);
		}

		public BCQueryResult FindVideosByTags(List<String> and_tags, List<String> or_tags, int pageSize, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<String> custom_fields) {
			return FindVideosByTags(and_tags, or_tags, pageSize, 0, sortBy, sortOrder, video_fields, custom_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		public BCQueryResult FindVideosByTags(List<String> and_tags, List<String> or_tags, int pageSize, int pageNumber, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<String> custom_fields) {
			return FindVideosByTags(and_tags, or_tags, pageSize, pageNumber, sortBy, sortOrder, video_fields, custom_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// Performs a search on all the tags of the videos in this account, and returns a collection of videos that contain the specified tags. Note that tags are not case-sensitive. 
		/// </summary>
		/// <param name="and_tags">
		/// Limit the results to those that contain all of these tags.
		/// </param>
		/// <param name="or_tags">
		/// Limit the results to those that contain at least one of these tags.
		/// </param>
		/// <param name="pageSize">
		/// Number of videos returned (-1 will return all) defaults to -1 max is 100
		/// </param>
		/// <param name="pageNumber">
		/// The number of page to return. Default is 0 (First page)
		/// </param>
		/// <param name="sortBy">
		/// The field by which to sort (defaults to CREATION_DATE)
		/// </param>
		/// <param name="sortOrder">
		/// The direction by which to sort (default to DESC)
		/// </param>
		/// <param name="video_fields">
		/// A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="custom_fields">
		/// A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <returns>
		/// Returns a BCQueryResult item
		/// </returns>
		public BCQueryResult FindVideosByTags(List<String> and_tags, List<String> or_tags, int pageSize, int pageNumber, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<String> custom_fields, MediaDeliveryTypeEnum media_delivery) {

			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_videos_by_tags");
			if (and_tags != null) reqparams.Add("and_tags", Implode(and_tags));
			if (or_tags != null) reqparams.Add("or_tags", Implode(or_tags));
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));
			reqparams.Add("sort_order", sortOrder.ToString());
			reqparams.Add("sort_by", sortBy.ToString());
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (pageSize >= 0) reqparams.Add("page_size", pageSize.ToString());
			if (pageNumber > 0) reqparams.Add("page_number", pageNumber.ToString());

			return MultipleQueryHandler(reqparams, BCObjectType.videos, Account);
		}

		#endregion Find Videos By Tags

		#endregion Video Read
	}
}
