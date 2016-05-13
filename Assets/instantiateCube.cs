﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class instantiateCube : MonoBehaviour {
	public GameObject pend;
	public Button myselfButton;

	// Use this for initialization
	void Start () {
		myselfButton = GetComponent<Button>();
		myselfButton.onClick.AddListener(() => instantiateMyCube());
	}

	void instantiateMyCube() {
		Vector3 pos = new Vector3(0, 0, 0);
		Quaternion rot = Quaternion.identity;
		Instantiate (pend, pos, rot);
	}

	// Update is called once per frame
	void Update () {
	
	}

	// make sure we free up the listener
	void Destroy() {
		myselfButton.onClick.RemoveListener(() => instantiateMyCube());
	}
}
