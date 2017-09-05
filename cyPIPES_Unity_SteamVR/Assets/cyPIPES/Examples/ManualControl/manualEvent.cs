using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manualEvent : MonoBehaviour {

	cyPIPESConfigs units;
	public Dictionary<string,cyPIPES_ManualControl> onUnit = new Dictionary<string,cyPIPES_ManualControl>();

	// Use this for initialization
	void Start () {
		units = GameObject.Find("cyPIPES").GetComponent<cyPIPESConfigs>();
		populateUnitsHere ();

	}
	
	// Update is called once per frame
	void Update () {

	}

	void populateUnitsHere (){
		foreach (string unit in units.unitList) {
			cyPIPES_ManualControl newUnit = gameObject.AddComponent (typeof(cyPIPES_ManualControl)) as cyPIPES_ManualControl;
			newUnit.assignID (unit);
			onUnit.Add (unit, newUnit);
			Debug.Log ("KEY BEING PUT IN DICTIONARY IS: " + unit);
		}	
	}
}
