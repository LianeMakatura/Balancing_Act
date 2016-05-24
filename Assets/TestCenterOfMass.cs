using UnityEngine;
using System.Collections;

public class TestCenterOfMass : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var rb = gameObject.GetComponent<Rigidbody>();
		rb.ResetCenterOfMass ();
		Debug.Log (rb.centerOfMass);
	}
}
