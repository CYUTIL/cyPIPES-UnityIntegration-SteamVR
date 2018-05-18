using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class cyPIPESConfigs : MonoBehaviour {

	public Dictionary<string,cyPIPESunit> allCyPIPESunits = new Dictionary<string,cyPIPESunit>();
	[Header("cyPIPES Logs and Errors Options")]
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Logs.")]
	public bool cyPIPESLogActive;
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Errors.")]
	public bool cyPIPESErrorsActive;
	int cyPIPESUnitCount;
	[Tooltip("Do not change, shown here for your reference.")]
	public string rootUnitAddress;
	[Tooltip("Do not change, shown here for your reference.")]
	public string rootUnitID;
	//Used only as a display in inspector and refereneced in Manual fx control example
	[Tooltip("Do not change, shown here for your reference.")]
	public List<string> unitList = new List<string>();

	//Init
	void Awake (){
		if (Application.isPlaying) {
			gameObject.transform.localPosition = Vector3.zero;
		}


		//Reading and storing cyPIPES master unit address
		try{
			string raw = System.IO.File.ReadAllText ("C:\\PIPES_Scanner\\PIPESMASTER.txt");
			if (raw != ""){
				string[] splitRaw = raw.Split(',');
				rootUnitAddress = splitRaw[0];
				rootUnitID = splitRaw[1];
				if (cyPIPESLogActive){Debug.Log ("cyPIPES Log: Root cyPIPES unit found as " + rootUnitID + " addressed " + rootUnitAddress + " all commands will be communicated to this address on port 5005.");}
			}
			if (raw == ""){
				if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: cyPIPESConfigs found an empty PIPESMASTER.txt. Please run cyPIPES Spatial Manager");}
			}
		}catch{
			if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: cyPIPESConfigs having trouble accessing PIPESMASTER.txt. Has cyPIPES Spatial Manager been installed on this system yet?");}
		}

		//Reading and storing cyPIPES unit config data
		string activeIDcsv = "";
		try{
			activeIDcsv = System.IO.File.ReadAllText ("C:\\PIPES_Scanner\\ConfigApps\\unitConfigs\\activeIDs.csv");
			if (cyPIPESLogActive){Debug.Log ("cyPIPES Log: Active units found " + activeIDcsv);}
		}catch{
			if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: cyPIPESConfigs having trouble accessing activeIDs.csv file. Please check cyPIPES Spatial Manager.");}
		}
		if (activeIDcsv != "") {
			string[] idParse = activeIDcsv.Split (',');
			cyPIPESUnitCount = idParse.Length;
			foreach (string unitID in idParse) {
				try{
					string jonData = System.IO.File.ReadAllText ("C:\\PIPES_Scanner\\ConfigApps\\unitConfigs\\"+unitID+".json");
					cyPIPESunit unit = JsonUtility.FromJson<cyPIPESunit>(jonData);
					allCyPIPESunits.Add (unitID,unit);
					if(unitList.Contains(unitID) == false){
						unitList.Add(unitID);
					}
					if (cyPIPESLogActive){Debug.Log("cyPIPES Log: cyPIPES unit " + unitID + " successfully added.");}
				}catch{
					if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: cyPIPESConfigs having trouble accessing unit " + unitID + "'s configuration .json file.");}
				}
			}
			//Only run if application is actually running
			if (Application.isPlaying) {
				//configurations stored, parse routine call
				if (allCyPIPESunits.Count == cyPIPESUnitCount) {
					gameObject.GetComponent<cyPIPESparsePost> ().parsePost ();
				}
			}
		}

	}

	public cyPIPESConfigs initRef(){
		GameObject mgrOj = GameObject.Find ("cyPIPES");
		cyPIPESConfigs comp = mgrOj.GetComponent<cyPIPESConfigs> ();
		return comp;
	}
	
}
