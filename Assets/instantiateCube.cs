using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class instantiateCube : MonoBehaviour {
	public GameObject pend;
	public Button myselfButton;
	private int numCube = 0;

	// Use this for initialization
	void Start () {
		myselfButton = GetComponent<Button>();
		myselfButton.onClick.AddListener(() => instantiateMyCube());
	}

	void instantiateMyCube() {
		Vector3 pos = new Vector3(0, 0, 0);
		Quaternion rot = Quaternion.identity;
		pend = GameObject.CreatePrimitive (PrimitiveType.Cube);

		pend.AddComponent<Pendant> ();

		pend.transform.position = pos;
		pend.transform.rotation = rot;
		pend.name = "Cube" + numCube++;

		pend.GetComponent<BoxCollider>().isTrigger = true;
	} 

	// Update is called once per frame
	void Update () {
	
	}

	// make sure we free up the listener
	void Destroy() {
		myselfButton.onClick.RemoveListener(() => instantiateMyCube());
	}
}
