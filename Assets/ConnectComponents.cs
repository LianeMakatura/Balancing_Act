using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConnectComponents : MonoBehaviour {
	// will eventually create a cylinder (hopefully extruded along a curve?), put both involved components and the connecting edge in the same
	// multibody pendant object, and update the center of mass (math and display)
	// will have to look at rigid connections between these things, and how to fix a point and simulate
	public Button myselfButton;
	private MobileMaster masterMobile;
	private GameObject obj1, obj2, newConnector;
	public float indicator_size = 0.1f;


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
			createMultiBodyPendant ();
			Debug.Log ("the value of newConnector is " + newConnector.ToString());
			makeImmutable_connect ();
		} 
		else {
			Debug.Log ("Must have 2 selected");
		}
	}

	// connect the selected objects
	void makeConnector() {
		// instantiate the cylinder with one of the points as point of instantiation
		Vector3 obj1_pos = obj1.transform.position;
		Vector3 obj2_pos = obj2.transform.position;

		Vector3 pos = Vector3.Lerp (obj1_pos, obj2_pos, 0.5f); // put origin of object in between the two pieces

		newConnector = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
		newConnector.transform.position = pos;

		newConnector.GetComponent<CapsuleCollider>().isTrigger = true;


		Vector3 scale_vec = new Vector3 (indicator_size, indicator_size, indicator_size);
		newConnector.transform.localScale = scale_vec;
		Debug.Log ("scale of piece is " + scale_vec.ToString());

		// transform.LookAt to make y axis of cylinder face the other point
		newConnector.transform.LookAt (obj2.transform.TransformPoint (obj2_pos));
		newConnector.transform.Rotate (new Vector3 (1.0f, 0, 0), 90);

		//scale cylinder based on distance between the points
		Vector3 newScale = newConnector.transform.localScale;
		newScale.y = Vector3.Distance (obj1_pos, obj2_pos) / 2;
		Debug.Log ("scale of piece is " + newScale.ToString());
		newConnector.transform.localScale = newScale;

		Pendant p = newConnector.AddComponent<Pendant> (); // we need material density, volume; adds rigid body and draggable
		p.isConnector = true;
		newConnector.GetComponent<DragRigidBody> ().isDraggable = true;
	}


	void createMultiBodyPendant() {
		GameObject pendant_group = new GameObject ();
		MultiBodyPendant mbp = pendant_group.AddComponent<MultiBodyPendant> ();

		mbp.addConnector (newConnector);
		mbp.addPendant (obj1.transform.parent.gameObject);
		mbp.addPendant (obj2.transform.parent.gameObject);

		mbp.SendMessage ("freezeGroup");
	}

	void makeImmutable_connect() {
		GameObject cube1 = obj1.transform.parent.gameObject;
		GameObject cube2 = obj2.transform.parent.gameObject;

		masterMobile.selected.Remove(obj1);			// clear out the selected list
		masterMobile.selected.Remove(obj2);	

		cube1.SendMessage ("makeImmutable");
		cube2.SendMessage ("makeImmutable");
		newConnector.SendMessage ("makeImmutable");

		cube1.SendMessage ("removeSuspensionPoint");
		cube2.SendMessage ("removeSuspensionPoint");
		newConnector.SendMessage ("removeSuspensionPoint");
	}

	// Update is called once per frame
	void Update () {

	}

	// make sure we free up the listener
	void Destroy() {
		myselfButton.onClick.RemoveListener(() => connect());
	}
}
