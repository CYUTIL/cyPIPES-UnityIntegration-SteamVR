using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using Valve.VR;

public class cyPIPESparsePost : MonoBehaviour {

	[Header("cyPIPES Logs and Errors Options")]
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Logs.")]
	public bool cyPIPESLogActive;
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Errors.")]
	public bool cyPIPESErrorsActive;

	[Tooltip("This is usually for the Vive [CameraRig] object, but if you are using a different Vive prefab place the playspace in this field.")]
	public GameObject vrPlaySpaceObj;
	[HideInInspector]
	public cyPIPESConfigs configs;
	[HideInInspector]
	public cyPIPEStileNormalization vEFDswitch;
	[Header("cyPIPES Sensors and Unit prefabs")]
	[Tooltip("Please leave this set unless you are replacing with a custom virtual sensor prefab.")]
	public GameObject windSensorPrefab;
	[Tooltip("Please leave this set unless you are replacing with a custom virtual sensor prefab.")]
	public GameObject heatSensorPrefab;
	[Tooltip("Please leave this set unless you are replacing with a custom virtual sensor prefab.")]
	public GameObject otherSensorPrefab;
	[Tooltip("Please leave this set unless you are replacing with a custom virtual cyPIPES unit prefab.")]
	public GameObject unitObjectPrefab;
	public Dictionary<string,GameObject> allCyPIPESsensors = new Dictionary<string,GameObject>();
	//[HideInInspector]
	public Dictionary<string,GameObject> allUnitObjects = new Dictionary<string,GameObject>();
		
	public void parsePost(){
		//parse all unit configs and post vEFDsensors around user
		if (configs.allCyPIPESunits.Count > 0) {
			foreach (string key in configs.allCyPIPESunits.Keys) {
				//Parse each unit
				if (configs.allCyPIPESunits[key].ID != ""){
					//Instatiating unit as a GameObject instead of Object
					GameObject unit = Instantiate (unitObjectPrefab, Vector3.zero, Quaternion.identity);//instatiate unit
					unit.name = configs.allCyPIPESunits[key].ID;//rename unit to ID number.  This helps unit reference it's own data config parameters.
					unit.transform.parent = gameObject.transform; //set parent
					allUnitObjects.Add(configs.allCyPIPESunits[key].ID,unit);//Save unit to allUnitObjects list <GameObjects>
					setConfigParams(configs.allCyPIPESunits[key].ID);//Set actual config data to unit
				}

				//Parse each channel from each unit
				//Ch1
				if (configs.allCyPIPESunits [key].ch1Type == "Fan") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch1posX, configs.allCyPIPESunits [key].ch1posY, configs.allCyPIPESunits [key].ch1posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch1rotX, configs.allCyPIPESunits [key].ch1rotY, configs.allCyPIPESunits [key].ch1rotZ);
					GameObject newObj = Instantiate (windSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch1scalX, configs.allCyPIPESunits [key].ch1scalY, configs.allCyPIPESunits [key].ch1scalZ);
					allCyPIPESsensors.Add (key+","+"ch1",newObj);
				}
				if (configs.allCyPIPESunits [key].ch1Type == "Heat") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch1posX, configs.allCyPIPESunits [key].ch1posY, configs.allCyPIPESunits [key].ch1posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch1rotX, configs.allCyPIPESunits [key].ch1rotY, configs.allCyPIPESunits [key].ch1rotZ);
					GameObject newObj = Instantiate (heatSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch1scalX, configs.allCyPIPESunits [key].ch1scalY, configs.allCyPIPESunits [key].ch1scalZ);
					allCyPIPESsensors.Add (key+","+"ch1",newObj);
				}
				if (configs.allCyPIPESunits [key].ch1Type == "Other") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch1posX, configs.allCyPIPESunits [key].ch1posY, configs.allCyPIPESunits [key].ch1posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch1rotX, configs.allCyPIPESunits [key].ch1rotY, configs.allCyPIPESunits [key].ch1rotZ);
					GameObject newObj = Instantiate (otherSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch1scalX, configs.allCyPIPESunits [key].ch1scalY, configs.allCyPIPESunits [key].ch1scalZ);
					allCyPIPESsensors.Add (key+","+"ch1",newObj);
				}
				//Ch2
				if (configs.allCyPIPESunits [key].ch2Type == "Fan") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch2posX, configs.allCyPIPESunits [key].ch2posY, configs.allCyPIPESunits [key].ch2posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch2rotX, configs.allCyPIPESunits [key].ch2rotY, configs.allCyPIPESunits [key].ch2rotZ);
					GameObject newObj = Instantiate (windSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch1scalX, configs.allCyPIPESunits [key].ch1scalY, configs.allCyPIPESunits [key].ch1scalZ);
					allCyPIPESsensors.Add (key+","+"ch2",newObj);
				}
				if (configs.allCyPIPESunits [key].ch2Type == "Heat") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch2posX, configs.allCyPIPESunits [key].ch2posY, configs.allCyPIPESunits [key].ch2posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch2rotX, configs.allCyPIPESunits [key].ch2rotY, configs.allCyPIPESunits [key].ch2rotZ);
					GameObject newObj = Instantiate (heatSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch2scalX, configs.allCyPIPESunits [key].ch2scalY, configs.allCyPIPESunits [key].ch2scalZ);
					allCyPIPESsensors.Add (key+","+"ch2",newObj);
				}
				if (configs.allCyPIPESunits [key].ch2Type == "Other") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch2posX, configs.allCyPIPESunits [key].ch2posY, configs.allCyPIPESunits [key].ch2posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch2rotX, configs.allCyPIPESunits [key].ch2rotY, configs.allCyPIPESunits [key].ch2rotZ);
					GameObject newObj = Instantiate (otherSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch2scalX, configs.allCyPIPESunits [key].ch2scalY, configs.allCyPIPESunits [key].ch2scalZ);
					allCyPIPESsensors.Add (key+","+"ch2",newObj);
				}
				//Ch3
				if (configs.allCyPIPESunits [key].ch3Type == "Fan") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch3posX, configs.allCyPIPESunits [key].ch3posY, configs.allCyPIPESunits [key].ch3posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch3rotX, configs.allCyPIPESunits [key].ch3rotY, configs.allCyPIPESunits [key].ch3rotZ);
					GameObject newObj = Instantiate (windSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch3scalX, configs.allCyPIPESunits [key].ch3scalY, configs.allCyPIPESunits [key].ch3scalZ);
					allCyPIPESsensors.Add (key+","+"ch3",newObj);
				}
				if (configs.allCyPIPESunits [key].ch3Type == "Heat") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch3posX, configs.allCyPIPESunits [key].ch3posY, configs.allCyPIPESunits [key].ch3posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch3rotX, configs.allCyPIPESunits [key].ch3rotY, configs.allCyPIPESunits [key].ch3rotZ);
					GameObject newObj = Instantiate (heatSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch3scalX, configs.allCyPIPESunits [key].ch3scalY, configs.allCyPIPESunits [key].ch3scalZ);
					allCyPIPESsensors.Add (key+","+"ch3",newObj);
				}
				if (configs.allCyPIPESunits [key].ch3Type == "Other") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch3posX, configs.allCyPIPESunits [key].ch3posY, configs.allCyPIPESunits [key].ch3posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch3rotX, configs.allCyPIPESunits [key].ch3rotY, configs.allCyPIPESunits [key].ch3rotZ);
					GameObject newObj = Instantiate (otherSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch3scalX, configs.allCyPIPESunits [key].ch3scalY, configs.allCyPIPESunits [key].ch3scalZ);
					allCyPIPESsensors.Add (key+","+"ch3",newObj);
				}
				//Ch4
				if (configs.allCyPIPESunits [key].ch4Type == "Fan") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch4posX, configs.allCyPIPESunits [key].ch4posY, configs.allCyPIPESunits [key].ch4posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch4rotX, configs.allCyPIPESunits [key].ch4rotY, configs.allCyPIPESunits [key].ch4rotZ);
					GameObject newObj = Instantiate (windSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch4scalX, configs.allCyPIPESunits [key].ch4scalY, configs.allCyPIPESunits [key].ch4scalZ);
					allCyPIPESsensors.Add (key+","+"ch4",newObj);
				}
				if (configs.allCyPIPESunits [key].ch4Type == "Heat") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch4posX, configs.allCyPIPESunits [key].ch4posY, configs.allCyPIPESunits [key].ch4posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch4rotX, configs.allCyPIPESunits [key].ch4rotY, configs.allCyPIPESunits [key].ch4rotZ);
					GameObject newObj = Instantiate (heatSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch4scalX, configs.allCyPIPESunits [key].ch4scalY, configs.allCyPIPESunits [key].ch4scalZ);
					allCyPIPESsensors.Add (key+","+"ch4",newObj);
				}
				if (configs.allCyPIPESunits [key].ch4Type == "Other") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch4posX, configs.allCyPIPESunits [key].ch4posY, configs.allCyPIPESunits [key].ch4posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch4rotX, configs.allCyPIPESunits [key].ch4rotY, configs.allCyPIPESunits [key].ch4rotZ);
					GameObject newObj = Instantiate (otherSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch4scalX, configs.allCyPIPESunits [key].ch4scalY, configs.allCyPIPESunits [key].ch4scalZ);
					allCyPIPESsensors.Add (key+","+"ch4",newObj);
				}
				//Ch5
				if (configs.allCyPIPESunits [key].ch5Type == "Fan") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch5posX, configs.allCyPIPESunits [key].ch5posY, configs.allCyPIPESunits [key].ch5posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch5rotX, configs.allCyPIPESunits [key].ch5rotY, configs.allCyPIPESunits [key].ch5rotZ);
					GameObject newObj = Instantiate (windSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch5scalX, configs.allCyPIPESunits [key].ch5scalY, configs.allCyPIPESunits [key].ch5scalZ);
					allCyPIPESsensors.Add (key+","+"ch5",newObj);
				}
				if (configs.allCyPIPESunits [key].ch5Type == "Heat") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch5posX, configs.allCyPIPESunits [key].ch5posY, configs.allCyPIPESunits [key].ch5posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch5rotX, configs.allCyPIPESunits [key].ch5rotY, configs.allCyPIPESunits [key].ch5rotZ);
					GameObject newObj = Instantiate (heatSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch5scalX, configs.allCyPIPESunits [key].ch5scalY, configs.allCyPIPESunits [key].ch5scalZ);
					allCyPIPESsensors.Add (key+","+"ch5",newObj);
				}
				if (configs.allCyPIPESunits [key].ch5Type == "Other") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch5posX, configs.allCyPIPESunits [key].ch5posY, configs.allCyPIPESunits [key].ch5posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch5rotX, configs.allCyPIPESunits [key].ch5rotY, configs.allCyPIPESunits [key].ch5rotZ);
					GameObject newObj = Instantiate (otherSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch5scalX, configs.allCyPIPESunits [key].ch5scalY, configs.allCyPIPESunits [key].ch5scalZ);
					allCyPIPESsensors.Add (key+","+"ch5",newObj);
				}
				//Ch6
				if (configs.allCyPIPESunits [key].ch6Type == "Fan") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch6posX, configs.allCyPIPESunits [key].ch6posY, configs.allCyPIPESunits [key].ch6posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch6rotX, configs.allCyPIPESunits [key].ch6rotY, configs.allCyPIPESunits [key].ch6rotZ);
					GameObject newObj = Instantiate (windSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch6scalX, configs.allCyPIPESunits [key].ch6scalY, configs.allCyPIPESunits [key].ch6scalZ);
					allCyPIPESsensors.Add (key+","+"ch6",newObj);
				}
				if (configs.allCyPIPESunits [key].ch6Type == "Heat") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch6posX, configs.allCyPIPESunits [key].ch6posY, configs.allCyPIPESunits [key].ch6posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch6rotX, configs.allCyPIPESunits [key].ch6rotY, configs.allCyPIPESunits [key].ch6rotZ);
					GameObject newObj = Instantiate (heatSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch6scalX, configs.allCyPIPESunits [key].ch6scalY, configs.allCyPIPESunits [key].ch6scalZ);
					allCyPIPESsensors.Add (key+","+"ch6",newObj);
				}
				if (configs.allCyPIPESunits [key].ch6Type == "Other") {
					Vector3 objPos = new Vector3 (configs.allCyPIPESunits [key].ch6posX, configs.allCyPIPESunits [key].ch6posY, configs.allCyPIPESunits [key].ch6posZ);
					Vector3 objRot = new Vector3 (configs.allCyPIPESunits [key].ch6rotX, configs.allCyPIPESunits [key].ch6rotY, configs.allCyPIPESunits [key].ch6rotZ);
					GameObject newObj = Instantiate (otherSensorPrefab, objPos, Quaternion.identity);
					newObj.transform.eulerAngles = objRot;
					newObj.transform.SetParent (gameObject.transform);
					//newObj.transform.parent = gameObject.transform;
					//newObj.transform.localScale = new Vector3 (configs.allCyPIPESunits [key].ch6scalX, configs.allCyPIPESunits [key].ch6scalY, configs.allCyPIPESunits [key].ch6scalZ);
					allCyPIPESsensors.Add (key+","+"ch6",newObj);
				}
			}
			//reference user's play space object attach cyPIPES as a child along with all the virtual sensors
			if (VRSettings.enabled) {
				if (vrPlaySpaceObj != null) {
					gameObject.transform.localPosition = vrPlaySpaceObj.transform.localPosition;
					gameObject.transform.parent = vrPlaySpaceObj.transform;
					//gameObject.transform.localPosition = Vector3.zero;
					//Signal to tile normalization that all vEFDs are placed and cyPIPES is a child of tracked space.
					vEFDswitch.vEFDparse = true;
				} else {
					if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: Null reference to a vrPlaySpaceObj object. The [CameraRig] prefab is usually referenced here.");}
				}
			} else {
				if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: No VR tracked space detected.");}
			}
		}
	}

	void setConfigParams (string unitKey){
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ID = configs.allCyPIPESunits [unitKey].ID;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.IP = configs.allCyPIPESunits [unitKey].IP;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch1Type = configs.allCyPIPESunits [unitKey].ch1Type;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch1posX = configs.allCyPIPESunits [unitKey].ch1posX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch1posY = configs.allCyPIPESunits [unitKey].ch1posY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch1posZ = configs.allCyPIPESunits [unitKey].ch1posZ;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch1rotX = configs.allCyPIPESunits [unitKey].ch1rotX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch1rotY = configs.allCyPIPESunits [unitKey].ch1rotY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch1rotZ = configs.allCyPIPESunits [unitKey].ch1rotZ;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch1scalX = configs.allCyPIPESunits [unitKey].ch1scalX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch1scalY = configs.allCyPIPESunits [unitKey].ch1scalY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch1scalZ = configs.allCyPIPESunits [unitKey].ch1scalZ;

		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch2Type = configs.allCyPIPESunits [unitKey].ch2Type;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch2posX = configs.allCyPIPESunits [unitKey].ch2posX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch2posY = configs.allCyPIPESunits [unitKey].ch2posY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch2posZ = configs.allCyPIPESunits [unitKey].ch2posZ;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch2rotX = configs.allCyPIPESunits [unitKey].ch2rotX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch2rotY = configs.allCyPIPESunits [unitKey].ch2rotY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch2rotZ = configs.allCyPIPESunits [unitKey].ch2rotZ;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch2scalX = configs.allCyPIPESunits [unitKey].ch2scalX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch2scalY = configs.allCyPIPESunits [unitKey].ch2scalY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch2scalZ = configs.allCyPIPESunits [unitKey].ch2scalZ;

		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch3Type = configs.allCyPIPESunits [unitKey].ch3Type;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch3posX = configs.allCyPIPESunits [unitKey].ch3posX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch3posY = configs.allCyPIPESunits [unitKey].ch3posY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch3posZ = configs.allCyPIPESunits [unitKey].ch3posZ;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch3rotX = configs.allCyPIPESunits [unitKey].ch3rotX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch3rotY = configs.allCyPIPESunits [unitKey].ch3rotY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch3rotZ = configs.allCyPIPESunits [unitKey].ch3rotZ;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch3scalX = configs.allCyPIPESunits [unitKey].ch3scalX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch3scalY = configs.allCyPIPESunits [unitKey].ch3scalY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch3scalZ = configs.allCyPIPESunits [unitKey].ch3scalZ;

		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch4Type = configs.allCyPIPESunits [unitKey].ch4Type;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch4posX = configs.allCyPIPESunits [unitKey].ch4posX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch4posY = configs.allCyPIPESunits [unitKey].ch4posY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch4posZ = configs.allCyPIPESunits [unitKey].ch4posZ;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch4rotX = configs.allCyPIPESunits [unitKey].ch4rotX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch4rotY = configs.allCyPIPESunits [unitKey].ch4rotY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch4rotZ = configs.allCyPIPESunits [unitKey].ch4rotZ;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch4scalX = configs.allCyPIPESunits [unitKey].ch4scalX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch4scalY = configs.allCyPIPESunits [unitKey].ch4scalY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch4scalZ = configs.allCyPIPESunits [unitKey].ch4scalZ;

		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch5Type = configs.allCyPIPESunits [unitKey].ch5Type;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch5posX = configs.allCyPIPESunits [unitKey].ch5posX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch5posY = configs.allCyPIPESunits [unitKey].ch5posY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch5posZ = configs.allCyPIPESunits [unitKey].ch5posZ;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch5rotX = configs.allCyPIPESunits [unitKey].ch5rotX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch5rotY = configs.allCyPIPESunits [unitKey].ch5rotY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch5rotZ = configs.allCyPIPESunits [unitKey].ch5rotZ;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch5scalX = configs.allCyPIPESunits [unitKey].ch5scalX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch5scalY = configs.allCyPIPESunits [unitKey].ch5scalY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch5scalZ = configs.allCyPIPESunits [unitKey].ch5scalZ;

		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch6Type = configs.allCyPIPESunits [unitKey].ch6Type;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch6posX = configs.allCyPIPESunits [unitKey].ch6posX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch6posY = configs.allCyPIPESunits [unitKey].ch6posY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch6posZ = configs.allCyPIPESunits [unitKey].ch6posZ;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch6rotX = configs.allCyPIPESunits [unitKey].ch6rotX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch6rotY = configs.allCyPIPESunits [unitKey].ch6rotY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch6rotZ = configs.allCyPIPESunits [unitKey].ch6rotZ;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch6scalX = configs.allCyPIPESunits [unitKey].ch6scalX;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch6scalY = configs.allCyPIPESunits [unitKey].ch6scalY;
		allUnitObjects [unitKey].GetComponent<cyPIPES> ().unitData.ch6scalZ = configs.allCyPIPESunits [unitKey].ch6scalZ;
	}

}
