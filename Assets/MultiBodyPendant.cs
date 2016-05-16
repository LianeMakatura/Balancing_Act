using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiBodyPendant : MonoBehaviour {
	public List<GameObject> pendants;
	private Vector3 CenterOfMass;

	// Use this for initialization
	void Awake () {
		pendants = new List<GameObject>();
		Rigidbody rb = gameObject.AddComponent<Rigidbody> (); // make this object have mass, CoM, etc.
		rb.useGravity = false;		// don't want these unless simulating
		rb.isKinematic = true;
		rb.constraints = RigidbodyConstraints.FreezePositionZ; // might also need to freeze rotation later, not sure.

		gameObject.AddComponent<RigidBodyEditor> ();
		gameObject.AddComponent<DragRigidBody>(); // draggable group
	}

	public void addPendant(GameObject shape) {
		pendants.Add (shape);
		shape.transform.parent = gameObject.transform;
	}

	// to be called via message after all objects have been added in ConnectComponents
	public void freezeGroup () {
		// add joints to constrain the group
		// compute the center of mass

		// change the location of the suspension point to be the center of mass
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
