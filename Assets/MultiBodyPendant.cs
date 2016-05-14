using UnityEngine;
using System.Collections;

// might be about to 

public class MultiBodyPendant : MonoBehaviour {
	public ArrayList pendants; // could also implement a heap-like array for binary tree imp.
	private Vector3 com;

	// Use this for initialization
	void Start () {
		pendants = new ArrayList();
	}

	void addPendant(GameObject shape) {
		pendants.Add(shape);
	}

	Vector3 computeCenterOfMass() {
		Vector3 CoM_loc = Vector3.zero;
		float mass_sum = 0f;

		foreach (GameObject shape in pendants) {
			CoM_loc += shape.GetComponent<Rigidbody>().worldCenterOfMass * shape.GetComponent<Rigidbody>().mass;
			mass_sum += shape.GetComponent<Rigidbody>().mass;
		}

		CoM_loc /= mass_sum;
		com = CoM_loc;
		return CoM_loc;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
