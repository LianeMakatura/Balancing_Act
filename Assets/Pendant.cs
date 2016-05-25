using UnityEngine;
using System.Collections;

public class Pendant : MonoBehaviour, IPendant {
	public float mat_density = 1;
	public bool isConnector = false; // assume it's an object
	public float sample_rate = 0.01f;
	public Vector3 suspensionPoint;
	public Vector3 centerOfMass;
	public float voxelMass;

	//use this for initialization
	void Awake ()
	{
		Rigidbody rb;
		if (gameObject.GetComponent<Rigidbody> () == null) {
			rb = gameObject.AddComponent<Rigidbody> ();
		} else {
			rb = gameObject.GetComponent<Rigidbody> ();
		}

		rb.useGravity = false;		// don't want these unless simulating
		rb.isKinematic = true;
		rb.constraints = RigidbodyConstraints.FreezePositionZ; // might also need to freeze rotation later, not sure.

		// find the mass of the object using the volume and material density -- this seems to be not totally functional :(
		// scaling seems off
//		MeshFilter meshF = gameObject.GetComponent<MeshFilter>();
//		float volume = ComputeVolume(meshF);
//		string msg = "The volume of the mesh is " + volume + " cube units.";
//		Debug.Log(msg);
//
//		float newMass = volume * mat_density;
//		rb.mass = newMass;
//		string msg2 = "The mass of the mesh is " + volume*mat_density + " units.";
//		Debug.Log(msg2);
		rb.mass = voxelMass;

		gameObject.AddComponent<DragRigidBody> (); // makes cube draggable
		gameObject.AddComponent<RigidBodyEditor> (); // creates the center of mass marker


	}

	void Start() {
		computeCenterOfMass ();
		Debug.Log ("center of mass is " + centerOfMass);
		findSuspensionPoint ();
		Debug.Log ("suspension point is " + suspensionPoint);
	}


	// find the center of mass of an arbitrary object
	public void computeCenterOfMass() {
		Vector3 point_sum = new Vector3(0f,0f,0f);
		int num_points = 0;

		Renderer rend = gameObject.GetComponent<Renderer> ();
		Collider col = gameObject.GetComponent<MeshCollider>(); // get the bounding box for an object
		if (col == null) {
			Debug.Log("This object has no mesh collider!");
		}

		Vector3 bound_min = rend.bounds.min;
		Vector3 bound_max = rend.bounds.max;
		Debug.Log ("bound min: " + bound_min + ", Bound max: " +  bound_max);

		for (float x=bound_min.x; x <= bound_max.x; x+=sample_rate) {
			for (float y=bound_min.y; y <= bound_max.y; y+=sample_rate) {
				// cast a ray in the z direction
				Vector3 start = new Vector3(x, y, Camera.main.transform.position.z);
				Ray ray = new Ray(start, Camera.main.transform.forward);

				RaycastHit hit;
				// if it hit my object
				if (col.Raycast(ray, out hit, 100.0f)) {
					point_sum += new Vector3(x, y, 0.0f);	//add this coord to my running sum
					num_points++; // update how many points have been included in tally
					Debug.DrawRay (start, Camera.main.transform.forward*10, Color.red, 20.0f);
				}

			}
		}
		// average all the positions by how many points were included (because we're assuming uniform density so they all have the same "mass" ie equal weighting)
		Debug.LogWarning("numpoints is " + num_points);
		Vector3 com = point_sum / num_points;

		//update the center of mass of the object (will no longer update automatically but that's not necessary anyway)
		centerOfMass = com;
		voxelMass = num_points;
		gameObject.GetComponent<Rigidbody>().centerOfMass = com;
	}

	public void findSuspensionPoint() {
		GameObject suspPt = gameObject.GetComponent<RigidBodyEditor>().marker;
		if (suspPt == null) {
			Debug.LogWarning ("there's no susp point!");
		}

		Renderer rend = gameObject.GetComponent<Renderer> ();
		// calculate the intersection with the mesh that is directly above the center of mass in the direction of gravity
		Collider col = gameObject.GetComponent<MeshCollider>(); // get the bounding box for an object
		if (col == null) {
			Debug.Log("This object has no mesh collider!");
		}

		Vector3 bound_min = rend.bounds.min;
		Vector3 bound_max = rend.bounds.max;

		suspensionPoint = new Vector3(centerOfMass.x, centerOfMass.y, bound_min.z -suspPt.transform.localScale.y); // puts it in front of our mesh

		for (float y=centerOfMass.y; y <= bound_max.y; y+=sample_rate) { // check along the upward direction
			// cast a ray in the z direction
			Vector3 start = new Vector3(centerOfMass.x, y, Camera.main.transform.position.z);
			Ray ray = new Ray(start, Camera.main.transform.forward);

			RaycastHit hit;
			// if it hit my object
			if (col.Raycast(ray, out hit, 1000.0f)) {
				suspensionPoint.y = y;
			}
		}

		// we've got the last (highest) intersection
		// take off 2 radii of the suspension point (so top of hole is 1 radius from top of object)
		suspensionPoint.y -= 2 * suspPt.transform.localScale.y;
		suspPt.transform.position = suspensionPoint;
	}

	public Vector3 getCenterOfMass() {
		Debug.Log ("Pendant get CoM has been called!");
		return centerOfMass;
	}

	public Vector3 getSuspensionPoint() {
		Debug.Log ("Pendant get suspension point has been called!");
		return suspensionPoint;
	}



	public float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3) //http://gamedev.stackexchange.com/questions/106318/getting-the-volume-of-an-uneven-mesh
	{
		float v321 = p3.x * p2.y * p1.z;
		float v231 = p2.x * p3.y * p1.z;
		float v312 = p3.x * p1.y * p2.z;
		float v132 = p1.x * p3.y * p2.z;
		float v213 = p2.x * p1.y * p3.z;
		float v123 = p1.x * p2.y * p3.z;
		return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
	}

	public float ComputeVolume(MeshFilter meshF)
	{
		Mesh mesh = meshF.sharedMesh;
		float volume = 0;
		Vector3[] vertices = mesh.vertices;
		int[] triangles = mesh.triangles;
		for (int i = 0; i < mesh.triangles.Length; i += 3)
		{
			Vector3 p1 = vertices[triangles[i + 0]];
			Vector3 p2 = vertices[triangles[i + 1]];
			Vector3 p3 = vertices[triangles[i + 2]];
			volume += SignedVolumeOfTriangle(p1, p2, p3);
		}
		volume *= gameObject.transform.localScale.x * gameObject.transform.localScale.y * gameObject.transform.localScale.z;
		return Mathf.Abs(volume);
	}


	public void Update() {

	}

}
