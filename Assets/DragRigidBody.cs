// from 
using UnityEngine;
using System.Collections;

public class DragRigidBody : MonoBehaviour {

	public float catchingDistance = 3f;
	bool isDragging = false;
	public bool isDraggable = true;

	// Use this for initialization
	void Start()
	{
	}
	// Update is called once per frame
	void Update() {
		if (isDraggable) {
			if (Input.GetMouseButton(0)) {
					TryToDrag();
			}
		}
	}

	public void makeImmutable() {
		isDraggable = false;
	}

	private void TryToDrag(){
		RaycastHit hitInfo = new RaycastHit();
		bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
		if (hit && hitInfo.transform.gameObject == gameObject) {
			gameObject.GetComponent<Rigidbody> ().MovePosition (CalculateMouse3DVector ());
		}

	}

	private Vector3 CalculateMouse3DVector(){
		Vector3 v3 = Input.mousePosition;
//		v3.z = catchingDistance;
		v3 = Camera.main.ScreenToWorldPoint(v3);
		return v3;
	}
}
