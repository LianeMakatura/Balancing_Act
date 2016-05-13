using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public float RotateSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		Vector3 RotationVect = new Vector3(0, RotateSpeed,0);
		transform.Rotate(RotationVect);
	
	}
}
