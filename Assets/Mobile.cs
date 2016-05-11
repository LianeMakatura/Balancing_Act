using UnityEngine;
using System.Collections;

public class Mobile : MonoBehaviour {
	public ArrayList pendants; // could also implement a heap-like array for binary tree imp.

	// Use this for initialization
	void Start () {
		pendants = new ArrayList();
	}

	void addPendant(GameObject shape) {
		pendants.Add(shape);
	}

	Vector3 computeCenterOfMass() {
		Vector3 CoM = Vector3.zero;
		float mass_sum = 0f;

		foreach (GameObject shape in pendants) {
			CoM += shape.GetComponent<Rigidbody>().worldCenterOfMass * shape.GetComponent<Rigidbody>().mass;
			mass_sum += shape.GetComponent<Rigidbody>().mass;
		}

		CoM /= mass_sum;
		return CoM;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
