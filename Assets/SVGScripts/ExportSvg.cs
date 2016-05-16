using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExportSvg : MonoBehaviour {
	public Button myselfButton;

	// Use this for initialization
	void Start () {
		myselfButton = GetComponent<Button>();
		myselfButton.onClick.AddListener (() => Exporter.CreateWizard ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
