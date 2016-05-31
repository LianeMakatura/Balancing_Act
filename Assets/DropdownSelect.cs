using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropdownSelect : MonoBehaviour {
	public Dropdown myDropdown;
	private Button[] buttons;
	private Button button0, button1, button2, button3, button4, button5;

	public GameObject pend;
	public GameObject pendInstance;
	public Button myselfButton;
	private int tracking = 0;
	public Mesh model;

	void Start() {
		myDropdown = GetComponent<Dropdown> ();

		// mute all buttons before selection
		foreach (Button button in myDropdown.transform.parent.GetComponentsInChildren<Button>()) {
			button.interactable = false;
		}

		Canvas canvas;
		canvas = myDropdown.transform.parent.GetComponentInChildren<Canvas>();
		buttons = canvas.GetComponentsInChildren<Button> ();
		button0 = buttons [0];
		button1 = buttons [1];
		button2 = buttons [2];
		button3 = buttons [3];
		button4 = buttons [4];
		button5 = buttons [5];
			
		myDropdown.onValueChanged.AddListener(delegate {
			myDropdownValueChangedHandler(myDropdown);
		});
	}

	void Destroy() {
		myDropdown.onValueChanged.RemoveAllListeners();
	}

	// 0: Marine Animals, 1: Dinosaurs, 2: Star Wars
	private void myDropdownValueChangedHandler(Dropdown target) {
		foreach (Button button in myDropdown.transform.parent.GetComponentsInChildren<Button>()) {
			if (!button.interactable) button.interactable = true;
		}

		// instantiate 6 new buttons
		switch (target.value) {
		case 0:
			Debug.Log ("0");		// marine animals
			DisplayMarineAnimals ();
			break;
		case 1:						// dinosaurs
			Debug.Log ("1");
			DisplayDinosaurs ();
			break;
		case 2:						// star wars
			Debug.Log ("2");
			DisplayStarWars ();
			break;
		}
	}


	// 0 = blue_whale, 1 = Dolphin, 2 = jellyfish, 3 = seahorse, 4 = seal
	public void DisplayMarineAnimals() {
		prepButton (button0, "Marine/seastar", "Seastar");
		prepButton (button1, "Marine/Dolphin", "Dolphin");
		prepButton (button2, "Marine/jellyfish", "Jellyfish");
		prepButton (button3, "Marine/seahorse", "Seahorse");
		prepButton (button4, "Marine/seal", "Seal");
		prepButton (button5, "Marine/blue_whale", "Whale");
	}

	public void DisplayDinosaurs() {
		prepButton (button0, "Dinos/MrRex", "Velociraptor");
		prepButton (button1, "Dinos/dino_egg", "Dinosaur Egg");
		prepButton (button2, "Dinos/herbovor", "Apatosaurus");
		prepButton (button3, "Dinos/pointydino", "Stegosaurus");
		prepButton (button4, "Dinos/tRex", "T-Rex");
		prepButton (button5, "Dinos/humpydino", "Loch Ness Monster");
	}

	public void DisplayStarWars() {
		prepButton (button0, "Starwars/bb8", "BB-8");
		prepButton (button1, "Starwars/Darth_vader", "Darth Vader");
		prepButton (button2, "Starwars/falcon", "Millenium Falcon");
		prepButton (button3, "Starwars/death_star", "Death Star");
		prepButton (button4, "Starwars/Galactic_Empire", "Empire Logo");
		prepButton (button5, "Starwars/yoda", "Yoda");
	}

	void instantiateAnimal(string meshName) {
		model = (Mesh) Resources.Load (meshName, typeof(Mesh));

		pend.GetComponent<MeshFilter>().mesh = model;
		pend.GetComponent<MeshCollider>().sharedMesh = model;
		Debug.Log (pend.GetComponent<MeshCollider> ().sharedMesh);

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
