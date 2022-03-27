using UnityEngine;
using System.Collections;
using Exception = System.Exception;

public class DialogManager : MonoBehaviour
{
	private static readonly string MANAGER_PREFAB = "UI/DialogManager";

	public GameObject prefabMarkNetworkAccess = null;
	public GameObject prefabDialog = null;

	private GameObject objectMarkNetworkAccess = null;
	private GameObject objectDialog = null;
	private Dialog.Callback callbackDialog = null;

	private static DialogManager instance = null;
	public static DialogManager Instance
	{
		get
		{
			return DialogManager.GetInstance();
		}
	}

	public static DialogManager GetInstance()
	{
		if (instance == null)
		{
			Object prefab = Resources.Load(MANAGER_PREFAB);
			Instantiate(prefab);
			Debug.Log("DialogManager instance created: " + instance );
		}
		return instance;
	}

	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(gameObject);
		}

		objectMarkNetworkAccess = Instantiate(prefabMarkNetworkAccess) as GameObject;
		objectMarkNetworkAccess.transform.SetParent(this.transform);
		objectMarkNetworkAccess.SetActive(false);
	}

	#region Create and show dialog
	//----------------------------------
	public SystemDialog CallSystemDialog(string messageTitle, string message, SystemDialog.AppearanceType appearanceType, Dialog.Callback callback, float _wait = 0.0f)
	{
		if (IsExistDialog())
		{
			Debug.Log("Dialog exists!");
			return null;
		}

		SystemDialog dialog = CreateSystemDialog(messageTitle, message, appearanceType, _wait);
		StartDialog(dialog, callback);
		return dialog;
	}

	//	Create System Dialog
	private SystemDialog CreateSystemDialog(string messageTitle, string message, SystemDialog.AppearanceType appearanceType, float _wait)
	{
		SystemDialog.ButtonType buttonType;
		if (appearanceType == SystemDialog.AppearanceType.Confirm)
		{
			buttonType = SystemDialog.ButtonType.YesNo;
		}	
		else
		{
			buttonType = SystemDialog.ButtonType.Ok;
		}

		// Instantiate Dialog panel
		GameObject obj = Instantiate(prefabDialog) as GameObject;
		// Assign to Dialog Manager parent object
		obj.transform.SetParent(this.transform, false);

		// Set message values
		SystemDialog dialog = obj.GetComponent<SystemDialog>();
		dialog.SetAppearanceType(appearanceType);
		dialog.SetMessageTitle(messageTitle);
		dialog.SetMessage(message);
		dialog.SetButtonType(buttonType);
		dialog.SetWaitTime(_wait);

		return dialog;
	}

	private void StartDialog(Dialog dialog, Dialog.Callback callback)
	{
		objectDialog = dialog.gameObject;
		callbackDialog = callback;
		dialog.SetCallback(this.CallbackDialog);
	}

	private void CallbackDialog(Dialog dialog, int result)
	{
		objectDialog = null;

		if (callbackDialog != null)
		{
			Dialog.Callback callback = callbackDialog;
			callbackDialog = null;
			callback(dialog, result);
		}
	}
	#endregion

	#region Manage network access dialog
	//----------------------------------
	public void SetObjectMarkNetworkAccessActive(bool active)
	{
		if (objectMarkNetworkAccess != null)
		{
			objectMarkNetworkAccess.SetActive(active);
		}
	}
	#endregion

	#region Check if any dialog is open
	//---------------------------------
	public bool IsExistDialog()
	{
		return (objectDialog != null || objectMarkNetworkAccess.activeInHierarchy);
	}

	//private bool IsExistDialog()
	//{
	//	return (objectDialog != null);
	//}

	#endregion

	public void OnDestroy()
	{
		if (instance == this)
		{
			Destroy(objectMarkNetworkAccess);
			if (objectDialog != null)
			{
				Destroy(objectDialog);
			}			
			instance = null;
		}
	}
}