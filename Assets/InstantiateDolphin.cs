using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InstantiateDolphin : MonoBehaviour {
	public GameObject pend;
	public GameObject pendInstance;
	public Button myselfButton;
	private int numCube = 0;
	public Mesh model;

	// Use this for initialization
	void Start () {
		myselfButton = GetComponent<Button>();
		myselfButton.onClick.AddListener(() => instantiateMyDolphin());
	}

	void instantiateMyDolphin() {
		model = (Mesh) Resources.Load ("Dolphin", typeof(Mesh));
		//				pend = Instantiate(model) as GameObject;

		pend.GetComponent<MeshFilter>().mesh = model;
		pend.GetComponent<MeshCollider>().sharedMesh = model;

		Vector3 pos = new Vector3(0, 0, 0);
		Quaternion rot = Quaternion.identity;
		pendInstance = Instantiate (pend, pos, rot) as GameObject;
		pendInstance.AddComponent<Pendant> ();

		pendInstance.name = "Dolphin " + numCube++;

	} 

	// Update is called once per frame
	void Update () {

	}

	// make sure we free up the listener
	void Destroy() {
		myselfButton.onClick.RemoveListener(() => instantiateMyDolphin());
	}
}
