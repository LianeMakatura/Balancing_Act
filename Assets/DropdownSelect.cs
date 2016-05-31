using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropdownSelect : MonoBehaviour {
	public Dropdown myDropdown;
	private Button[] buttons;
	private Button button0, button1, button2, button3, button4;

	public GameObject pend;
	public GameObject pendInstance;
	public Button myselfButton;
	private int tracking = 0;
	public Mesh model;
//	private string name = "seal";

	void Start() {
		Transform panel;
		Canvas canvas;
		myDropdown = GetComponent<Dropdown> ();
		canvas = myDropdown.transform.parent.GetComponentInChildren<Canvas>();

		buttons = canvas.GetComponentsInChildren<Button> ();
		button0 = buttons [0];
		button1 = buttons [1];
		button2 = buttons [2];
		button3 = buttons [3];
		button4 = buttons [4];
			
		myDropdown.onValueChanged.AddListener(delegate {
			myDropdownValueChangedHandler(myDropdown);
		});
	}

	void Destroy() {
		myDropdown.onValueChanged.RemoveAllListeners();
	}

	// 0: Marine Animals, 1: Dinosaurs, 2: Star Wars
	private void myDropdownValueChangedHandler(Dropdown target) {
//		Debug.Log("selected: "+target.value);

		// instantiate 6 new buttons
		switch (target.value) {
		case 0:
			Debug.Log ("0");
			DisplayMarineAnimals ();
			break;
		case 1:
			Debug.Log("1");
			break;
		case 2:
			Debug.Log("2");
			break;
		}

		// for 6 different buttons
			// change text
			// change image
			// change onclick listener

	}

//	public GameObject CreateButton(Transform panel ,Vector3 position, Vector2 size)  
//	{
//		GameObject button = new GameObject("button1");
//		button.transform.parent = panel;
//		button.AddComponent<RectTransform>();
//		button.AddComponent<Button>();
//		button.transform.position = position;
////		button.GetComponent<RectTransform>().SetSize(size);
////		button.GetComponent<Button>().onClick.AddListener(method);
////		button.AddComponent(script);
//
//		return button;
//	}

	// 0 = blue_whale, 1 = Dolphin, 2 = jellyfish, 3 = seahorse, 4 = seal
	public void DisplayMarineAnimals() {
		prepButton (button0, "blue_whale", "Whale");
		prepButton (button1, "Dolphin", "Dolphin");
		prepButton (button2, "jellyfish", "Jellyfish");
		prepButton (button4, "seal", "Seal");

	}

	void instantiateAnimal(string meshName) {
		model = (Mesh) Resources.Load (meshName, typeof(Mesh));

		pend.GetComponent<MeshFilter>().mesh = model;
		pend.GetComponent<MeshCollider>().sharedMesh = model;

		Vector3 pos = new Vector3(0, 0, 0);
		Quaternion rot = Quaternion.identity;
		pendInstance = Instantiate (pend, pos, rot) as GameObject;
		pendInstance.AddComponent<Pendant> ();
		//
		pendInstance.name = meshName + " " + tracking++;
	} 

	void prepButton (Button button, string meshName, string buttonName) {
		button.onClick.RemoveAllListeners ();
		button.onClick.AddListener(() => instantiateAnimal(meshName));
		button.GetComponentInChildren<Text>().text = buttonName;

	}

	public void SetDropdownIndex(int index) {
		myDropdown.value = index;
	}
}
