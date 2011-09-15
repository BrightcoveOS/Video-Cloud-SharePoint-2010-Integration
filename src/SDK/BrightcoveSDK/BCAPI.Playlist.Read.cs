using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrightcoveSDK.Entities.Containers;
using BrightcoveSDK.Extensions;
using BrightcoveSDK.HTTP;
using BrightcoveSDK.Media;

namespace BrightcoveSDK
{
	public partial class BCAPI
	{
		#region Playlist Read

		#region Find All Playlists

		public BCQueryResult FindAllPlaylists() {
			return FindAllPlaylists(-1);
		}

		public BCQueryResult FindAllPlaylists(int howMany) {
			return FindAllPlaylists(howMany, BCSortByType.CREATION_DATE, BCSortOrderType.ASC, null);
		}

		public BCQueryResult FindAllPlaylists(int howMany, BCSortOrderType sortOrder) {
			return FindAllPlaylists(howMany, BCSortByType.CREATION_DATE, sortOrder, null);
		}

		public BCQueryResult FindAllPlaylists(int howMany, BCSortByType sortBy) {
			return FindAllPlaylists(howMany, sortBy, BCSortOrderType.ASC, null);
		}

		public BCQueryResult FindAllPlaylists(int howMany, BCSortByType sortBy, BCSortOrderType sortOrder) {
			return FindAllPlaylists(howMany, sortBy, sortOrder, null);
		}

		public BCQueryResult FindAllPlaylists(int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields) {
			return FindAllPlaylists(howMany, sortBy, sortOrder, video_fields, null);
		}

		public BCQueryResult FindAllPlaylists(int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<String> custom_fields) {
			return FindAllPlaylists(howMany, sortBy, sortOrder, video_fields, custom_fields, null);
		}

		public BCQueryResult FindAllPlaylists(int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<String> custom_fields, List<string> playlist_fields) {
			return FindAllPlaylists(howMany, sortBy, sortOrder, video_fields, custom_fields, playlist_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// Find all playlists in this account.
		/// </summary>
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
		/// <param name="playlist_fields">
		/// A comma-separated list of the fields you wish to have populated in the playlists contained in the returned object. Passing null populates with all fields. 
		/// </param>
		/// <returns>
		/// Returns a BCQueryResult item
		/// </returns>
        public BCQueryResult FindAllPlaylists(int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<String> custom_fields, List<string> playlist_fields, MediaDeliveryTypeEnum media_delivery)
        {
            return FindAllPlaylists(howMany, sortBy, sortOrder, video_fields, custom_fields, playlist_fields, MediaDeliveryTypeEnum.DEFAULT, 0, false);
        }

		public BCQueryResult FindAllPlaylists(int howMany, BCSortByType sortBy, BCSortOrderType sortOrder, List<VideoFields> video_fields, List<String> custom_fields, List<string> playlist_fields, MediaDeliveryTypeEnum media_delivery, int pageNumber, bool getItemCount)
        {
			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_all_playlists");
			reqparams.Add("sort_order", sortOrder.ToString());
			reqparams.Add("sort_by", sortBy.ToString());
			reqparams.Add("media_delivery", media_delivery.ToString());
            if (howMany >= 0) reqparams.Add("page_size", howMany.ToString());
            if (pageNumber > 0) reqparams.Add("page_number", pageNumber.ToString());
            if (getItemCount) reqparams.Add("get_item_count", "true");
            if (playlist_fields != null) reqparams.Add("playlist_fields", Implode(playlist_fields));
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));

			BCQueryResult qr = MultipleQueryHandler(reqparams, BCObjectType.playlists, Account);

			return qr;
		}

		#endregion Find All Playlists

		#region Find Playlist By Id

		public BCPlaylist FindPlaylistById(long playlist_id) {
			return FindPlaylistById(playlist_id, null);
		}

		public BCPlaylist FindPlaylistById(long playlist_id, List<VideoFields> video_fields) {
			return FindPlaylistById(playlist_id, video_fields, null);
		}

		public BCPlaylist FindPlaylistById(long playlist_id, List<VideoFields> video_fields, List<string> custom_fields) {
			return FindPlaylistById(playlist_id, video_fields, custom_fields, null);
		}

		public BCPlaylist FindPlaylistById(long playlist_id, List<VideoFields> video_fields, List<string> custom_fields, List<PlaylistFields> playlist_fields) {
			return FindPlaylistById(playlist_id, video_fields, custom_fields, playlist_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// Finds a particular playlist based on its id.
		/// </summary>
		/// <param name="playlist_id">
		/// The id of the playlist requested.
		/// </param>
		/// <param name="video_fields">
		/// A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="custom_fields">
		/// A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="playlist_fields">
		/// A comma-separated list of the fields you wish to have populated in the playlists contained in the returned object. Passing null populates with all fields. 
		/// </param>
		/// <returns>
		/// Returns a BCPlaylist item
		/// </returns>
		public BCPlaylist FindPlaylistById(long playlist_id, List<VideoFields> video_fields, List<string> custom_fields, List<PlaylistFields> playlist_fields, MediaDeliveryTypeEnum media_delivery) {

			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_playlist_by_id");
			reqparams.Add("playlist_id", playlist_id.ToString());
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (playlist_fields != null) reqparams.Add("playlist_fields", playlist_fields.ToFieldString());
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));

			//Get the JSon reader returned from the APIRequest
			QueryResultPair qrp = BCAPIRequest.ExecuteRead(reqparams, Account);

			return JSON.Converter.Deserialize<BCPlaylist>(qrp.JsonResult);
		}

		#endregion Find Playlist By Id

		#region Find Playlists By Ids

		public BCQueryResult FindPlaylistsByIds(List<long> playlist_ids) {
			return FindPlaylistsByIds(playlist_ids, null);
		}

		public BCQueryResult FindPlaylistsByIds(List<long> playlist_ids, List<VideoFields> video_fields) {
			return FindPlaylistsByIds(playlist_ids, video_fields, null);
		}

		public BCQueryResult FindPlaylistsByIds(List<long> playlist_ids, List<VideoFields> video_fields, List<string> custom_fields) {
			return FindPlaylistsByIds(playlist_ids, video_fields, custom_fields, null);
		}

		public BCQueryResult FindPlaylistsByIds(List<long> playlist_ids, List<VideoFields> video_fields, List<string> custom_fields, List<PlaylistFields> playlist_fields) {
			return FindPlaylistsByIds(playlist_ids, video_fields, custom_fields, playlist_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// Retrieve a set of playlists based on their ids.
		/// </summary>
		/// <param name="playlist_ids">
		/// The ids of the playlists you would like retrieved.
		/// </param>
		/// <param name="video_fields">
		/// A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="custom_fields">
		/// A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="playlist_fields">
		/// A comma-separated list of the fields you wish to have populated in the playlists contained in the returned object. Passing null populates with all fields. 
		/// </param>
		/// <returns>
		/// Returns a BCQueryResult item
		/// </returns>
		public BCQueryResult FindPlaylistsByIds(List<long> playlist_ids, List<VideoFields> video_fields, List<string> custom_fields, List<PlaylistFields> playlist_fields, MediaDeliveryTypeEnum media_delivery) {

			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_playlists_by_ids");
			reqparams.Add("playlist_ids", Implode(playlist_ids));
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (playlist_fields != null) reqparams.Add("playlist_fields", playlist_fields.ToFieldString());
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));

			BCQueryResult qr = MultipleQueryHandler(reqparams, BCObjectType.playlists, Account);

			return qr;

		}

		#endregion Find Playlists By Ids

		#region Find Playlist By Reference Id

		public BCPlaylist FindPlaylistByReferenceId(string reference_id) {
			return FindPlaylistByReferenceId(reference_id, null);
		}

		public BCPlaylist FindPlaylistByReferenceId(string reference_id, List<VideoFields> video_fields) {
			return FindPlaylistByReferenceId(reference_id, video_fields, null);
		}

		public BCPlaylist FindPlaylistByReferenceId(string reference_id, List<VideoFields> video_fields, List<string> custom_fields) {
			return FindPlaylistByReferenceId(reference_id, video_fields, custom_fields, null);
		}

		public BCPlaylist FindPlaylistByReferenceId(string reference_id, List<VideoFields> video_fields, List<string> custom_fields, List<PlaylistFields> playlist_fields) {
			return FindPlaylistByReferenceId(reference_id, video_fields, custom_fields, playlist_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// Retrieve a playlist based on its publisher-assigned reference id.
		/// </summary>
		/// <param name="reference_id">
		/// The reference id of the playlist we'd like to retrieve.
		/// </param>
		/// <param name="video_fields">
		/// A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="custom_fields">
		/// A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="playlist_fields">
		/// A comma-separated list of the fields you wish to have populated in the playlists contained in the returned object. Passing null populates with all fields. 
		/// </param>
		/// <returns>
		/// Returns a BCPlaylist item
		/// </returns>
		public BCPlaylist FindPlaylistByReferenceId(string reference_id, List<VideoFields> video_fields, List<string> custom_fields, List<PlaylistFields> playlist_fields, MediaDeliveryTypeEnum media_delivery) {

			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_playlist_by_reference_id");
			reqparams.Add("reference_id", reference_id);
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (playlist_fields != null) reqparams.Add("playlist_fields", playlist_fields.ToFieldString());
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));

			//Get the JSon reader returned from the APIRequest
			string jsonStr = BCAPIRequest.ExecuteRead(reqparams, Account).JsonResult;
			return JSON.Converter.Deserialize<BCPlaylist>(jsonStr);
		}

		#endregion Find Playlist By Reference Id

		#region Find Playlists By Reference Ids

		public BCQueryResult FindPlaylistsByReferenceIds(List<string> reference_ids) {
			return FindPlaylistsByReferenceIds(reference_ids, null);
		}

		public BCQueryResult FindPlaylistsByReferenceIds(List<string> reference_ids, List<VideoFields> video_fields) {
			return FindPlaylistsByReferenceIds(reference_ids, video_fields, null);
		}

		public BCQueryResult FindPlaylistsByReferenceIds(List<string> reference_ids, List<VideoFields> video_fields, List<string> custom_fields) {
			return FindPlaylistsByReferenceIds(reference_ids, video_fields, custom_fields, null);
		}

		public BCQueryResult FindPlaylistsByReferenceIds(List<string> reference_ids, List<VideoFields> video_fields, List<string> custom_fields, List<PlaylistFields> playlist_fields) {
			return FindPlaylistsByReferenceIds(reference_ids, video_fields, custom_fields, playlist_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// Retrieve multiple playlists based on their publisher-assigned reference ids.
		/// </summary>
		/// <param name="reference_ids">
		/// The reference ids of the playlists we'd like to retrieve.
		/// </param>
		/// <param name="video_fields">
		/// A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="custom_fields">
		/// A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="playlist_fields">
		/// A comma-separated list of the fields you wish to have populated in the playlists contained in the returned object. Passing null populates with all fields. 
		/// </param>
		/// <returns>
		/// Returns a BCQueryResult item
		/// </returns>
		public BCQueryResult FindPlaylistsByReferenceIds(List<string> reference_ids, List<VideoFields> video_fields, List<string> custom_fields, List<PlaylistFields> playlist_fields, MediaDeliveryTypeEnum media_delivery) {

			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_playlists_by_reference_ids");
			reqparams.Add("reference_ids", Implode(reference_ids));
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (playlist_fields != null) reqparams.Add("playlist_fields", playlist_fields.ToFieldString());
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));

			BCQueryResult qr = MultipleQueryHandler(reqparams, BCObjectType.playlists, Account);

			return qr;
		}

		#endregion Find Playlists By Ids

		#region Find Playlists For Player Id

		public BCQueryResult FindPlaylistsForPlayerId(long player_id) {
			return FindPlaylistsForPlayerId(player_id, -1);
		}

		public BCQueryResult FindPlaylistsForPlayerId(long player_id, int howMany) {
			return FindPlaylistsForPlayerId(player_id, howMany, null);
		}

		public BCQueryResult FindPlaylistsForPlayerId(long player_id, int howMany, List<VideoFields> video_fields) {
			return FindPlaylistsForPlayerId(player_id, howMany, video_fields, null);
		}

		public BCQueryResult FindPlaylistsForPlayerId(long player_id, int howMany, List<VideoFields> video_fields, List<string> custom_fields) {
			return FindPlaylistsForPlayerId(player_id, howMany, video_fields, custom_fields, null);
		}

		public BCQueryResult FindPlaylistsForPlayerId(long player_id, int howMany, List<VideoFields> video_fields, List<string> custom_fields, List<PlaylistFields> playlist_fields) {
			return FindPlaylistsForPlayerId(player_id, howMany, video_fields, custom_fields, playlist_fields, MediaDeliveryTypeEnum.DEFAULT);
		}

		/// <summary>
		/// Given the id of a player, returns all the playlists assigned to that player.
		/// </summary>
		/// <param name="player_id">
		/// The player id whose playlists we want to return.
		/// </param>
		/// <param name="howMany">
		/// The number of videos to return (-1 will return all) defaults to -1
		/// </param>
		/// <param name="video_fields">
		/// A comma-separated list of the fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="custom_fields">
		/// A comma-separated list of the custom fields you wish to have populated in the videos contained in the returned object. Passing null populates with all fields. (defaults to all) 
		/// </param>
		/// <param name="playlist_fields">
		/// A comma-separated list of the fields you wish to have populated in the playlists contained in the returned object. Passing null populates with all fields. 
		/// </param>
		/// <returns>
		/// Returns a BCQueryResult item
		/// </returns>
		public BCQueryResult FindPlaylistsForPlayerId(long player_id, int howMany, List<VideoFields> video_fields, List<string> custom_fields, List<PlaylistFields> playlist_fields, MediaDeliveryTypeEnum media_delivery) {

			Dictionary<String, String> reqparams = new Dictionary<string, string>();

			//Build the REST parameter list
			reqparams.Add("command", "find_playlists_for_player_id");
			reqparams.Add("player_id", player_id.ToString());
			reqparams.Add("media_delivery", media_delivery.ToString());
			if (howMany >= 0) reqparams.Add("page_size", howMany.ToString());
			if (playlist_fields != null) reqparams.Add("playlist_fields", playlist_fields.ToFieldString());
			if (video_fields != null) reqparams.Add("video_fields", video_fields.ToFieldString());
			if (custom_fields != null) reqparams.Add("custom_fields", Implode(custom_fields));

			BCQueryResult qr = MultipleQueryHandler(reqparams, BCObjectType.playlists, Account);

			return qr;
		}

		#endregion Find Playlists For Player Id

		#endregion Playlist Read
	}
}
