using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiBodyPendant : MonoBehaviour, IPendant {
	public List<GameObject> pendants;
	public List<GameObject> all_pendants; // list of all single Pendants in this hierarchy, for colliders; setting the suspension point

	private GameObject newConnector;
	public float sample_rate = 0.01f;

	public Vector3 centerOfMass;
	public Vector3 suspensionPoint;
	public float voxelMass;
	public Vector3 minBound, maxBound;


	// Use this for initialization
	void Awake () {
		pendants = new List<GameObject>();
		all_pendants = new List<GameObject> ();
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

		MultiBodyPendant mbp = shape.GetComponent<MultiBodyPendant> ();
		if (mbp == null) { // it's a singleton
			all_pendants.Add (shape);
		} 
		else {		 // it's already a MBP
			foreach (GameObject p in mbp.all_pendants) {
				all_pendants.Add(p);
			}
		}
		//shape.transform.parent = gameObject.transform;

		// add joint to the connector (added before the pendants
		//FixedJoint joint = shape.AddComponent<FixedJoint>();
		//joint.connectedBody = newConnector.GetComponent<Rigidbody>();

		// update the fixed joint
		//shape.GetComponent<FixedJoint>().connectedBody = newConnector.GetComponent<Rigidbody>();
	}

	public void addConnector(GameObject shape) {
		pendants.Add (shape);
		all_pendants.Add (shape); // we know it's a singleton since it's a connector
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
			IPendant p = shape.GetComponent<IPendant> ();

			//might have to convert center of mass to world coords but I think the groups are already from 000
			Vector3 pt = p.getCenterOfMass();
			Debug.Log ("the local point for object " + shape + "is: " + pt);

			Vector3 transpt = shape.transform.TransformPoint(p.getCenterOfMass());
			Debug.Log ("the world point for object " + shape + "is: " + transpt);

			if (shape.GetComponent<Pendant> () != null && shape.GetComponent<Pendant> ().isConnector) {
				transpt = pt; // don't use the transformed ones
			}

			CoM_loc += transpt * p.getVoxelMass();
			mass_sum += p.getVoxelMass();
		}
		voxelMass = mass_sum;

		CoM_loc /= mass_sum;
		centerOfMass = CoM_loc;
		Debug.Log ("Center of mass for the multibody pendant is: " + centerOfMass);
		return CoM_loc;
	}

	public void findSuspensionPoint() {
		GameObject suspPt = gameObject.GetComponent<RigidBodyEditor>().marker;

		List<Collider> col_list = new List<Collider> ();
		maxBound = new Vector3(Mathf.NegativeInfinity, Mathf.NegativeInfinity, Mathf.NegativeInfinity);
		minBound = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);

		// calculate the intersection with the mesh that is directly above the center of mass in the direction of gravity
		foreach (GameObject shape in all_pendants) {
			col_list.Add(shape.GetComponent<MeshCollider> ()); // get the bounding box for an object

			// find the highest point we have to check 
			IPendant pend = shape.GetComponent<IPendant> ();
			Vector3 loc_bmin = pend.getMinBound();
			Vector3 loc_bmax = pend.getMaxBound();

			if (loc_bmin.y < minBound.y) {// conceptually verify that we only care about y bound
				minBound = loc_bmin;
			}
			if (loc_bmax.y > maxBound.y) {
				maxBound = loc_bmax;
			}
		}

//		suspensionPoint = new Vector3(centerOfMass.x, centerOfMass.y, minBound.z -suspPt.transform.localScale.y); // puts it in front of our mesh
		suspensionPoint = new Vector3(centerOfMass.x, centerOfMass.y, 0f); // puts it in front of our mesh

		for (float y=centerOfMass.y; y <= maxBound.y; y+=sample_rate) { // check along the upward direction
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
		suspensionPoint.y += suspPt.GetComponent<SphereCollider>().radius * suspPt.transform.localScale.y;
		suspPt.transform.position = suspensionPoint;
	}

	// Update is called once per frame
	void Update () {
	
	}

	// ALL THE INTERFACE GETTERS
	public Vector3 getCenterOfMass() {
		return centerOfMass;
	}

	public Vector3 getSuspensionPoint() {
		return suspensionPoint;
	}

	public float getVoxelMass() {
		return voxelMass;
	}

	public Vector3 getMinBound() {
		return minBound;
	}
	public Vector3 getMaxBound() {
		return maxBound;
	}
}
