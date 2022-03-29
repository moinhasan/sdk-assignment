using System.Collections.Generic;
using UnityEngine.Networking;

namespace APIClient.HttpProcess
{	
	public class Request {
		private string url;
		private string method;
		private Dictionary<string, string> headers;
		private int timeout;

		public Request(string url) {
			this.method = "GET";
			this.url = url;
			this.timeout = 0;
			this.headers = new Dictionary<string, string> ();
		}

		public Request Url(string url) {
			this.url = url;
			return this;
		}

		public Request Method(string method) {
			this.method = method;
			return this;
		}

		public Request AddHeader(string name, string value) {
			this.headers.Add (name, value);
			return this;
		}

		public Request RemoveHeader(string name) {
			this.headers.Remove (name);
			return this;
		}

		public Request Timeout(int timeout) {
			this.timeout = timeout;
			return this;
		}

		public Request Get() {
			Method (UnityWebRequest.kHttpVerbGET);
			return this;
		}

		public string Url() {
			return url;
		}

		public string Method() {
			return method;
		}

		public Dictionary<string, string> Headers() {
			return headers;
		}

		public int Timeout() {
			return timeout;
		}
	}
}