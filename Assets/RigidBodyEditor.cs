using UnityEditor;
using UnityEngine;

public class RigidBodyEditor : MonoBehaviour
{
	private GameObject marker= GameObject.CreatePrimitive(PrimitiveType.Sphere);
	void Update()
	//try parenting the sphere to the cube so that each cube has 1, don't create a new one unless we don't already have a sphere
	{
		Rigidbody rb = gameObject.GetComponent<Rigidbody>();
		Material mat = marker.GetComponent<Renderer>().material;
		mat.color = Color.red;
		marker.GetComponent<Transform>().position = rb.transform.TransformPoint(rb.centerOfMass);
	}
}

