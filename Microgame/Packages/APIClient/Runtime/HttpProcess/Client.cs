using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace APIClient.HttpProcess
{
	public class Client {
		private Response response;
		private string error;
		private UnityWebRequest www;

		public Client() {
			error = null;
			response = null;
			www = null;
		}

		public IEnumerator Send(Request request) {
			using (www = new UnityWebRequest (request.Url (), request.Method ())) {

				www.timeout = request.Timeout ();

				Dictionary<string, string> headers = request.Headers ();
				if (headers != null) {
					foreach (KeyValuePair<string, string> header in headers) {
						www.SetRequestHeader (header.Key, header.Value);
					}
				}

				www.downloadHandler = new DownloadHandlerBuffer ();

				yield return www.SendWebRequest ();

				if (www.result == UnityWebRequest.Result.ConnectionError
					|| www.result == UnityWebRequest.Result.ProtocolError
					|| www.result == UnityWebRequest.Result.DataProcessingError) {
					error = www.error;
				} else {
					response = HttpProcess.Response.From (www);
				}
			}
		}

		public void Abort() {
			www.Abort ();
		}

		public bool IsSuccessful() {
			return error == null;
		}

		public string Error() {
			return error;
		}

		public HttpProcess.Response Response() {
			return response;
		}
	}
}
