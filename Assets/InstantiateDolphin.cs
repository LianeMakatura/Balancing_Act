using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InstantiateDolphin : MonoBehaviour {
	public GameObject pend;
	public Button myselfButton;
	private int numCube = 0;

	// Use this for initialization
	void Start () {
		myselfButton = GetComponent<Button>();
		myselfButton.onClick.AddListener(() => instantiateMyDolphin());
	}

	void instantiateMyDolphin() {
		Vector3 pos = new Vector3(0, 0, 0);
		Quaternion rot = Quaternion.identity;
		Instantiate (pend, pos, rot);
		pend.AddComponent<Pendant> ();

		pend.name = "Dolphin " + numCube++;

		pend.GetComponent<BoxCollider>().isTrigger = true;
	} 

	// Update is called once per frame
	void Update () {

	}

	// make sure we free up the listener
	void Destroy() {
		myselfButton.onClick.RemoveListener(() => instantiateMyDolphin());
	}
}
