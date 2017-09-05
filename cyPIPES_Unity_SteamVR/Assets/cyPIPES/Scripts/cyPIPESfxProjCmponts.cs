using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cyPIPESfxProjCmponts : MonoBehaviour {

	[Header("cyPIPES Logs and Errors Options")]
	//Debug options
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Logs.")]
	public bool cyPIPESLogActive;
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Errors.")]
	public bool cyPIPESErrorsActive;

	//Basically the cyPIPES master object & user reference set
	[HideInInspector]
	public GameObject cyPIPESmgmtObj;
	[HideInInspector]
	public GameObject vrPlaySpace;
	[HideInInspector]
	public cyPIPESuserMonitor userInfo;
	[Tooltip("No need to assign this if you are using a standard [CameraRig] object it will be automaticlly referenced from cyPIPES. If you are using a custom user object other than the one in [CameraRig] please manually link it here in the inspector.")]
	public Transform userObj;
	[HideInInspector]
	public cyPIPEStileNormalization recordSource;
	[HideInInspector]
	public cyPIPESparsePost parser;
	[HideInInspector]
	public Vector3 userPos;
	[HideInInspector]
	public Vector3 chestPos;
	[HideInInspector]
	public bool userActive;
	[HideInInspector]
	public int cyPIPESlayer;

	//Reference vEFDTileRecod
	public Dictionary <string,GameObject> tileRecord = new Dictionary <string,GameObject>();

	//Awake function use just in case developers dynamically instatiate effects after runtime.
	void Awake () {

		//Reference cyPIPES Layer Mask
		cyPIPESlayer = LayerMask.NameToLayer ("cyPIPES");

		//Reference cyPIPES master object
		try{
			cyPIPESmgmtObj = GameObject.Find("cyPIPES");
		}catch{
			if(cyPIPESErrorsActive){Debug.LogError ("cyPIPES ERROR: FX Projection having a hard time finding the cyPIPES master object. Have you added it to the scene? It should be located in Assets/cyPIPES/Prefabs");}
		}
		//Reference the VR play space object from cyPIPES<cyPIPESParsePost.cs>.  This also acts as a double check that users might not have made another object called cyPIPES
		try{
			if(cyPIPESmgmtObj != null){
				vrPlaySpace = cyPIPESmgmtObj.GetComponent<cyPIPESparsePost>().vrPlaySpaceObj;
			}
		}catch{
			//Looks like cyPIPESPasePost is not present. This is likely because a seconed object is named "cyPIPES" and is not the cyPIPES master object
			if(cyPIPESErrorsActive){Debug.LogError ("cyPIPES ERROR: Effects objects cannot reference the vr play space from cyPIPES master object. Do you have another object in the scene named \"cyPIPES\"? If so please rename or remove it so that only the cyPIPES master prefab has this name.");}

			//Checking for custom vr play space and custom user object
			if (vrPlaySpace == null) {
				if (userObj == null) {
					if (cyPIPESLogActive) {
						Debug.Log ("cyPIPES Log: A custom VR tracked space has been detected. If you are using a custom user object other than the one in [CameraRig] please manually link it as userObj in each fx object");
					}
				}
			}
		}
		//Refrencing cyPIPEStileNormalization component
		try{
			recordSource = cyPIPESmgmtObj.GetComponent<cyPIPEStileNormalization>();
		}catch{
			if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: Unable to reference cyPIPEStileNormalization.cs from cyPIPES master object be sure it is active");}
		}
		//Referenceing parser component
		try{
			parser = cyPIPESmgmtObj.GetComponent<cyPIPESparsePost>();
		}catch{
			if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: Unable to reference cyPIPESparsePost.cs from cyPIPES master object be sure it is active");}
		}
		//Reference user object
		try{
			//if userObj is not manually assigned as a custom one, reference the Camera (eye) in [CameraRig]
			if (userObj == null)
			{
				userObj = vrPlaySpace.transform.GetChild(2);
			}
		}catch{
			if(cyPIPESErrorsActive){Debug.LogError ("cyPIPES ERROR: " + this.gameObject.name + " unable to reference user object from [CameraRig] component.");}
		}

		//Reference userMonitor
		try{
			userInfo = cyPIPESmgmtObj.GetComponent<cyPIPESuserMonitor>();
		}catch{
			if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: vEFD refrencing issue with cyPIPESuserMonitor.");}
		}

		//Make a copy of tile record with a public function so that developers could call later for updates to tile record.
		updateTileRecord();

	}

	void Update () {
		//Transposing values to be less verbose
		userPos = userInfo.userPosition;
		chestPos = userInfo.userChestPos;
		userActive = userInfo.userInTrackedSpace;
	}

	//Update tileRecord
	public void updateTileRecord(){

		try{
			
			//Check to see if vEFDtileRecord is complete, if not just wait for a moment
			if(recordSource.vEFDtileRecordComplete != true){
				StartCoroutine(checkOnTileRecord(0.5f));
			}

			//Make a local copy of the vEFDtileRecord if it has finished updating
			if(recordSource.vEFDtileRecordComplete == true){
				foreach (string key in recordSource.vEFDtileRecord.Keys){
					tileRecord.Add(key,recordSource.vEFDtileRecord[key]);
				}
				//Debug.Log("TILE RECORD COUNT IS " + tileRecord.Count);
			}

		}catch{
			if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: " + this.gameObject.name + " is having a problem updating the vEFD tile record.");}
		}

	}

	//Coroutine to wait until vEFDtileRecord is completed
	IEnumerator checkOnTileRecord(float waitTime){
		yield return new WaitForSeconds (waitTime);
		//Debug.Log ("COROUTINE SEES " + recordSource.vEFDtileRecordComplete);
		updateTileRecord ();
	}

}
