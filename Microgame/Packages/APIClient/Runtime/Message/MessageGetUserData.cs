using UnityEngine;

namespace APIClient.Message
{
    public class MessageGetUserData : Message
    {
        public UserData UserData { get; private set; }
        public int UserId { get; private set; }
		
		public MessageGetUserData(CallbackMessage callback, int userId)
		: base(APICommand.GetUsersData, callback)
		{
			this.UserId = userId;
		}

		protected override bool SendContent(ref string url)
		{
			url = GetUrl(this.Command + this.UserId);
			Debug.Log("GetUserData url: " + url);
			return true;
		}

		protected override bool CallbackServerContent(string data)
		{
			UserData user = JsonUtility.FromJson<UserData>(data);
			this.UserData = user;
			Debug.Log(user.address.geo.lat);
			return true;
		}

	}
}