using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiBodyPendant : MonoBehaviour {
	public List<GameObject> pendants = new List<GameObject>();
	private Vector3 CenterOfMass;

	// Use this for initialization
	void Start () {
//		pendants = new List<GameObject>();
		gameObject.AddComponent<RigidBodyEditor> (); // make this object have mass, CoM, etc.
		gameObject.AddComponent<DragRigidBody>(); // draggable group
	}

	public void addPendant(GameObject shape) {
		pendants.Add (shape);
		Debug.Log (shape.transform.parent.ToString());
		shape.transform.parent = gameObject.transform;
	}

	public void addConnector(GameObject shape) {
		pendants.Add (shape);
		shape.transform.parent = gameObject.transform;
	}

	Vector3 computeCenterOfMass() {
		Vector3 CoM_loc = Vector3.zero;
		float mass_sum = 0f;

		foreach (GameObject shape in pendants) {
			CoM_loc += shape.GetComponent<Rigidbody>().worldCenterOfMass * shape.GetComponent<Rigidbody>().mass;
			mass_sum += shape.GetComponent<Rigidbody>().mass;
		}

		CoM_loc /= mass_sum;
		CenterOfMass = CoM_loc;
		return CoM_loc;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
