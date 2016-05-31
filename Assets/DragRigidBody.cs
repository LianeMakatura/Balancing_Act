// inspired by http://answers.unity3d.com/questions/596454/drag-rigidbody.html
using UnityEngine;
using System.Collections;

public class DragRigidBody : MonoBehaviour {

	public float catchingDistance = 3f;
	public static bool isDragging = false; // one for all the classes, so we only drag one object at a time.
	public bool imDragging = false; // one just for me! 
	public bool isDraggable = true;
	private Vector3 moveVec;
	private Vector3 offset;

	// Use this for initialization
	void Start()
	{
	}
	// Update is called once per frame
	void Update() {
		if (isDraggable) {
			if (Input.GetMouseButton(0)) {
				if (isDragging == false) {		// if we're not already dragging, check
					TryToDrag ();
				}
				else if (imDragging) { // I'm the one being dragged! (otherwise somebody else is)
					moveVec = CalculateMouse3DVector ();
					moveVec += new Vector3 (offset.x, offset.y, 0.0f); // update the offset, only in x,y
					gameObject.GetComponent<Rigidbody> ().MovePosition (moveVec);
				}
			}
			else if (!Input.GetMouseButton(0)) { // we've since let go of the mouse
				isDragging = false;
				imDragging = false;
			}
		}
	}

	public void makeImmutable() {
		isDraggable = false;
	}

	private void TryToDrag(){
		RaycastHit hitInfo = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		bool hit = Physics.Raycast(ray, out hitInfo);
		if (hit && hitInfo.transform.gameObject == gameObject) {
			isDragging = true;
			imDragging = true;
			offset = gameObject.transform.position - ray.origin; // to offset so it doesn't align with origin of object
		}

	}

	private Vector3 CalculateMouse3DVector(){
		Vector3 v3 = Input.mousePosition;
		//		v3.z = catchingDistance;
		v3 = Camera.main.ScreenToWorldPoint(v3);
		return v3;
	}
}
