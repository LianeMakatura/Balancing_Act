using UnityEditor;
using UnityEngine;

public class RigidBodyEditor : MonoBehaviour
{

	public float CoM_indicator_size = 1f;
	private Vector3 marker_scale_vec;
	public GameObject marker; 
	private Rigidbody game_object_rb;

//	public MeshCollider meshCollider;

	void Awake() {
		game_object_rb = gameObject.GetComponent<Rigidbody>();

		// create marker to indicate CoM
		marker = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		marker.transform.parent = gameObject.transform; //parent CoM to the object its representing

		marker_scale_vec = new Vector3(CoM_indicator_size, CoM_indicator_size, CoM_indicator_size);
		marker.transform.localScale = marker_scale_vec;

		Rigidbody marker_rb = marker.AddComponent<Rigidbody>(); // Add the rigidbody.
		marker_rb.mass = 0;
		marker_rb.isKinematic = true; // not controlled by physics
		marker_rb.useGravity = false;

		Material mat = marker.GetComponent<Renderer>().material; //change the sphere's material to be red
		mat.color = Color.red;

		marker.AddComponent<SuspensionPoint>(); // add suspension point functionality

		// add joint to the connector (added before the pendants
		FixedJoint joint = gameObject.AddComponent<FixedJoint>();
		joint.connectedBody = marker.GetComponent<Rigidbody>();

		// need to find the highest intersection point of the mesh that's above the com
		if (marker != null && game_object_rb != null) {
			marker.transform.position = gameObject.GetComponent<IPendant> ().getSuspensionPoint();
		}

	}

	// to remove the suspension point after the object it's attached to becomes immutable
	public void removeSuspensionPoint() {
		Destroy (marker);
		marker = null;
	}

	// so you can't click on the suspension point after the object's been attached
	public void invalidateSuspensionPoint() {
		Destroy (marker.GetComponent<SphereCollider>());
		Destroy (marker.GetComponent<SuspensionPoint> ());
		Material mat = marker.GetComponent<Renderer>().material; //change the sphere's material to be red
		mat.color = Color.black;
	}

	void Update()
	{

	}
}

