using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiBodyPendant : MonoBehaviour, IPendant {
	public List<GameObject> pendants;
	private Vector3 centerOfMass;
	private Vector3 suspensionPoint;
	private GameObject newConnector;
	public float sample_rate = 0.01f;

	// Use this for initialization
	void Awake () {
		pendants = new List<GameObject>();
		Rigidbody rb = gameObject.AddComponent<Rigidbody> (); // make this object have mass, CoM, etc.
		rb.useGravity = false;		// don't want these unless simulating
		rb.isKinematic = true;
//		rb.constraints = RigidbodyConstraints.FreezePositionZ; // might also need to freeze rotation later, not sure.

		gameObject.AddComponent<RigidBodyEditor> ();

		DragRigidBody drb = gameObject.AddComponent<DragRigidBody>(); // draggable group
		drb.isDraggable = false;
	}

	public void addPendant(GameObject shape) {
		pendants.Add (shape);
		//shape.transform.parent = gameObject.transform;

		// add joint to the connector (added before the pendants
		//FixedJoint joint = shape.AddComponent<FixedJoint>();
		//joint.connectedBody = newConnector.GetComponent<Rigidbody>();

		// update the fixed joint
		shape.GetComponent<FixedJoint>().connectedBody = newConnector.GetComponent<Rigidbody>();
	}

	public void addConnector(GameObject shape) {
		pendants.Add (shape);
		//shape.transform.parent = gameObject.transform;
		newConnector = shape; 			// keep track of the new connector so we know what to join them to
	}

	// to be called via message after all objects have been added in ConnectComponents
	public void freezeGroup () {
		computeCenterOfMass(); // compute the center of mass
		findSuspensionPoint();
	
		gameObject.GetComponent<Rigidbody>().centerOfMass = centerOfMass; // change the location of the suspension point to be the center of mass

		//update the fixed joint
		//newConnector.GetComponent<FixedJoint>().connectedBody = gameObject.GetComponent<RigidBodyEditor>().marker.GetComponent<Rigidbody>();
	}

	Vector3 computeCenterOfMass() {
		Vector3 CoM_loc = Vector3.zero;
		float mass_sum = 0f;

		foreach (GameObject shape in pendants) {
			CoM_loc += shape.GetComponent<Rigidbody>().worldCenterOfMass * shape.GetComponent<Rigidbody>().mass;
			mass_sum += shape.GetComponent<Rigidbody>().mass;
		}
		CoM_loc /= mass_sum;
		centerOfMass = CoM_loc;
		Debug.Log ("Center of mass for the multibody pendant is: " + centerOfMass);
		return CoM_loc;
	}

	public void findSuspensionPoint() {
		GameObject suspPt = gameObject.GetComponent<RigidBodyEditor>().marker;

		List<Collider> col_list = new List<Collider> ();
		Vector3 bound_max = new Vector3(Mathf.NegativeInfinity, Mathf.NegativeInfinity, Mathf.NegativeInfinity);
		Vector3 bound_min = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);

		// calculate the intersection with the mesh that is directly above the center of mass in the direction of gravity
		foreach (GameObject shape in pendants) {
			col_list.Add(shape.GetComponent<MeshCollider> ()); // get the bounding box for an object

			// find the highest point we have to check 
			Renderer rend = shape.GetComponent<Renderer> ();
			Vector3 loc_bmin = rend.bounds.min;
			Vector3 loc_bmax = rend.bounds.max;

			if (loc_bmin.y < bound_min.y) {
				bound_min = loc_bmin;
			}
			if (loc_bmax.y > bound_max.y) {
				bound_max = loc_bmax;
			}
		}

		suspensionPoint = new Vector3(centerOfMass.x, centerOfMass.y, bound_min.z -suspPt.transform.localScale.y); // puts it in front of our mesh

		for (float y=centerOfMass.y; y <= bound_max.y; y+=sample_rate) { // check along the upward direction
			// cast a ray in the z direction
			Vector3 start = new Vector3(centerOfMass.x, y, Camera.main.transform.position.z);
			Ray ray = new Ray(start, Camera.main.transform.forward);

			RaycastHit hit;
			// if it hit my object
			foreach (Collider col in col_list) {
				if (col.Raycast (ray, out hit, 1000.0f)) {
					suspensionPoint.y = y;
				}
			}
		}

		// we've got the last (highest) intersection
		// take off 2 radii of the suspension point (so top of hole is 1 radius from top of object)
		suspensionPoint.y -= 2 * suspPt.transform.localScale.y;
		suspPt.transform.position = suspensionPoint;
	}

	public Vector3 getCenterOfMass() {
		Debug.Log ("MBP get CoM has been called!");
		return centerOfMass;
	}

	public Vector3 getSuspensionPoint() {
		Debug.Log ("MBP get suspension point has been called!");
		return suspensionPoint;
	}


	// Update is called once per frame
	void Update () {
	
	}
}
