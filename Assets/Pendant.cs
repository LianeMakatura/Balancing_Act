using UnityEngine;
using System.Collections;

public class Pendant : MonoBehaviour {
	public float mat_density;

	//use this for initialization
	void Start ()
	{
		MeshFilter meshF = gameObject.GetComponent<MeshFilter>();
		float volume = ComputeVolume(meshF);
		string msg = "The volume of the mesh is " + volume + " cube units.";
		Debug.Log(msg);

		float newMass = volume * mat_density;
		gameObject.GetComponent<Rigidbody>().mass = newMass;
		string msg2 = "The mass of the mesh is " + volume*mat_density + " cube units.";
		Debug.Log(msg2);
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



	public void Update() {
		
	}

}
