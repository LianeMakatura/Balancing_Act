using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConnectComponents : MonoBehaviour {
	// will eventually create a cylinder (hopefully extruded along a curve?), put both involved components and the connecting edge in the same
	// multibody pendant object, and update the center of mass (math and display)
	// will have to look at rigid connections between these things, and how to fix a point and simulate
	public GameObject connector; // cylinder to represent connector
	public Button myselfButton;

	// Use this for initialization
	void Start () {
		myselfButton = GetComponent<Button>();
		myselfButton.onClick.AddListener(() => instantiateMyCube());
	}

	void instantiateMyCube() {
		// instantiate the cylinder with one of the points as point of instantiation

		Vector3 pos = new Vector3(0, 0, 0);
		Quaternion rot = Quaternion.identity;
		Instantiate (connector, pos, rot);

		// transform.LookAt to mke cylinder face the other point
		//scale cylinder based on distance between the points

	}

	// Update is called once per frame
	void Update () {

	}

	// make sure we free up the listener
	void Destroy() {
		myselfButton.onClick.RemoveListener(() => instantiateMyCube());
	}
}
