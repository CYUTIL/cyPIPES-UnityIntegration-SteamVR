using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cyPIPESuserMonitor : MonoBehaviour {

	[Header("cyPIPES Logs and Errors Options")]
	//Debug options
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Logs.")]
	public bool cyPIPESLogActive;
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Errors.")]
	public bool cyPIPESErrorsActive;

	[HideInInspector]
	public cyPIPESparsePost parser;

	GameObject vrPlaySpace;

	[Tooltip("No need to assign this if you are using a standard [CameraRig] object it will be automaticlly referenced from cyPIPES. If you are using a custom user object other than the one in [CameraRig] please manually link it here in the inspector.")]
	public Transform userObj;
	[HideInInspector]
	public Vector3 userPosition;
	[HideInInspector]
	public Vector3 userChestPos;
	//[HideInInspector]
	public bool userInTrackedSpace;
	int cyPIPESlayer;

	void Awake () {
		
		cyPIPESlayer = LayerMask.NameToLayer ("cyPIPES");

		//Reference the VR play space object from cyPIPES<cyPIPESParsePost.cs>.  This also acts as a double check that users might not have made another object called cyPIPES
		try{
			vrPlaySpace = gameObject.GetComponent<cyPIPESparsePost>().vrPlaySpaceObj;
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
		
	}

	void Update () {

		//Update user position, chest position, debug ray, and if user is within the tracked space
		if (userObj != null) {
			//User pos, and chest pos
			userPosition = userObj.transform.position;
			userChestPos = new Vector3 (userPosition.x, userPosition.y - 0.3f, userPosition.z);
			//Debug Ray
			if (cyPIPESLogActive) {
				Debug.DrawRay (userPosition, Vector3.down, Color.red, 0.11f, true);
			}
			//Check out if user is within tracked space
			RaycastHit hit;
			Vector3 belowTrackedSpace = new Vector3 (userPosition.x, userPosition.y - 3.0f, userPosition.z);
			Vector3 userToBelow = belowTrackedSpace - userPosition;

			//Check if user is within tracked space
			if (Physics.Raycast (userPosition, userToBelow, out hit, cyPIPESlayer)) {
					userInTrackedSpace = true;
			} else {
				userInTrackedSpace = false;
				//Call for ALL OFF command
			}
		}
		
	}
}
