using UnityEngine;
using System.Collections;
using System;
using APIClient.HttpProcess;

namespace APIClient.Message
{
	public abstract class Message
	{
		//CallBack to handle the result
		public delegate bool CallbackMessage(Message message);
		private CallbackMessage callbackMessage = null;

		//Request URL
		private string url = null;

		//Identification command
		public string Command { get; private set; }
		//Did it end?
		public bool IsDone { get; private set; }
		//Succeeded?
		public bool IsSuccess { get; private set; }
		//Results
		public string ReceivedData { get; private set; }
		public string statusText { get; private set; }
		public string errorText { get; private set; }

		//constructor, having resource command and callback 
		protected Message( string command, CallbackMessage callbackMessage )
		{ 
			this.Command = command;
			this.callbackMessage = callbackMessage;
			this.IsDone = false;
			this.IsSuccess = false;
			this.url = null;
		}

		//Send contents
		protected abstract bool SendContent(ref string url);

		//Individual correspondence of receiving transmission results
		protected abstract bool CallbackServerContent(string data);

		//Send execution
		public IEnumerator Send()
		{
			if (!SendContent(ref this.url))
			{
				Debug.LogError("Set Request Content!");
				yield return null;
			}

			//return StartCoroutine(Get(this.url, 1));
			yield return Get(this.url);
		}

		IEnumerator Get(string baseUrl)
		{
			Request request = new Request(baseUrl);

			Client http = new Client();
			yield return http.Send(request);
			ProcessResult(http);
		}

		void ProcessResult(Client http)
		{
			if (http.IsSuccessful())
			{
				Response resp = http.Response();
				this.ReceivedData = resp.Body();
				this.IsSuccess = false;
				try
				{
					//Process the content received from server
					this.IsSuccess = CallbackServerContent(this.ReceivedData);
                }
				catch (Exception e)
				{
					if (Debug.isDebugBuild) Debug.LogWarning("Message: response process failed e: " + e);
				}

				// If fails to process tha data
				if (!this.IsSuccess)
				{
					this.errorText = "Failed to process server response";
				}
				this.statusText = resp.Status().ToString();
				this.IsDone = true;
				Debug.Log("Status: " + resp.Status().ToString());

				// Transfer to recipients.
				if (callbackMessage != null)
				{
					callbackMessage(this);
				}
			}
			else
			{
				this.errorText = http.Error();
				this.IsDone = true;
				this.IsSuccess = false;
				Debug.Log("error: " + http.Error());

				//if (RetryCount < maxRetry)
				//{
				//	Retry();
				//	return;
				//}

				// Transfer to recipients.
				if (callbackMessage != null)
				{
					callbackMessage(this);
				}
			}
		}

		//	Get the URL for the command
		public string GetUrl(String command)
		{
			return NetworkDefine.API_BASE_PATH + command;
		}

	}
}