// from 
using UnityEngine;
using System.Collections;

public class DragRigidBody : MonoBehaviour {

	public float catchingDistance = 3f;
	bool isDragging = false;
	GameObject draggingObject;
	public bool isDraggable = true;

	// Use this for initialization
	void Start()
	{
	}
	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButton(0)) {
			if (!isDragging) {
				draggingObject = GetObjectFromMouseRaycast();
				if (draggingObject) {
					isDragging = true;
				}
			}
			else if (draggingObject != null) {
				draggingObject.GetComponent<Rigidbody>().MovePosition(CalculateMouse3DVector());
			}
		}
		else {
			isDragging = false;
		}
	}

	public void makeImmutable() {
		isDraggable = false;
	}

	private GameObject GetObjectFromMouseRaycast(){
		GameObject gmObj = null;
		RaycastHit hitInfo = new RaycastHit();
		bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
		if (hit) {
			if (hitInfo.collider.gameObject.GetComponent<Rigidbody>() &&
				Vector3.Distance(hitInfo.collider.gameObject.transform.position,
					transform.position) <= catchingDistance) {
				gmObj = hitInfo.collider.gameObject;
			}
		}
		return gmObj;
	}

	private Vector3 CalculateMouse3DVector(){
		Vector3 v3 = Input.mousePosition;
		v3.z = catchingDistance;
		v3 = Camera.main.ScreenToWorldPoint(v3);
		return v3;
	}
}
