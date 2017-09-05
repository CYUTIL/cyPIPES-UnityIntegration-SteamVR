using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class otherFX_cyPIPES : MonoBehaviour {

	[Header("cyPIPES Logs and Errors Options")]
	//Debug options
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Logs.")]
	public bool cyPIPESLogActive;
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Errors.")]
	public bool cyPIPESErrorsActive;

	[HideInInspector]
	public cyPIPESfxProjCmponts dataRef;

	public enum FXtype{
		ambientEffect,
		rangedDirectionalEffect,
		directionalEffect
	}

	public FXtype otherEffectType;

	[Header("Effect Parameters")]
	[Tooltip("Set the effective range of this effect")]
	public float range = 10.0f;
	//Had to up the scope on these two. They are used during update
	[Tooltip("This field is for advanced cy.PIPES developers who have custom vEFDs attached via USB or bluetooth and need to communicate custome commands with this field")]
	public string customStringCommand;
	GameObject closestHit;
	string tileHit;
	string lastTileHit;
	List<string>activeChs = new List<string>();
	List<string> tileNames = new List<string> ();

	void Awake(){
		//Collect all tile names. (Right now this is manual, but later it will be automated based on proceedurally generated tiles)
		tileNames.Add("backTile");
		tileNames.Add("backTileOut");
		tileNames.Add("leftTile");
		tileNames.Add("leftTileOut");
		tileNames.Add("frontTile");
		tileNames.Add("frontTileOut");
		tileNames.Add("rightTile");
		tileNames.Add("rightTileOut");
		tileNames.Add("topTile");
		tileNames.Add("topTileOut");
		tileNames.Add("bottomTile");
		tileNames.Add("bottomTileOut");
	}

	// Update is called once per frame
	void Update () {

		//check distance between user's chest and vEF
		float userEFdistance = Vector3.Distance(dataRef.chestPos, gameObject.transform.position);

		if (otherEffectType == FXtype.ambientEffect) {
			//AMBIENT FX ON
			//AMBIENT FX ON
			//AMBIENT FX ON
			//AMBIENT FX ON
			//AMBIENT FX ON
			//AMBIENT FX ON
			//If user is in active space
			if (dataRef.userActive) {
				//if this vEF is within range
				if (userEFdistance <= range) {
					//raycast to user and record what is hit in an array
					RaycastHit[] hits;
					Vector3 direction = dataRef.userPos - gameObject.transform.position;
					hits = Physics.RaycastAll (transform.position, direction, range);//NOTE FOR THE FUTURE, vEFs inside the tracked space will never register a hit a -1 raycast from user is needed for this.
					if (hits.Length > 0) {
						//if first object hit is within the cyPIPES layer
						float[] hitDistance = new float[hits.Length];
						int i = 0;
						foreach (RaycastHit hit in hits) {//sync hit distances with hits
							hitDistance [i] = Vector3.Distance (hit.transform.position, transform.position);
							i++;
						}
						int j = 0;
						foreach (float dist in hitDistance) {//find shortest hit disance object
							if (dist == Mathf.Min (hitDistance)) {//be sure that the closet object is a cyPIPES layer object 
								if (hits [j].transform.gameObject.layer == dataRef.cyPIPESlayer) {
									closestHit = hits [j].transform.gameObject;//reference which tile it is
									//Debug.Log ("HIT DETECTED ON " + closestHit.name);
								}
							}
							j++;
						}
						//check through tile record for matches in FX types
						//Normalize tileHit name
						try{
							tileHit = normalizeTileName(closestHit.name);
						}catch{
							Debug.LogError ("cyPIPES ERROR: Tile hit name is unrecognized for use in "+ gameObject.name);
						}
						//Debug.Log ("CURRENT TILE HIT = " + tileHit);
						if (tileHit != "") {	
							foreach (string data in dataRef.tileRecord.Keys) {
								//if FX type is matched on tile change unit channel propetries to intesnsity level and record unitID,ch in local active list (EVERY FRAME SEND ON TYPE COMMANDS)
								//reminder data format is unitID,ch#,chType,tileAssignment
								string[] details = data.Split (',');
								if (details [2] == "Other") {//its an Other channel
									if (details [3] == tileHit) {//if its the tile
										foreach (string uID in dataRef.parser.allUnitObjects.Keys) {//lets communicate the ON command now
											if (uID == details [0]) {//if the unitID matches unitID
												//change parameter in the correct channel
												if (details [1] == "ch1") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH1 = customStringCommand;}
												if (details [1] == "ch2") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH2 = customStringCommand;}
												if (details [1] == "ch3") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH3 = customStringCommand;}
												if (details [1] == "ch4") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH4 = customStringCommand;}
												if (details [1] == "ch5") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH5 = customStringCommand;}
												if (details [1] == "ch6") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH6 = customStringCommand;}
												//Assign data to activeChs[]
												if(!activeChs.Contains(data)){
													activeChs.Add (data);
												}
												//Debug.Log ("DATA FROM vEF BEING UTILZED " + data);
											}
										}
									}
								}
							}

						} else {
							if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: vEF raycast is hitting " + closestHit.name + " which is in cyPIPES layer, but not a tile.");}
						}
					}
				}
			}

			//AMBIENT FX OFF
			//AMBIENT FX OFF
			//AMBIENT FX OFF
			//AMBIENT FX OFF
			//AMBIENT FX OFF
			//AMBIENT FX OFF
			//if local activeChs list is not empty
			if (activeChs.Count > 0) {
				//If user is not in active space OFF condition
				if (dataRef.userActive == false) {
					//shut down all channels in active list and clear list (ONLY ONCE SEND OFF TYPE COMMANDS)
					foreach (string data in activeChs) {
						//reminder data format is unitID,ch#,chType,tileAssignment
						string[] details = data.Split (',');
						foreach (string uID in dataRef.parser.allUnitObjects.Keys) {
							if (uID == details [0]) {//if the unitID matches unitID record entry
								if (details [1] == "ch1") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH1 = "0";}
								if (details [1] == "ch2") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH2 = "0";}
								if (details [1] == "ch3") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH3 = "0";}
								if (details [1] == "ch4") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH4 = "0";}
								if (details [1] == "ch5") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH5 = "0";}
								if (details [1] == "ch6") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH6 = "0";}
								//activeChs.Clear ();//NOT WORKING
								//activeChs.Remove(data);
							}
						}
					}
				}
				//if user is not within range OFF condition
				if (userEFdistance > range) {
					//change unit channel properties to 0 and clear local active list (ONLY ONCE SEND OFF TYPE COMMANDS)
					foreach (string data in activeChs) {
						//reminder data format is unitID,ch#,chType,tileAssignment
						string[] details = data.Split (',');
						foreach (string uID in dataRef.parser.allUnitObjects.Keys) {
							if (uID == details [0]) {//if the unitID matches unitID
								if (details [1] == "ch1") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH1 = "0";}
								if (details [1] == "ch2") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH2 = "0";}
								if (details [1] == "ch3") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH3 = "0";}
								if (details [1] == "ch4") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH4 = "0";}
								if (details [1] == "ch5") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH5 = "0";}
								if (details [1] == "ch6") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH6 = "0";}
								//activeChs.Clear ();//NOT WORKING
								//activeChs.Remove(data);
							}
						}
					}
				}
				//if this vEF changes from current tileHit to another
				if (lastTileHit != tileHit) {
					//change unit channel properties to 0 and clear local active list (ONLY ONCE SEND OFF TYPE COMMANDS)
					foreach (string data in activeChs) {
						//reminder data format is unitID,ch#,chType,tileAssignment
						string[] details = data.Split (',');
						foreach (string uID in dataRef.parser.allUnitObjects.Keys) {
							if (uID == details [0]) {//if the unitID matches unitID
								if (details [1] == "ch1") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH1 = "0";}
								if (details [1] == "ch2") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH2 = "0";}
								if (details [1] == "ch3") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH3 = "0";}
								if (details [1] == "ch4") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH4 = "0";}
								if (details [1] == "ch5") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH5 = "0";}
								if (details [1] == "ch6") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH6 = "0";}
								//activeChs.Clear ();//NOT WORKING
								//activeChs.Remove(data);
							}
						}
					}
				}
				lastTileHit = tileHit;
			}

			//AMBIENT FX DEBUG
			//AMBIENT FX DEBUG
			//AMBIENT FX DEBUG
			//AMBIENT FX DEBUG
			//AMBIENT FX DEBUG
			//AMBIENT FX DEBUG
			if (cyPIPESLogActive) {
				//Calculate Endpoint
				Vector3 direction = dataRef.userPos - gameObject.transform.position;
				Vector3 endPoint = (Vector3.zero + (direction.normalized * range));
				Debug.DrawRay (gameObject.transform.position, endPoint, Color.black, 0.11f);
			}
		}//End of Ambient FX type

		//RANGED DIRECTIONAL FX ON
		//RANGED DIRECTIONAL FX ON
		//RANGED DIRECTIONAL FX ON
		//RANGED DIRECTIONAL FX ON
		//RANGED DIRECTIONAL FX ON
		//RANGED DIRECTIONAL FX ON
		if (otherEffectType == FXtype.rangedDirectionalEffect) {
			//If user is in active space
			if (dataRef.userActive) {
				//Ditermine which tile is active for this effect (This is too basic. In the future you need analyze where on the tile it hits and include nearest 1 or two tiles for more of a gradient effect) Corners of the tile cude cause switching instead of gradient of effects
				Vector3 thisFXdirection = gameObject.transform.forward;//EF direction
				Vector3 userToEFdir = gameObject.transform.position - dataRef.userPos;//user to vEF direction
				Vector3 endPoint = (userToEFdir + (gameObject.transform.forward.normalized * range));//endpoint direction from user
				RaycastHit fxhit;
				//Raycasting from user to directional endpoint taking into account range
				Physics.Raycast (dataRef.userPos, endPoint, out fxhit,10.0f);
				if (cyPIPESErrorsActive) {Debug.DrawRay (dataRef.userPos, endPoint, Color.magenta, 0.11f);} //Magenta raycast annotation from user to directional endpoint
				string directionalTile = null;
				try{
					Debug.Log(fxhit.transform.gameObject.name);
					if(fxhit.collider != null){
						directionalTile = oppositeTile(fxhit.transform.gameObject.name);
					}
				}catch{
					if (fxhit.collider != null) {
						if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: Tile hit name is unrecognized for use in " + gameObject.name + " as " + fxhit.transform.gameObject.name);}
					}
				}
				//If the directional tile is not null
				if (directionalTile != null) {
					//if this vEF is within range
					if (userEFdistance <= range) {
						//raycast to user and record what is hit in an array
						RaycastHit[] hits;
						Vector3 direction = dataRef.userPos - gameObject.transform.position;
						hits = Physics.RaycastAll (transform.position, direction, range);//NOTE FOR THE FUTURE, vEFs inside the tracked space will never register a hit a -1 raycast from user is needed for this.
						if (hits.Length > 0) {
							//if first object hit is within the cyPIPES layer
							float[] hitDistance = new float[hits.Length];
							int i = 0;
							foreach (RaycastHit hit in hits) {//sync hit distances with hits
								hitDistance [i] = Vector3.Distance (hit.transform.position, transform.position);
								i++;
							}
							int j = 0;
							foreach (float dist in hitDistance) {//find shortest hit disance object
								if (dist == Mathf.Min (hitDistance)) {//be sure that the closet object is a cyPIPES layer object 
									if (hits [j].transform.gameObject.layer == dataRef.cyPIPESlayer) {
										closestHit = hits [j].transform.gameObject;//reference which tile it is
										//Debug.Log ("HIT DETECTED ON " + closestHit.name);
									}
								}
								j++;
							}
							//check through tile record for matches in FX types
							//Normalize tileHit name
							try {
								tileHit = directionalTile;
							} catch {
								if (cyPIPESErrorsActive) {
									Debug.LogError ("cyPIPES ERROR: Tile hit name is unrecognized for use in " + gameObject.name);
								}
							}
							//Debug.Log ("CURRENT TILE HIT = " + tileHit);
							if (tileHit != "") {	
								foreach (string data in dataRef.tileRecord.Keys) {
									//if FX type is matched on tile change unit channel propetries to intesnsity level and record unitID,ch in local active list (EVERY FRAME SEND ON TYPE COMMANDS)
									//reminder data format is unitID,ch#,chType,tileAssignment
									string[] details = data.Split (',');
									if (details [2] == "Other") {//its an Other channel
										if (details [3] == directionalTile) {//if record includes the directional tile
											foreach (string uID in dataRef.parser.allUnitObjects.Keys) {//lets communicate the ON command now
												if (uID == details [0]) {//if the unitID matches unitID
													//change parameter in the correct channel
													if (details [1] == "ch1") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH1 = customStringCommand;}
													if (details [1] == "ch2") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH2 = customStringCommand;}
													if (details [1] == "ch3") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH3 = customStringCommand;}
													if (details [1] == "ch4") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH4 = customStringCommand;}
													if (details [1] == "ch5") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH5 = customStringCommand;}
													if (details [1] == "ch6") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH6 = customStringCommand;}
													//Assign data to activeChs[]
													if(!activeChs.Contains(data)){
														activeChs.Add (data);
													}
													//Debug.Log ("DATA FROM vEF BEING UTILZED " + data);
												}
											}
										}
									}
								}

							} else {
								if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: vEF raycast is hitting " + closestHit.name + " which is in cyPIPES layer, but not a tile.");}
							}
						}
					}
				}
			}

			//RANGED DIRECTIONAL FX OFF
			//RANGED DIRECTIONAL FX OFF
			//RANGED DIRECTIONAL FX OFF
			//RANGED DIRECTIONAL FX OFF
			//RANGED DIRECTIONAL FX OFF
			//RANGED DIRECTIONAL FX OFF
			//if local activeChs list is not empty
			if (activeChs.Count > 0) {
				//If user is not in active space OFF condition
				if (dataRef.userActive == false) {
					//shut down all channels in active list and clear list (ONLY ONCE SEND OFF TYPE COMMANDS)
					foreach (string data in activeChs) {
						//reminder data format is unitID,ch#,chType,tileAssignment
						string[] details = data.Split (',');
						foreach (string uID in dataRef.parser.allUnitObjects.Keys) {
							if (uID == details [0]) {//if the unitID matches unitID record entry
								if (details [1] == "ch1") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH1 = "0";}
								if (details [1] == "ch2") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH2 = "0";}
								if (details [1] == "ch3") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH3 = "0";}
								if (details [1] == "ch4") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH4 = "0";}
								if (details [1] == "ch5") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH5 = "0";}
								if (details [1] == "ch6") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH6 = "0";}
								//activeChs.Clear ();//NOT WORKING
								//activeChs.Remove(data);
							}
						}
					}
				}
				//if user is not within range OFF condition
				if (userEFdistance > range) {
					//change unit channel properties to 0 and clear local active list (ONLY ONCE SEND OFF TYPE COMMANDS)
					foreach (string data in activeChs) {
						//reminder data format is unitID,ch#,chType,tileAssignment
						string[] details = data.Split (',');
						foreach (string uID in dataRef.parser.allUnitObjects.Keys) {
							if (uID == details [0]) {//if the unitID matches unitID
								if (details [1] == "ch1") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH1 = "0";}
								if (details [1] == "ch2") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH2 = "0";}
								if (details [1] == "ch3") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH3 = "0";}
								if (details [1] == "ch4") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH4 = "0";}
								if (details [1] == "ch5") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH5 = "0";}
								if (details [1] == "ch6") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH6 = "0";}
								//activeChs.Clear ();//NOT WORKING
								//activeChs.Remove(data);
							}
						}
					}
				}
				//if this vEF changes from current tileHit to another
				if (lastTileHit != tileHit) {
					//change unit channel properties to 0 and clear local active list (ONLY ONCE SEND OFF TYPE COMMANDS)
					foreach (string data in activeChs) {
						//reminder data format is unitID,ch#,chType,tileAssignment
						string[] details = data.Split (',');
						foreach (string uID in dataRef.parser.allUnitObjects.Keys) {
							if (uID == details [0]) {//if the unitID matches unitID
								if (details [1] == "ch1") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH1 = "0";}
								if (details [1] == "ch2") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH2 = "0";}
								if (details [1] == "ch3") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH3 = "0";}
								if (details [1] == "ch4") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH4 = "0";}
								if (details [1] == "ch5") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH5 = "0";}
								if (details [1] == "ch6") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH6 = "0";}
								//activeChs.Clear ();//NOT WORKING
								//activeChs.Remove(data);
							}
						}
					}
				}
				lastTileHit = tileHit;
			}

			//RANGED DIRECTIONAL FX DEBUG
			//RANGED DIRECTIONAL FX DEBUG
			//RANGED DIRECTIONAL FX DEBUG
			//RANGED DIRECTIONAL FX DEBUG
			//RANGED DIRECTIONAL FX DEBUG
			//RANGED DIRECTIONAL FX DEBUG
			if (cyPIPESLogActive) {
				//Calculate Endpoint
				Vector3 rangeCheck = dataRef.userPos - gameObject.transform.position;
				Vector3 direction = gameObject.transform.forward;
				Vector3 endPoint = (Vector3.zero + (rangeCheck.normalized * range));
				Vector3 directionEndPoint = (Vector3.zero + (direction.normalized * range));
				Debug.DrawRay (gameObject.transform.position, endPoint, Color.gray, 0.11f);
				Debug.DrawRay (gameObject.transform.position, directionEndPoint, Color.black, 0.11f);
			}
		}//End of Ranged Directional FX Type

		//DIRECTIONAL FX ON
		//DIRECTIONAL FX ON
		//DIRECTIONAL FX ON
		//DIRECTIONAL FX ON
		//DIRECTIONAL FX ON
		//DIRECTIONAL FX ON
		if (otherEffectType == FXtype.directionalEffect) {
			//If user is in active space
			if (dataRef.userActive) {
				//Ditermine which tile is active for this effect (This is too basic. In the future you need analyze where on the tile it hits and include nearest 1 or two tiles for more of a gradient effect) Corners of the tile cude cause switching instead of gradient of effects
				Vector3 thisFXdirection = gameObject.transform.forward;//EF direction
				Vector3 userToEFdir = gameObject.transform.position - dataRef.userPos;//user to vEF direction
				Vector3 endPoint = (userToEFdir + (gameObject.transform.forward.normalized * 1000));//endpoint direction from user
				RaycastHit fxhit;
				//Raycasting from user to directional endpoint 1000 range
				Physics.Raycast (dataRef.userPos, endPoint, out fxhit, 10.0f);
				if (cyPIPESErrorsActive) {Debug.DrawRay (dataRef.userPos, endPoint, Color.magenta, 0.11f);}//Magenta raycast annotation from user to directional endpoint
				string directionalTile = null;
				try{
					Debug.Log(fxhit.transform.gameObject.name);
					if(fxhit.collider != null){
						directionalTile = oppositeTile(fxhit.transform.gameObject.name);
					}
				}catch{
					if (fxhit.collider != null) {
						if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: Tile hit name is unrecognized for use in " + gameObject.name + " as " + fxhit.transform.gameObject.name);}
					}
				}

				//If the directional tile is not null
				if (directionalTile != null) {
					//raycast to user and record what is hit in an array
					RaycastHit[] hits;
					Vector3 direction = dataRef.userPos - gameObject.transform.position;
					hits = Physics.RaycastAll (transform.position, direction, 5000);
					if (hits.Length > 0) {
						//if first object hit is within the cyPIPES layer
						float[] hitDistance = new float[hits.Length];
						int i = 0;
						foreach (RaycastHit hit in hits) {//sync hit distances with hits
							hitDistance [i] = Vector3.Distance (hit.transform.position, transform.position);
							i++;
						}
						int j = 0;
						foreach (float dist in hitDistance) {//find shortest hit disance object
							if (dist == Mathf.Min (hitDistance)) {//be sure that the closet object is a cyPIPES layer object 
								if (hits [j].transform.gameObject.layer == dataRef.cyPIPESlayer) {
									closestHit = hits [j].transform.gameObject;//reference which tile it is
									//Debug.Log ("HIT DETECTED ON " + closestHit.name);
								}
							}
							j++;
						}
						//check through tile record for matches in FX types
						//Normalize tileHit name
						try {
							tileHit = directionalTile;
						} catch {
							if (cyPIPESErrorsActive) {
								Debug.LogError ("cyPIPES ERROR: Tile hit name is unrecognized for use in " + gameObject.name);
							}
						}
						//Debug.Log ("CURRENT TILE HIT = " + tileHit);
						if (tileHit != "") {	
							foreach (string data in dataRef.tileRecord.Keys) {
								//if FX type is matched on tile change unit channel propetries to intesnsity level and record unitID,ch in local active list (EVERY FRAME SEND ON TYPE COMMANDS)
								//reminder data format is unitID,ch#,chType,tileAssignment
								string[] details = data.Split (',');
								if (details [2] == "Other") {//its an Other channel
									if (details [3] == directionalTile) {//if record includes the directional tile
										foreach (string uID in dataRef.parser.allUnitObjects.Keys) {//lets communicate the ON command now
											if (uID == details [0]) {//if the unitID matches unitID
												//change parameter in the correct channel
												if (details [1] == "ch1") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH1 = customStringCommand;}
												if (details [1] == "ch2") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH2 = customStringCommand;}
												if (details [1] == "ch3") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH3 = customStringCommand;}
												if (details [1] == "ch4") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH4 = customStringCommand;}
												if (details [1] == "ch5") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH5 = customStringCommand;}
												if (details [1] == "ch6") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH6 = customStringCommand;}
												//Assign data to activeChs[]
												if(!activeChs.Contains(data)){
													activeChs.Add (data);
												}
												//Debug.Log ("DATA FROM vEF BEING UTILZED " + data);
											}
										}
									}
								}
							}

						} else {
							if (cyPIPESErrorsActive) {
								Debug.LogError ("cyPIPES ERROR: vEF raycast is hitting " + closestHit.name + " which is in cyPIPES layer, but not a tile.");
							}
						}
					}
				}
			}
			//DIRECTIONAL FX OFF
			//DIRECTIONAL FX OFF
			//DIRECTIONAL FX OFF
			//DIRECTIONAL FX OFF
			//DIRECTIONAL FX OFF
			//DIRECTIONAL FX OFF
			//if local activeChs list is not empty
			if (activeChs.Count > 0) {
				//If user is not in active space OFF condition
				if (dataRef.userActive == false) {
					//shut down all channels in active list and clear list (ONLY ONCE SEND OFF TYPE COMMANDS)
					foreach (string data in activeChs) {
						//reminder data format is unitID,ch#,chType,tileAssignment
						string[] details = data.Split (',');
						foreach (string uID in dataRef.parser.allUnitObjects.Keys) {
							if (uID == details [0]) {//if the unitID matches unitID record entry
								if (details [1] == "ch1") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH1 = "0";}
								if (details [1] == "ch2") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH2 = "0";}
								if (details [1] == "ch3") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH3 = "0";}
								if (details [1] == "ch4") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH4 = "0";}
								if (details [1] == "ch5") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH5 = "0";}
								if (details [1] == "ch6") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH6 = "0";}
								//activeChs.Clear ();//NOT WORKING
								//activeChs.Remove(data);
							}
						}
					}
				}
				//if this vEF changes from current tileHit to another
				if (lastTileHit != tileHit) {
					//change unit channel properties to 0 and clear local active list (ONLY ONCE SEND OFF TYPE COMMANDS)
					foreach (string data in activeChs) {
						//reminder data format is unitID,ch#,chType,tileAssignment
						string[] details = data.Split (',');
						foreach (string uID in dataRef.parser.allUnitObjects.Keys) {
							if (uID == details [0]) {//if the unitID matches unitID
								if (details [1] == "ch1") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH1 = "0";}
								if (details [1] == "ch2") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH2 = "0";}
								if (details [1] == "ch3") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH3 = "0";}
								if (details [1] == "ch4") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH4 = "0";}
								if (details [1] == "ch5") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH5 = "0";}
								if (details [1] == "ch6") {dataRef.parser.allUnitObjects [uID].GetComponent<cyPIPES> ().customCommandCH6 = "0";}
								//activeChs.Clear ();//NOT WORKING
								//activeChs.Remove(data);
							}
						}
					}
				}
				lastTileHit = tileHit;
			}

			//DIRECTIONAL FX DEBUG
			//DIRECTIONAL FX DEBUG
			//DIRECTIONAL FX DEBUG
			//DIRECTIONAL FX DEBUG
			//DIRECTIONAL FX DEBUG
			//DIRECTIONAL FX DEBUG
			if (cyPIPESLogActive) {
				//Calculate Endpoint
				Vector3 rangeCheck = dataRef.userPos - gameObject.transform.position;
				Vector3 direction = gameObject.transform.forward;
				Vector3 endPoint = (Vector3.zero + (rangeCheck.normalized * 1000));
				Vector3 directionEndPoint = (Vector3.zero + (direction.normalized * 1000));
				Debug.DrawRay (gameObject.transform.position, endPoint, Color.gray, 0.11f);
				Debug.DrawRay (gameObject.transform.position, directionEndPoint, Color.black, 0.11f);
			}
		}//End of Ranged Directional FX Type
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color(1, 0, 0, 0.5F);
		Gizmos.DrawIcon (transform.position, "Other.png", true);
	}

	string oppositeTile (string tile){
		foreach (string tileName in tileNames) {
			if (tile == tileNames[0] || tile == tileNames[1]) {
				Debug.Log ("OPPOSITE TILE FOR " + gameObject.name + " IS " + "frontTile");
				return("frontTile");
			}
			if (tile == tileNames [2] || tile == tileNames [3]) {
				Debug.Log ("OPPOSITE TILE FOR " + gameObject.name + " IS " + "rightTile");
				return("rightTile");
			}
			if (tile == tileNames [4] || tile == tileNames [5]) {
				Debug.Log ("OPPOSITE TILE FOR " + gameObject.name + " IS " + "backTile");
				return("backTile");
			}
			if (tile == tileNames [6] || tile == tileNames [7]) {
				Debug.Log ("OPPOSITE TILE FOR " + gameObject.name + " IS " + "leftTile");
				return("leftTile");
			}
			if (tile == tileNames [8] || tile == tileNames [9]) {
				Debug.Log ("OPPOSITE TILE FOR " + gameObject.name + " IS " + "bottomTile");
				return("bottomTile");
			}
			if (tile == tileNames [10] || tile == tileNames [11]) {
				Debug.Log ("OPPOSITE TILE FOR " + gameObject.name + " IS " + "topTile");
				return("topTile");
			}
		}
		Debug.Log ("OPPOSITE TILE FOR " + gameObject.name + " IS " + "NULL");
		return null;
	}

	string normalizeTileName(string tile){

		foreach (string tileName in tileNames) {
			if (tile == tileNames[0] || tile == tileNames[1]) {
				return("backTile");
			}
			if (tile == tileNames [2] || tile == tileNames [3]) {
				return("leftTile");
			}
			if (tile == tileNames [4] || tile == tileNames [5]) {
				return("frontTile");
			}
			if (tile == tileNames [6] || tile == tileNames [7]) {
				return("rightTile");
			}
			if (tile == tileNames [8] || tile == tileNames [9]) {
				return("topTile");
			}
			if (tile == tileNames [10] || tile == tileNames [11]) {
				return("bottomTile");
			}
		}

		return null;

	}

}
