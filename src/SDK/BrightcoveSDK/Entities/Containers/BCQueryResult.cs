using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using BrightcoveSDK.Media;
using BrightcoveSDK.Containers;

namespace BrightcoveSDK.Entities.Containers
{
	[DataContract]
	public class BCQueryResult
	{
		[DataMember(Name = "videos")]
		public BCCollection<BCVideo> Videos;
		[DataMember(Name = "playlists")]
		public BCCollection<BCPlaylist> Playlists;
		[DataMember(Name = "page_number")]
		public int PageNumber { get; set; }
		[DataMember(Name = "page_size")]
		public int PageSize { get; set; }
		[DataMember(Name = "total_count")]
		public int TotalCount { get; set; }

		public int MaxToGet = 0;
		public List<QueryResultPair> QueryResults = new List<QueryResultPair>();

		public BCQueryResult() {
			Playlists = new BCCollection<BCPlaylist>();
			Videos = new BCCollection<BCVideo>();
			PageNumber = 0;
			PageSize = 0;
			TotalCount = 0;
		}

		public void Merge(BCQueryResult qr) {

			//if (qr.QueryResults != null && qr.QueryResults.Count > 0)
			//        QueryResults.Add(qr.QueryResults[qr.QueryResults.Count -1]);
			if (qr.Videos != null) Videos.AddRange(qr.Videos);
			if(qr.Playlists != null) Playlists.AddRange(qr.Playlists);
			PageNumber = qr.PageNumber;
			TotalCount = qr.TotalCount;
			PageSize = qr.PageSize;
		}
	}
}
