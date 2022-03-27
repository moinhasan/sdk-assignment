using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

	public static void DeactivateChildren(GameObject go, bool active)
	{
		go.SetActive(active);
		foreach (Transform child in go.transform)
		{
			DeactivateChildren(child.gameObject, active);
		}
	}
}
