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
	private GameObject obj1, obj2;
	public float indicator_size = 1f;


	// Use this for initialization
	void Start () {
		myselfButton = GetComponent<Button>();
		myselfButton.onClick.AddListener(() => connect());

		masterMobile = GameObject.Find("Mobile_Master").GetComponent<MobileMaster> ();
	}

	void connect() {
		if (masterMobile.selected.Count == 2) {
			obj1 = ((GameObject)masterMobile.selected [0]);
			obj2 = ((GameObject)masterMobile.selected [1]);

			makeConnector ();
			makeImmutable ();
			createMultiBodyPendant ();
		} 
		else {
			Debug.Log ("Must have 2 selected");
		}
	}

	// connect the selected objects
	void makeConnector() {
		Debug.Log ("trying to connect");
		// instantiate the cylinder with one of the points as point of instantiation
		Vector3 obj1_pos = obj1.transform.position;
		Vector3 obj2_pos = obj2.transform.position;

		Vector3 pos = Vector3.Lerp(obj1_pos, obj2_pos, 0.5f); // put origin of object in between the two pieces

		Quaternion rot = Quaternion.identity;
		GameObject newConnector = (GameObject) Instantiate (connector, pos, rot);

		Vector3 scale_vec = new Vector3(indicator_size, indicator_size, indicator_size);
		newConnector.transform.localScale = scale_vec;

		// transform.LookAt to make y axis of cylinder face the other point
		newConnector.transform.LookAt(obj2.transform.TransformPoint(obj2_pos));
		newConnector.transform.Rotate (new Vector3 (1.0f, 0, 0), 90);

		//scale cylinder based on distance between the points
		Vector3 newScale = newConnector.transform.localScale;
		newScale.y = Vector3.Distance(obj1_pos, obj2_pos)/2;
		newConnector.transform.localScale = newScale;
	}

	void makeImmutable() {
		GameObject cube1 = obj1.transform.parent.gameObject;
		GameObject cube2 = obj2.transform.parent.gameObject;

		masterMobile.selected.Remove(obj1);			// clear out the selected list
		masterMobile.selected.Remove(obj2);

		Destroy (cube1.GetComponent<DragRigidBody>());		// can no longer drag the cubes
		Destroy (cube2.GetComponent<DragRigidBody>());

		Destroy (obj1);								// get rid of the suspension points
		Destroy (obj2);
	}


	void createMultiBodyPendant() {
	}

	// Update is called once per frame
	void Update () {

	}

	// make sure we free up the listener
	void Destroy() {
		myselfButton.onClick.RemoveListener(() => connect());
	}
}
