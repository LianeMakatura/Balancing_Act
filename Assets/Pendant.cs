using UnityEngine;
using System.Collections;

public class Pendant : MonoBehaviour {
	public float mat_density = 1;
	public bool isConnector = false; // assume it's an object

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

		// find the mass of the object using the volume and material density
		MeshFilter meshF = gameObject.GetComponent<MeshFilter>();
		float volume = ComputeVolume(meshF);
		string msg = "The volume of the mesh is " + volume + " cube units.";
		Debug.Log(msg);

		float newMass = volume * mat_density;
		rb.mass = newMass;
		string msg2 = "The mass of the mesh is " + volume*mat_density + " cube units.";
		Debug.Log(msg2);

		gameObject.AddComponent<DragRigidBody> (); // makes cube draggable
		gameObject.AddComponent<RigidBodyEditor> (); // creates the center of mass marker


		// should I just remove the box/capsule colliders and use all mesh colliders? I probably should.
	}

	// find the center of mass of an arbitrary object
	public Vector3 computeCenterOfMass() {
		Collider col = gameObject.GetComponent<MeshCollider>(); // get the bounding box for an object
		if (col == null) {
			col = gameObject.GetComponent<BoxCollider>();
		}
		if (col == null) { // still null
			col = gameObject.GetComponent<CapsuleCollider>();
		}


		Vector3 bound_min = col.bounds.min;
		Vector3 bound_max = col.bounds.max;

		float sample_rate = 0.001;
		for (float x=bound_min.x; x <= bound_max.x; x+=sample_rate) {
			for (float y=bound_min.y; y <= bound_max.y; y+=sample_rate) {
				// cast a ray in the z direction
				// if it hit my object
					//add this coord to my running sum
					// update how many points have been included in tally

			}
		}
		// average all the positions by how many points were included (because we're assuming uniform density so they all have the same "mass" ie equal weighting)

		//update the center of mass of the object
	}

	public Vector3 findSuspensionPoint() {
		// calculate the intersection with the mesh that is directly above the center of mass in the direction of gravity
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
		volume *= meshF.gameObject.transform.localScale.x * meshF.gameObject.transform.localScale.y * meshF.gameObject.transform.localScale.z;
		return Mathf.Abs(volume);
	}


	// also need to set the center of mass for objects


	public void Update() {

	}

}
