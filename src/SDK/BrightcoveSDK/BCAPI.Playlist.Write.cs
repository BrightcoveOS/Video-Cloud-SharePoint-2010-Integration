using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrightcoveSDK.JSON;
using BrightcoveSDK.HTTP;
using BrightcoveSDK.Media;

namespace BrightcoveSDK
{
	public partial class BCAPI
	{
		#region Playlist Write

		#region Create Playlist

		/// <summary>
		/// Creates a playlist. This method must be called using an HTTP POST request and JSON parameters.
		/// </summary>
		/// <param name="playlist">
		/// The metadata for the playlist you'd like to create. This takes the form of a JSON object of 
		/// name/value pairs, each of which corresponds to a settable property of the Playlist object. 
		/// Populate the videoIds property of the playlist, not the videos property. 
		/// </param>
		/// <returns>
		/// The ID of the Playlist you created.
		/// </returns>
		public RPCResponse<long> CreatePlaylist(BCPlaylist playlist) {

			// Generate post objects
			Dictionary<string, object> postParams = new Dictionary<string, object>();

			//add video to the post params
			RPCRequest rpc = new RPCRequest();
			rpc.method = "create_playlist";
			rpc.parameters = "\"playlist\": " + playlist.ToCreateJSON() + " ,\"token\": \"" + Account.WriteToken.Value + "\"";
			postParams.Add("json", rpc.ToJSON());

			//Get the JSon reader returned from the APIRequest
			RPCResponse<long> rpcr = BCAPIRequest.ExecuteWrite<long>(postParams, Account);

			return rpcr;
		}

		#endregion Create Playlist

		#region Update Playlist

		/// <summary>
		/// Updates a playlist, specified by playlist id. This method must be called 
		/// using an HTTP POST request and JSON parameters.
		/// </summary>
		/// <param name="playlist">
		/// The metadata for the playlist you'd like to create. This takes the form of a 
		/// JSON object of name/value pairs, each of which corresponds to a settable 
		/// property of the Playlist object. Populate the videoIds property of the 
		/// playlist, not the videos property. 
		/// </param>
		/// <returns></returns>
		public RPCResponse<BCPlaylist> UpdatePlaylist(BCPlaylist playlist) {

			// Generate post objects
			Dictionary<string, object> postParams = new Dictionary<string, object>();

			//add video to the post params
			RPCRequest rpc = new RPCRequest();
			rpc.method = "update_playlist";
			rpc.parameters = "\"playlist\": " + playlist.ToUpdateJSON() + " ,\"token\": \"" + Account.WriteToken.Value + "\"";
			postParams.Add("json", rpc.ToJSON());

			//Get the JSon reader returned from the APIRequest
			RPCResponse<BCPlaylist> rpcr = BCAPIRequest.ExecuteWrite<BCPlaylist>(postParams, Account);

			return rpcr;
		}

		#endregion Update Playlist

		#region Delete Playlist

		//by reference id
		public RPCResponse DeletePlaylist(string reference_id) {
			return DeletePlaylist(reference_id, false);
		}
		public RPCResponse DeletePlaylist(string reference_id, bool cascade) {
			return DeletePlaylist(-1, reference_id, cascade);
		}

		//by video id
		public RPCResponse DeletePlaylist(long playlist_id) {
			return DeletePlaylist(playlist_id, false);
		}
		public RPCResponse DeletePlaylist(long playlist_id, bool cascade) {
			return DeletePlaylist(playlist_id, null, cascade);
		}

		/// <summary>
		/// Deletes a playlist, specified by playlist id.
		/// </summary>
		/// <param name="playlist_id">
		/// the id for the playlist to delete
		/// </param>
		/// <param name="reference_id">
		///	The publisher-assigned reference id of the playlist you'd like to delete.
		/// </param>
		/// <returns>
		/// RPC Response Object
		/// </returns>
		private RPCResponse DeletePlaylist(long playlist_id, string reference_id, bool cascade) {

			// Generate post objects
			Dictionary<string, object> postParams = new Dictionary<string, object>();

			//add video to the post params
			RPCRequest rpc = new RPCRequest();
			rpc.method = "delete_playlist";
			if (playlist_id > -1) {
				rpc.parameters = "\"playlist_id\": " + playlist_id.ToString();
			} else if (reference_id != null) {
				rpc.parameters = "\"reference_id\": " + reference_id.ToString();
			}
			rpc.parameters += ", \"token\": \"" + Account.WriteToken.Value + "\"";
			rpc.parameters += ", \"cascade\": " + cascade.ToString().ToLower();
			postParams.Add("json", rpc.ToJSON());

			//Get the JSon reader returned from the APIRequest
			RPCResponse rpcr = BCAPIRequest.ExecuteWrite(postParams, Account);

			return rpcr;
		}

		#endregion Delete Playlist

		#endregion Playlist Write
	}
}
