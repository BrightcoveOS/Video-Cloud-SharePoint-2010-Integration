using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BrightcoveSDK.JSON
{
	public class RPCError
	{

		/// <summary>
		/// The Object that was returned by the invoked method. This must be null in case there was an error invoking the method.
		/// </summary>
		[DataMember]
		public string name { get; set; }

		/// <summary>
		/// An Error object if there was an error invoking the method. It must be null if there was no error.
		/// </summary>
		[DataMember]
		public string message { get; set; }

		/// <summary>
		/// This must be the same id as the request it is responding to.
		/// </summary>
		[DataMember]
		public string code { get; set; }
	}
}
