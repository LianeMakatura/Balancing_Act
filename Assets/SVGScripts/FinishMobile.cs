using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FinishMobile : MonoBehaviour {
	public Button myselfButton;

	// Use this for initialization
	void Start () {
		myselfButton = GetComponent<Button>();
		myselfButton.onClick.AddListener (() => Finish());
	}


	void Finish() {
		// mute creation buttons
		Canvas canvas = myselfButton.transform.parent.GetComponentInChildren<Canvas>();
		Button[] buttons = canvas.GetComponentsInChildren<Button> ();
		foreach (Button button in buttons) {
			button.interactable = false;
		}

		// send msg to mobile master
		MobileMaster masterMobile = GameObject.Find("Mobile_Master").GetComponent<MobileMaster> ();
		masterMobile.last_susp_pt.SendMessage ("invalidateSuspensionPoint");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
