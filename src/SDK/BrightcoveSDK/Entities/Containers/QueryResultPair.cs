using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BrightcoveSDK.Entities.Containers
{
	public class QueryResultPair
	{
		private string _query { get; set; }
		private string _json { get; set; }

		public string Query {
			get {
				return HttpUtility.UrlDecode(_query);
			}
			set {
				_query = value;
			}
		}
		public string JsonResult {
			get {
				return _json;
			}
			set {
				_json = value;
			}
		}
		public QueryResultPair(string Query, string JsonResult) {
			_query = Query;
			_json = JsonResult;
		}
	}
}
