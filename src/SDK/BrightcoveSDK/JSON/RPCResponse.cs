using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BrightcoveSDK.JSON
{
	[DataContract]
	public class RPCResponse
	{

		/// <summary>
		/// The Object that was returned by the invoked method. This must be null in case there was an error invoking the method.
		/// </summary>
		[DataMember]
		public string result { get; set; }

		/// <summary>
		/// An Error object if there was an error invoking the method. It must be null if there was no error.
		/// </summary>
		[DataMember]
		public RPCError error { get; set; }

		/// <summary>
		/// This must be the same id as the request it is responding to.
		/// </summary>
		[DataMember]
		public string id { get; set; }
	}

	[DataContract]
	public class RPCResponse<T>
	{

		/// <summary>
		/// The Object that was returned by the invoked method. This must be null in case there was an error invoking the method.
		/// </summary>
		[DataMember]
		public T result { get; set; }

		/// <summary>
		/// An Error object if there was an error invoking the method. It must be null if there was no error.
		/// </summary>
		[DataMember]
		public RPCError error { get; set; }

		/// <summary>
		/// This must be the same id as the request it is responding to.
		/// </summary>
		[DataMember]
		public string id { get; set; }

		public RPCResponse() {
			error = new RPCError();
		}
	}
}
