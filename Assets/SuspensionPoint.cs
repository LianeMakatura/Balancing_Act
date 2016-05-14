using UnityEngine;
using System.Collections;

public class SuspensionPoint : MonoBehaviour {
	private Color[] colors = new Color[] {Color.red, Color.yellow};
	private int currentColor;
	// Use this for initialization
	void Start () {
		currentColor = 0;
	}
	
	// Update is called once per frame
	void Update () {

		// check to see if the susp. pt has been selected
		// will also need to check if more the 2 other pts selected, and update/track a "selected" variable for functionality
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100)) {
				currentColor = 1 - currentColor;
				gameObject.GetComponent<Renderer>().material.color = colors[currentColor]; // select or unselect
			}
		}
	}
}
