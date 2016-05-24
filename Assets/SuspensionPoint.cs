using UnityEngine;
using System.Collections;

public class SuspensionPoint : MonoBehaviour {
	private Color[] colors = new Color[] {Color.red, Color.yellow};
	private int currentColor;
	private bool colorChanged;
	private MobileMaster masterMobile;


	// Use this for initialization
	void Start () {
		currentColor = 0;
		masterMobile = GameObject.Find("Mobile_Master").GetComponent<MobileMaster> ();

		// add fixed joint
		gameObject.AddComponent<FixedJoint>(); // implicitly connected to the world

		// set to trigger
		gameObject.GetComponent<SphereCollider>().isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		colorChanged = false;

		// check to see if the susp. pt has been selected
		if(Input.GetMouseButtonDown(0)){
			if (masterMobile.selected.Count <= 2) { // there's still room in the selected array
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, 100) && hit.transform.gameObject == gameObject) {
					currentColor = 1 - currentColor;
					colorChanged = true;
					Debug.Log ("colorChanged");
				} 
			}
		}

		// make the selections and update the visuals
		if (colorChanged) {
			if (currentColor == 1 && masterMobile.selected.Count < 2) { // has been selected & can add
				Debug.Log("Case 1");
				masterMobile.selected.Add(gameObject);
				gameObject.GetComponent<Renderer> ().material.color = colors [currentColor]; // select
			}
			else if (currentColor == 0){ // has been unselected
				Debug.Log("Case 2");
				masterMobile.selected.Remove(gameObject);
				gameObject.GetComponent<Renderer> ().material.color = colors [currentColor]; // unselect
			}
			else {		// tried to select but couldn't add; unchange the color and inform user
				Debug.Log("Case 3");
				currentColor = 0;
				gameObject.GetComponent<Renderer> ().material.color = colors [0]; // unselect
				Debug.Log("cannot select more than 2 objects at a time");
			}
		}
	}
}
