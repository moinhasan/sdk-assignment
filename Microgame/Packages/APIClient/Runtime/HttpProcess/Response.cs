using UnityEngine.Networking;
using UnityEngine;

namespace APIClient.HttpProcess
{
	public class Response {
		private long status;
		private string body;
		private byte[] rawBody;

		public Response(long status, string body, byte[] rawBody) {
			this.status = status;
			this.body = body;
			this.rawBody = rawBody;
		}

		public long Status() {
			return status;
		}

		public string Body() {
			return body;
		}

		public byte[] RawBody() {
			return rawBody;
		}

		public static Response From(UnityWebRequest www) {
			return new Response (www.responseCode, www.downloadHandler.text, www.downloadHandler.data);
		}
	}
}
