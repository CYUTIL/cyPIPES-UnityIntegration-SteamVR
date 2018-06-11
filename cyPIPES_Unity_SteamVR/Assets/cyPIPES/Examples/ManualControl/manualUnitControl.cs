using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manualUnitControl : MonoBehaviour {

	[Header("cyPIPES Logs and Errors Options")]
	//Debug options
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Logs.")]
	public bool cyPIPESLogActive;
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Errors.")]
	public bool cyPIPESErrorsActive = true;

	[HideInInspector]
	public cyPIPESparsePost cyPIPESnetwork;
	public List<GameObject> cyPIPESunits;
	List<cyPIPES> controlList = new List<cyPIPES>();
	Dictionary<string,cyPIPES> controlUnit = new Dictionary<string,cyPIPES>();

	void Start () {
		//locate the cyPIPESParsePost component on the master cyPIPES prefan in the scene
		cyPIPESnetwork = GameObject.Find ("cyPIPES").GetComponent<cyPIPESparsePost> ();

		//Dig to find all virtual cyPIPES unit GameObjects
		try{
			foreach(GameObject unit in cyPIPESnetwork.allUnitObjects.Values){
				cyPIPESunits.Add(unit);
				controlList.Add(unit.GetComponent<cyPIPES>());
				controlUnit.Add(unit.name,unit.GetComponent<cyPIPES>());
			}
		}catch{
			if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: Cannot find virtual cyPIPES Unit(s) for manual control.");}
		}
		
	}
	
	// Update is called once per frame
	void Update () {

		//Example of manually scripting control of a unit specifically by unitID, replace 0000030 with your unique unitID number printed on the top of your cyPIPES unit.
		//On unit 0000030 turn on channel 1 when the space bar is pressed, or turn it off when the spacebar is not pressed.
		if (Input.GetKeyDown (KeyCode.Space)) {
			controlUnit ["0000030"].ch1Param (1);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			controlUnit ["0000030"].ch1Param (0);
		}

		//Example of manually scripting control of a unit without knowing it's unitID, this is more for processing commands when you do not know the unitIDs for some reason.
		//On the first found unit turn on channel 2 when the space bar is pressed, or turn it off when the spacebar is not pressed.
		if (Input.GetKeyDown (KeyCode.Space)) {
			controlList[0].ch2Param (1);
		}
		if (Input.GetKeyUp (KeyCode.Space))
		{
			controlList[0].ch2Param (0);
		}
	}
}
