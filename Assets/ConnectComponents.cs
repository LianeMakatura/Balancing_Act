using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConnectComponents : MonoBehaviour {
	// will eventually create a cylinder (hopefully extruded along a curve?), put both involved components and the connecting edge in the same
	// multibody pendant object, and update the center of mass (math and display)
	// will have to look at rigid connections between these things, and how to fix a point and simulate
	public GameObject connector; // cylinder to represent connector
	public Button myselfButton;
	private MobileMaster masterMobile;
	public float indicator_size = 1f;


	// Use this for initialization
	void Start () {
		myselfButton = GetComponent<Button>();
		myselfButton.onClick.AddListener(() => connect());

		masterMobile = GameObject.Find("Mobile_Master").GetComponent<MobileMaster> ();
	}

	void connect() {
		Debug.Log ("trying to connect");
		// instantiate the cylinder with one of the points as point of instantiation
		GameObject obj1 = ((GameObject)masterMobile.selected[0]);
		GameObject obj2 = ((GameObject)masterMobile.selected[1]);

		Vector3 obj1_pos = obj1.transform.position;
		Vector3 obj2_pos = obj2.transform.position;

		Vector3 pos = Vector3.Lerp(obj1_pos, obj2_pos, 0.5f);

		Quaternion rot = Quaternion.identity;
		GameObject newConnector = (GameObject) Instantiate (connector, pos, rot);
		//newConnector.transform.parent = obj1.transform; //parent CoM to the object its representing

		Vector3 scale_vec = new Vector3(indicator_size, indicator_size, indicator_size);
		newConnector.transform.localScale = scale_vec;

		// transform.LookAt to make y axis of cylinder face the other point
		newConnector.transform.LookAt(obj2.transform.TransformPoint(obj2_pos));
		newConnector.transform.Rotate (new Vector3 (1.0f, 0, 0), 90);

		//scale cylinder based on distance between the points
		Vector3 newScale = newConnector.transform.localScale;
//		newScale.y = Vector3.Distance(obj1.transform.TransformPoint(obj1_pos), obj2.transform.TransformPoint(obj2_pos))/2;
		newScale.y = Vector3.Distance(obj1_pos, obj2_pos)/2;
		newConnector.transform.localScale = newScale;


	}

	// Update is called once per frame
	void Update () {

	}

	// make sure we free up the listener
	void Destroy() {
		myselfButton.onClick.RemoveListener(() => connect());
	}
}
