using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BrightcoveSDK.JSON
{
	/// <summary>
	/// This is a json standard RPC object
	/// </summary>
	[DataContract]
	public class RPCRequest
	{
		/// <summary>
		/// A String containing the name of the method to be invoked.
		/// </summary>
		[DataMember]
		public string method = "null";

		/// <summary>
		/// An Array of objects to pass as arguments to the method.
		/// </summary>
		[DataMember(Name = "params")]
		public string parameters = "null";

		/// <summary>
		/// The request id. This can be of any type. It is used to match the response with the request that it is replying to.
		/// </summary>
		[DataMember]
		public string id = "null";
	}
}
