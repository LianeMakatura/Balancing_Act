using UnityEditor;
using UnityEngine;

public class RigidBodyEditor : MonoBehaviour
{

	public float CoM_indicator_size = 1f;
	private Vector3 marker_scale_vec;
	private GameObject marker; 

	void Start() {
		// create marker to indicate CoM
		marker = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		marker.transform.parent = gameObject.transform; //parent CoM to the object its representing

		marker_scale_vec = new Vector3(CoM_indicator_size, CoM_indicator_size, CoM_indicator_size);
		marker.transform.localScale = marker_scale_vec;

		Rigidbody marker_rb = marker.AddComponent<Rigidbody>(); // Add the rigidbody.
		marker_rb.mass = 0;
		marker_rb.isKinematic = true; // not controlled by physics

		Material mat = marker.GetComponent<Renderer>().material; //change the sphere's material to be red
		mat.color = Color.red;

		marker.AddComponent<SuspensionPoint>(); // add suspension point functionality
	}

	private Vector3 findSuspensionPoint(Rigidbody rb) {
		Vector3 com = rb.transform.TransformPoint(rb.centerOfMass);

		// TODO find amount to offset the CoM along direction of gravity (y) to intersect with the mesh


		return com + new Vector3(0, 0.6f, 0);
	}

	void Update()
	{
		Rigidbody rb = gameObject.GetComponent<Rigidbody>();

		// need to find the highest intersection point of the mesh that's above the com
		if (marker != null) {
			marker.GetComponent<Transform> ().position = findSuspensionPoint (rb);
		}
	}
}

