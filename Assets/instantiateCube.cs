using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class instantiateCube : MonoBehaviour {
	public GameObject pend;
	public GameObject pendInstance;
	public Button myselfButton;
	private int numCube = 0;

	// Use this for initialization
	void Start () {
		myselfButton = GetComponent<Button>();
		myselfButton.onClick.AddListener(() => instantiateMyCube());
	}

	void instantiateMyCube() {
		Vector3 pos = new Vector3(0, 0, 0);
		Quaternion rot = Quaternion.identity;
		pendInstance = GameObject.CreatePrimitive (PrimitiveType.Cube);

		pendInstance.AddComponent<MeshCollider> ();
		Destroy (pendInstance.GetComponent<BoxCollider>());

		pendInstance.AddComponent<Pendant> ();

		pendInstance.transform.position = pos;
		pendInstance.transform.rotation = rot;
		pendInstance.name = "Cube" + numCube++;
	
	} 

	// Update is called once per frame
	void Update () {
	
	}

	// make sure we free up the listener
	void Destroy() {
		myselfButton.onClick.RemoveListener(() => instantiateMyCube());
	}
}
