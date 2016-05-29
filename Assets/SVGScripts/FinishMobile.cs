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
		MobileMaster masterMobile = GameObject.Find("Mobile_Master").GetComponent<MobileMaster> ();
		Debug.Log ("EYYYYYY");
		masterMobile.last_susp_pt.SendMessage ("invalidateSuspensionPoint");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
