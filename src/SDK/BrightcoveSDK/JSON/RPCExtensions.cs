using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrightcoveSDK.JSON
{
	public static class RPCExtensions
	{

		#region Extension Methods

		public static string ToJSON(this RPCRequest jsonRPC) {

			string rpc = "{\"method\": \"" + jsonRPC.method + "\", \"params\":";
			if (!jsonRPC.parameters.Equals("null")) {
				rpc += "{" + jsonRPC.parameters + "}";
			}
			else {
				rpc += "null";
			}
			rpc += ", \"id\": " + jsonRPC.id + "}";

			return rpc;
		}

		public static string ToJSON(this RPCResponse jsonRPC) {

			string rpc = "{\"result\": {" + jsonRPC.result + "}, \"error\": " + jsonRPC.error + ", \"id\": " + jsonRPC.id + "}";

			return rpc;
		}

		#endregion
	}
}
