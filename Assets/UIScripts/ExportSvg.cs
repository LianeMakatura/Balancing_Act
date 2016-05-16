using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using Object = UnityEngine.Object;

public class ExportSvg : MonoBehaviour {

	// initialization when new components added
	void Awake() {
	
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void exportSvg() {
		string path = "test_svg.svg";
		TextWriter f = new StreamWriter(path);

		f.WriteLine("testing\t" + "testing\n" + "testing");

		f.Close();
	}

	// check if xml file is done loading
	public IEnumerator LoadXml(WWW xml) {
		while (!xml.isDone)
			yield return null;
	}

	/*/// <summary> Takes a path to the SVG to be imported, useful for button callbacks </summary>
	public IEnumerator ImportSvg(string path) {

		if (!UseUrl && SvgFile == null) {
			Debug.Log("RageTools: SVG File not set, canceling import.");
			yield break;
		}

		// Initializes the stuff we'll need
		var svgData = RageSvgObject.NewInstance();
		_gradients = new Dictionary<string, RageSvgGradient>();

		// The starting parent is the current one, ie. the root
		svgData.Parent = gameObject.transform;

		// Actual importing is started here
		yield return StartCoroutine(SvgLoad(svgData, path));

		// If it has a RageGroup attached, auto-update it after import
		var group = GetComponent<RageGroup>();
		if (group != null) group.UpdatePathList();

		if (PerspectiveMode) {
			var childSpline = GetComponentInChildren<RageSpline>();
			childSpline.AllRageSplinesTo3D();
		}
	}*/


}
