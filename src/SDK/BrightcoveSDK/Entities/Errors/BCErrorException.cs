using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace BrightcoveSDK.Errors
{
	[Serializable]
	public class BCErrorException : Exception
	{
		private string stringInfo;

		public override string Message {
			get {
				string message = base.Message;
				if (stringInfo != null) {
					message += Environment.NewLine + stringInfo;
				}
				return message;
			}
		}

		public BCErrorException()
			: base() {

		}

		public BCErrorException(string Message)
			: base(Message) {
		}

		public BCErrorException(string Message, Exception innerException)
			: base(Message, innerException) {
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			info.AddValue("StringInfo", stringInfo);

			base.GetObjectData(info, context);
		}
	}
}
