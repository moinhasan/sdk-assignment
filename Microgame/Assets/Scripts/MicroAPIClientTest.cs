using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using APIClient.Message;

public class MicroAPIClientTest : MonoBehaviour
{
	public Text userInfo;
	private DialogManager dialog;

    private void OnEnable()
    {
		this.GetComponent<Canvas>().enabled = false;
	}

    void Start()
    {
		dialog = DialogManager.Instance;
		StartCoroutine(new MessageGetUserData(this.CallbackMessage, 1).Send());
		dialog.ShowNetworkAccessActive(true);

	}

	public void ClosePanel()
	{
		this.gameObject.SetActive(false);
	}

	private bool CallbackMessage(Message message)
	{
		dialog.ShowNetworkAccessActive(false);
		MessageGetUserData msg = (MessageGetUserData)message;
		if (msg.IsSuccess)
		{
			UserData user = msg.UserData;
			userInfo.text = "ID: " + user.id + "     |     " + "Name: " + user.name + "     |     " + "Username: " + user.username + "\n\n" +
							"Email: " + user.email + ",  " + "Phone: " + user.phone + ",   " + "Website: " + user.website + "\n\n" +
							"Address:- \n" +
							"Street: " + user.address.street + ",  " + "Suite: " + user.address.suite + ",  " + "City: " + user.address.city + ",  " + "Zipcode: " + user.address.zipcode + "\n" +
							"GEO: [" + user.address.geo.lat + ", " + user.address.geo.lng + "]\n\n"+
                            "Company:- \n" +
                            "Name: " + user.company.name + ",  " + "catchPhrase: " + user.company.catchPhrase + ",\n" + "BS: " + user.company.bs;

			this.GetComponent<Canvas>().enabled = true;
		}
		else
		{
			dialog.CallSystemDialog("ERROR", msg.errorText, SystemDialog.AppearanceType.Default, null);
			userInfo.text = "error: " + msg.errorText;

		}
		return true;
	}
}
