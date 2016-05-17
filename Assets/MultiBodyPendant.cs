using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiBodyPendant : MonoBehaviour {
	public List<GameObject> pendants;
	private Vector3 CenterOfMass;
	private GameObject newConnector;

	// Use this for initialization
	void Awake () {
		pendants = new List<GameObject>();
		Rigidbody rb = gameObject.AddComponent<Rigidbody> (); // make this object have mass, CoM, etc.
		rb.useGravity = false;		// don't want these unless simulating
		rb.isKinematic = true;
		rb.constraints = RigidbodyConstraints.FreezePositionZ; // might also need to freeze rotation later, not sure.

		gameObject.AddComponent<RigidBodyEditor> ();
		DragRigidBody drb = gameObject.AddComponent<DragRigidBody>(); // draggable group
		//drb.isDraggable = false;
	}

	public void addPendant(GameObject shape) {
		pendants.Add (shape);
		shape.transform.parent = gameObject.transform;

		if (shape.GetComponent<Pendant> ().isConnector) {
			newConnector = shape; 			// keep track of the new connector so we know what to join them to
		}
	}

	private void joinToMBP(GameObject shape) {
		FixedJoint joint = shape.AddComponent<FixedJoint>();
		joint.connectedBody = newConnector.GetComponent<Rigidbody>();
//		shape.GetComponent<Rigidbody> ().isKinematic = false;
	}

	// to be called via message after all objects have been added in ConnectComponents
	public void freezeGroup () {
		// add joints to constrain the group
		foreach (GameObject shape in pendants) {
			joinToMBP (shape);
		}

		// compute the center of mass
		CenterOfMass = computeCenterOfMass();

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
		return CoM_loc;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
