using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class InstantiateStar : MonoBehaviour {
	public GameObject pend;
	public Button myselfButton;

	// Use this for initialization
	void Start () {
		myselfButton = GetComponent<Button>();
		myselfButton.onClick.AddListener(() => instantiateMyStar());
	}

	void instantiateMyStar() {
		pend = Instantiate(Resources.Load("Tropical Fish", typeof(GameObject))) as GameObject;
		pend.AddComponent<Pendant> ();
		pend.name = "testing";

		pend.GetComponent<MeshCollider>().isTrigger = true;
	} 

	// Update is called once per frame
	void Update () {

	}

	// make sure we free up the listener
	void Destroy() {
		myselfButton.onClick.RemoveListener(() => instantiateMyStar());
	}
}
