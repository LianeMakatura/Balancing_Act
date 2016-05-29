using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class InstantiateFish : MonoBehaviour {
	public GameObject pend;
	public GameObject pendInstance;
	public Button myselfButton;
	private int numCube = 0;
	public Mesh model;

	// Use this for initialization
	void Start () {
		myselfButton = GetComponent<Button>();
		myselfButton.onClick.AddListener(() => instantiateMyFish());
	}

	void instantiateMyFish() {
		model = (Mesh) Resources.Load ("Tropical Fish", typeof(Mesh));
//		pend = Instantiate(model) as GameObject;

		pend.GetComponent<MeshFilter>().mesh = model;
		pend.GetComponent<MeshCollider>().sharedMesh = model;
		pend.name = "testing";

//		pend.AddComponent<MeshFilter> ();
//		pend.AddComponent<Pendant> ();
//		pend.GetComponent<MeshCollider>().isTrigger = true;

		Vector3 pos = new Vector3(0, 0, 0);
		Quaternion rot = Quaternion.identity;
		pendInstance = Instantiate (pend, pos, rot) as GameObject;
		pendInstance.AddComponent<Pendant> ();
//
		pendInstance.name = "Fish " + numCube++;
	} 

	// Update is called once per frame
	void Update () {

	}

	// make sure we free up the listener
	void Destroy() {
		myselfButton.onClick.RemoveListener(() => instantiateMyFish());
	}
}