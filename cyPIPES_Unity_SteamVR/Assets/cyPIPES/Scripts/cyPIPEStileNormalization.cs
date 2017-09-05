using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
//
public class cyPIPEStileNormalization : MonoBehaviour {

	[Header("cyPIPES Logs and Errors Options")]
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Logs.")]
	public bool cyPIPESLogActive;
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Errors.")]
	public bool cyPIPESErrorsActive;

	//To be used in tile size calculations
	[Tooltip("Set this to your ceiling height in meters if not 8ft. It is set to an 8ft ceiling by default")]
	public float volumeHeight = 2.44f;
	public Dictionary <string,GameObject> inwardTiles = new Dictionary <string,GameObject>();
	public Dictionary <string,GameObject> outwardTiles = new Dictionary <string,GameObject>();
	[HideInInspector]
	public bool vEFDparse = false;
	bool tilesParsed = false;
	[HideInInspector]
	public bool vEFDtileRecordComplete;
	[HideInInspector]
	public cyPIPESConfigs allConfigs;
	[HideInInspector]
	public cyPIPESparsePost parser;
	Vector3 centerOfTrackedSpace;
	[HideInInspector]
	public Dictionary<string,GameObject> vEFDtileRecord = new Dictionary<string,GameObject>();
	int cyPIPESlayer;

	//OpenVR: Set how room bounds are calibrated. Hidden from inspector for now because only Calibrated state is supported
	public enum Size
	{
		Calibrated,
		_400x300,
		_300x225,
		_200x150
	}
	public Size size;
	[HideInInspector]
	public Vector3[] vertices;

	void Start () {
		cyPIPESlayer = LayerMask.NameToLayer ("cyPIPES");
		if (cyPIPESLogActive) {Debug.Log ("cyPIPES LAYER FOUND AT " + cyPIPESlayer);}

	}
		
	void Update () {

		if (vEFDparse == true && tilesParsed == true) {
			//run vEFD assignment function once
			vEFDassignments();
		}
	}

	//OpenVR: Collecting tracked space bounding references
	static bool GetBounds( Size size, ref HmdQuad_t pRect )
	{
		if (size == Size.Calibrated)
		{
			var initOpenVR = (!SteamVR.active && !SteamVR.usingNativeSupport);
			if (initOpenVR)
			{
				var error = EVRInitError.None;
				OpenVR.Init(ref error, EVRApplicationType.VRApplication_Other);
			}

			var chaperone = OpenVR.Chaperone;
			bool success = (chaperone != null) && chaperone.GetPlayAreaRect(ref pRect);
			if (!success)
				Debug.LogWarning("Failed to get Calibrated Play Area bounds! Make sure you have tracking first, and that your space is calibrated.");

			if (initOpenVR)
				OpenVR.Shutdown();

			return success;
		}
		else
		{
			try
			{
				var str = size.ToString().Substring(1);
				var arr = str.Split(new char[] {'x'}, 2);

				// convert to half size in meters (from cm)
				var x = float.Parse(arr[0]) / 200;
				var z = float.Parse(arr[1]) / 200;

				pRect.vCorners0.v0 =  x;
				pRect.vCorners0.v1 =  0;
				pRect.vCorners0.v2 =  z;

				pRect.vCorners1.v0 =  x;
				pRect.vCorners1.v1 =  0;
				pRect.vCorners1.v2 = -z;

				pRect.vCorners2.v0 = -x;
				pRect.vCorners2.v1 =  0;
				pRect.vCorners2.v2 = -z;

				pRect.vCorners3.v0 = -x;
				pRect.vCorners3.v1 =  0;
				pRect.vCorners3.v2 =  z;

				return true;
			}
			catch {}
		}

		return false;
	}

	//Update tiles at enable and if developer toggles this script on & off
	public void OnEnable()
	{
		if (Application.isPlaying)
		{
			// No need to remain enabled at runtime.
			// Anyone that wants to change properties at runtime
			// should call BuildMesh themselves.
			//enabled = false;

			// If we want the configured bounds of the user,
			// we need to wait for tracking.
			if (size == Size.Calibrated)
				StartCoroutine("UpdateBounds");

		}
	}

	//Coroutine to gather calibrated tracked space when tracking syetem is ready
	IEnumerator UpdateBounds()
	{
		var chaperone = OpenVR.Chaperone;
		if (chaperone == null)
			yield break;

		while (chaperone.GetCalibrationState() != ChaperoneCalibrationState.OK)
			yield return null;

		//Record the initial transfrom of vrPlaySpace
		Vector3 userPos = parser.vrPlaySpaceObj.transform.position;
		Vector3 userRot = parser.vrPlaySpaceObj.transform.eulerAngles;
		Vector3 userScl = parser.vrPlaySpaceObj.transform.localScale;
		//Reset the transfor of vrPlayspace to origin
		parser.vrPlaySpaceObj.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
		parser.vrPlaySpaceObj.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
		parser.vrPlaySpaceObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		//construct cyPIPES tiles
		BuildTiles();
		//Return the vrPlayspace back to it's original transfrom
		parser.vrPlaySpaceObj.transform.position = userPos;
		parser.vrPlaySpaceObj.transform.eulerAngles = userRot;
		parser.vrPlaySpaceObj.transform.localScale = userScl;
	}

	//Construct the cyPIPES tiles
	public void BuildTiles(){

		//OpenVR: Gather actual corners of floor room setup
		var rect = new HmdQuad_t ();
		if (!GetBounds (size, ref rect))
			return;
		var corners = new HmdVector3_t[] { rect.vCorners0, rect.vCorners1, rect.vCorners2, rect.vCorners3 };
		vertices = new Vector3[corners.Length * 2];
		for (int i = 0; i < corners.Length; i++) {
			var c = corners [i];
			vertices [i] = new Vector3 (c.v0, 0.01f, c.v2);
		}

		//Construct new tiles
		if (vertices.Length > 2) {
			//destroy any previous tiles if they exsist
			if (inwardTiles.Count > 2) {
				foreach (string key in inwardTiles.Keys) {
					Destroy (inwardTiles [key]);
				}
				foreach (string key in outwardTiles.Keys) {
					Destroy (outwardTiles [key]);
				}
			}
			
			//Calculate vertices (just the 8 for now)
			List<Vector3> spaceVerts = new List<Vector3> ();
			spaceVerts.Add (vertices [0]);//back bottom right
			spaceVerts.Add (vertices [1]);//back bottom left
			spaceVerts.Add (vertices [2]);//front bottom left
			spaceVerts.Add (vertices [3]);//front bottom right
			spaceVerts.Add (new Vector3 (vertices [0].x, vertices [0].y + volumeHeight, vertices [0].z));//top bottom right
			spaceVerts.Add (new Vector3 (vertices [1].x, vertices [1].y + volumeHeight, vertices [1].z));//top bottom left
			spaceVerts.Add (new Vector3 (vertices [2].x, vertices [2].y + volumeHeight, vertices [2].z));//top bottom left
			spaceVerts.Add (new Vector3 (vertices [3].x, vertices [3].y + volumeHeight, vertices [3].z));//top bottom right

			//Calulate tile sizes
			//For now this will be simply 6 sided and in this order bk,lft,ft,rht,top,btm
			List<float> widths = new List<float> ();
			List<float> heights = new List<float> ();
			widths.Add (Vector3.Distance (spaceVerts [0], spaceVerts [1]));//back
			heights.Add (Vector3.Distance (spaceVerts [0], spaceVerts [4]));//back
			widths.Add (Vector3.Distance (spaceVerts [1], spaceVerts [2]));//left
			heights.Add (Vector3.Distance (spaceVerts [1], spaceVerts [5]));//left
			widths.Add (Vector3.Distance (spaceVerts [2], spaceVerts [3]));//front
			heights.Add (Vector3.Distance (spaceVerts [2], spaceVerts [6]));//front
			widths.Add (Vector3.Distance (spaceVerts [3], spaceVerts [0]));//right
			heights.Add (Vector3.Distance (spaceVerts [3], spaceVerts [7]));//right
			widths.Add (Vector3.Distance (spaceVerts [4], spaceVerts [5]));//top
			heights.Add (Vector3.Distance (spaceVerts [4], spaceVerts [7]));//top
			widths.Add (Vector3.Distance (spaceVerts [0], spaceVerts [1]));//bottom
			heights.Add (Vector3.Distance (spaceVerts [0], spaceVerts [3]));//bottom

			//Calcuateing the center of tracked space
			centerOfTrackedSpace = spaceVerts [0];
			centerOfTrackedSpace = new Vector3 (centerOfTrackedSpace.x - widths [0] / 2, centerOfTrackedSpace.y, centerOfTrackedSpace.z);
			centerOfTrackedSpace = new Vector3 (centerOfTrackedSpace.x, centerOfTrackedSpace.y, centerOfTrackedSpace.z + heights [0] / 2);
			centerOfTrackedSpace = new Vector3 (centerOfTrackedSpace.x, centerOfTrackedSpace.y + volumeHeight / 2, centerOfTrackedSpace.z);
			//GameObject tester = cyPIPEStileBuild.CreateTile (0.2f, 0.2f, "tester", false);
			//tester.transform.parent = gameObject.transform;
			//tester.transform.position = centerOfTrackedSpace;

			//build tiles and place/oriente them and add to active tile list
			//back normal inward
			GameObject backTile = cyPIPEStileBuild.CreateTile (widths [0], heights [0], "backTile", true);
			backTile.transform.parent = gameObject.transform;
			inwardTiles.Add ("backTile", backTile);
			backTile.transform.position = new Vector3 (spaceVerts [0].x - widths [0], spaceVerts [0].y, spaceVerts [0].z);

				//back normal outward
				GameObject backTileOut = cyPIPEStileBuild.CreateTile (widths [0], heights [0], "backTileOut", true);
				backTileOut.transform.parent = gameObject.transform;
				outwardTiles.Add ("backTileOut", backTileOut);
				backTileOut.transform.position = new Vector3 (spaceVerts [0].x, spaceVerts [0].y, spaceVerts [0].z);
				backTileOut.transform.eulerAngles = new Vector3 (0.0f, 180.0f, 0.0f);

			//left normal inward
			GameObject leftTile = cyPIPEStileBuild.CreateTile (widths [1], heights [1], "leftTile", true);
			leftTile.transform.parent = gameObject.transform;
			inwardTiles.Add ("leftTile", leftTile);
			leftTile.transform.position = new Vector3 (spaceVerts [1].x, spaceVerts [1].y, spaceVerts [1].z + widths [1]);
			leftTile.transform.eulerAngles = new Vector3 (0.0f, 90.0f, 0.0f);

				//left normal outward
				GameObject leftTileOut = cyPIPEStileBuild.CreateTile (widths [1], heights [1], "leftTileOut", true);
				leftTileOut.transform.parent = gameObject.transform;
				outwardTiles.Add ("leftTileOut", leftTileOut);
				leftTileOut.transform.position = new Vector3 (spaceVerts [1].x, spaceVerts [1].y, spaceVerts [1].z);
				leftTileOut.transform.eulerAngles = new Vector3 (0.0f, -90.0f, 0.0f);

			//Front normal inward
			GameObject frontTile = cyPIPEStileBuild.CreateTile (widths [2], heights [2], "frontTile", true);
			frontTile.transform.parent = gameObject.transform;
			inwardTiles.Add ("frontTile", frontTile);
			frontTile.transform.position = new Vector3 (spaceVerts [2].x + widths [2], spaceVerts [2].y, spaceVerts [2].z);
			frontTile.transform.eulerAngles = new Vector3 (0.0f, 180.0f, 0.0f);

				//Front normal outward
				GameObject frontTileOut = cyPIPEStileBuild.CreateTile (widths [2], heights [2], "frontTileOut", true);
				frontTileOut.transform.parent = gameObject.transform;
				outwardTiles.Add ("frontTileOut", frontTileOut);
				frontTileOut.transform.position = new Vector3 (spaceVerts [2].x, spaceVerts [2].y, spaceVerts [2].z);
				frontTileOut.transform.eulerAngles = new Vector3 (0.0f, 0.0f, 0.0f);

			//Right normal inward
			GameObject rightTile = cyPIPEStileBuild.CreateTile (widths [3], heights [3], "rightTile", true);
			rightTile.transform.parent = gameObject.transform;
			inwardTiles.Add ("rightTile", rightTile);
			rightTile.transform.position = new Vector3 (spaceVerts [3].x, spaceVerts [3].y, spaceVerts [3].z - widths [3]);
			rightTile.transform.eulerAngles = new Vector3 (0.0f, -90.0f, 0.0f);

				//Right normal outward
				GameObject rightTileOut = cyPIPEStileBuild.CreateTile (widths [3], heights [3], "rightTileOut", true);
				rightTileOut.transform.parent = gameObject.transform;
				outwardTiles.Add ("rightTileOut", rightTileOut);
				rightTileOut.transform.position = new Vector3 (spaceVerts [3].x, spaceVerts [3].y, spaceVerts [3].z);
				rightTileOut.transform.eulerAngles = new Vector3 (0.0f, 90.0f, 0.0f);

			//top normal inward
			GameObject topTile = cyPIPEStileBuild.CreateTile (widths [4], heights [4], "topTile", true);
			topTile.transform.parent = gameObject.transform;
			inwardTiles.Add ("topTile", topTile);
			topTile.transform.position = new Vector3 (spaceVerts [4].x - widths [4], spaceVerts [4].y, spaceVerts [4].z);
			topTile.transform.eulerAngles = new Vector3 (90.0f, 0.0f, 0.0f);

				//top normal outward
				GameObject topTileOut = cyPIPEStileBuild.CreateTile (widths [4], heights [4], "topTileOut", true);
				topTileOut.transform.parent = gameObject.transform;
				outwardTiles.Add ("topTileOut", topTileOut);
				topTileOut.transform.position = new Vector3 (spaceVerts [4].x - widths [4], spaceVerts [4].y, spaceVerts [4].z + heights [4]);
				topTileOut.transform.eulerAngles = new Vector3 (-90.0f, 0.0f, 0.0f);

			//bottom normal inward
			GameObject bottomTile = cyPIPEStileBuild.CreateTile (widths [5], heights [5], "bottomTile", true);
			bottomTile.transform.parent = gameObject.transform;
			inwardTiles.Add ("bottomTile", bottomTile);
			bottomTile.transform.position = new Vector3 (spaceVerts [5].x, spaceVerts [5].y - volumeHeight, spaceVerts [5].z * -1);
			bottomTile.transform.eulerAngles = new Vector3 (-90.0f, 0.0f, 0.0f);

				//bottom normal outward
				GameObject bottomTileOut = cyPIPEStileBuild.CreateTile (widths [5], heights [5], "bottomTileOut", true);
				bottomTileOut.transform.parent = gameObject.transform;
				outwardTiles.Add ("bottomTileOut", bottomTileOut);
				bottomTileOut.transform.position = new Vector3 (spaceVerts [5].x, spaceVerts [5].y - volumeHeight, spaceVerts [5].z);
				bottomTileOut.transform.eulerAngles = new Vector3 (90.0f, 0.0f, 0.0f);

			//Signal that all tiles are parsed
			tilesParsed = true;
		}
	}


	void vEFDassignments (){
		//test each vEFD with the tiles by raycasting from each vEFD to the center of the tracked space
		//for reference  parser.allCyPIPESsensors["0000008,ch1"].transform

		//Signal that all tiles are being assigned
		tilesParsed = false;
		vEFDparse = false;
		vEFDtileRecordComplete = false;

		//raycast from vEFD to center of tracked space
		foreach (string key in parser.allCyPIPESsensors.Keys) {
			//calculate direction to center
			Vector3 vEFDtoCenter =  centerOfTrackedSpace - parser.allCyPIPESsensors[key].transform.localPosition;
			Vector3 vEFDlocalOut = parser.allCyPIPESsensors[key].transform.forward;
			float toCenterDistance = Vector3.Distance (parser.allCyPIPESsensors [key].transform.localPosition, centerOfTrackedSpace);
			//Debug.Log ("DISTANCE CALCULATED AS " + toCenterDistance);

			if (cyPIPESLogActive) {
				//TO
				Debug.DrawRay (parser.allCyPIPESsensors [key].transform.position, vEFDtoCenter, Color.blue, 30, true);
				//AWAY
				//Debug.DrawRay (parser.allCyPIPESsensors [key].transform.position, vEFDtoCenter * -1, Color.green, 30, true);
				Debug.DrawRay (parser.allCyPIPESsensors [key].transform.position, vEFDlocalOut, Color.green, 30, true);
			}

			//check for tile hits
			RaycastHit hit;
			//Ref channel #
			string[] unitInfo = key.Split (',');

			//Generate a Dictionary <unitID ch#,
			if (vEFDtileRecord.Count > 0) {

			}

			//INWARD RAYCAST CHECK
			if (vEFDtileRecord.ContainsKey(key) == false) {
				//if (Physics.Raycast (parser.allCyPIPESsensors [key].transform.position, vEFDtoCenter, out hit, cyPIPESlayer)) {
				if (Physics.Raycast(parser.allCyPIPESsensors[key].transform.position, vEFDtoCenter, out hit, toCenterDistance)){
					if(hit.collider.gameObject.layer == cyPIPESlayer){

						//Ref channel Type
						string chType = "";
						if (unitInfo [1] == "ch1") {chType = GameObject.Find (unitInfo [0]).GetComponent<cyPIPES> ().unitData.ch1Type;}
						if (unitInfo [1] == "ch2") {chType = GameObject.Find (unitInfo [0]).GetComponent<cyPIPES> ().unitData.ch2Type;}
						if (unitInfo [1] == "ch3") {chType = GameObject.Find (unitInfo [0]).GetComponent<cyPIPES> ().unitData.ch3Type;}
						if (unitInfo [1] == "ch4") {chType = GameObject.Find (unitInfo [0]).GetComponent<cyPIPES> ().unitData.ch4Type;}
						if (unitInfo [1] == "ch5") {chType = GameObject.Find (unitInfo [0]).GetComponent<cyPIPES> ().unitData.ch5Type;}
						if (unitInfo [1] == "ch6") {chType = GameObject.Find (unitInfo [0]).GetComponent<cyPIPES> ().unitData.ch6Type;}
						//Ref unitObject
						GameObject unit = GameObject.Find (unitInfo [0]);
						//Record tile assignment data. vEFDtileRecord<"ch#,chType,TileName",unitObject>
						if (cyPIPESLogActive) {Debug.Log("BEING ADDED " + unitInfo [0] + "," + unitInfo [1] + "," + chType + "," + hit.collider.gameObject.name);}

						//Calculate distance to point

						//Normalize tileHit name
						string tileName = hit.collider.gameObject.name;
						tileName = normalizeTileName (tileName);

						//Key format is unitID,ch#,chType,tileAssignment and value is actual tile GameObject
						vEFDtileRecord.Add (unitInfo [0] + "," + unitInfo [1] + "," + chType + "," + tileName, unit);

					}
				}
			
				//OUTWARD RAYCAST CHECK
				if (Physics.Raycast(parser.allCyPIPESsensors[key].transform.position, vEFDlocalOut, out hit, 10.0f)){
					if(hit.collider.gameObject.layer == cyPIPESlayer){

						//Ref channel Type
						string chType = "";
						if (unitInfo [1] == "ch1") {chType = GameObject.Find (unitInfo [0]).GetComponent<cyPIPES> ().unitData.ch1Type;}
						if (unitInfo [1] == "ch2") {chType = GameObject.Find (unitInfo [0]).GetComponent<cyPIPES> ().unitData.ch2Type;}
						if (unitInfo [1] == "ch3") {chType = GameObject.Find (unitInfo [0]).GetComponent<cyPIPES> ().unitData.ch3Type;}
						if (unitInfo [1] == "ch4") {chType = GameObject.Find (unitInfo [0]).GetComponent<cyPIPES> ().unitData.ch4Type;}
						if (unitInfo [1] == "ch5") {chType = GameObject.Find (unitInfo [0]).GetComponent<cyPIPES> ().unitData.ch5Type;}
						if (unitInfo [1] == "ch6") {chType = GameObject.Find (unitInfo [0]).GetComponent<cyPIPES> ().unitData.ch6Type;}
						//Ref unitObject
						GameObject unit = GameObject.Find (unitInfo [0]);
						//Record tile assignment data. vEFDtileRecord<"ch#,chType,TileName",unitObject>
						if (cyPIPESLogActive) {Debug.Log("BEING ADDED " + unitInfo [0] + "," + unitInfo [1] + "," + chType + "," + hit.collider.gameObject.name);}

						//Calculate distance to point

						//Normalize tileHit name
						string tileName = hit.collider.gameObject.name;
						tileName = normalizeTileName (tileName);

						//Key format is unitID,ch#,chType,tileAssignment and value is actual tile GameObject
						vEFDtileRecord.Add (unitInfo [0] + "," + unitInfo [1] + "," + chType + "," + hit.collider.gameObject.name, unit);

					}
				}
			}
			//assign vEFD to tile in a recrod <unitObj,vEFDchannel>

		}
		//Signal that all vEFDs have been assigned to tile record
		vEFDtileRecordComplete = true;
		if (cyPIPESLogActive) {
			Debug.Log ("cyPIPES Log: " +vEFDtileRecord.Count + " cyPIPES Effects Normalized to tracked space tiles.");
		}

	}

	string normalizeTileName(string tile){

		List<string> tileNames = new List<string> ();
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
