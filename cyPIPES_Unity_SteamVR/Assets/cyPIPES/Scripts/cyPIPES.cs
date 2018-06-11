using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cyPIPES : MonoBehaviour {

	[Header("cyPIPES Logs and Errors Options")]
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Logs.")]
	public bool cyPIPESLogActive;
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Errors.")]
	public bool cyPIPESErrorsActive;

	GameObject cyPIPESmasterObj;
	//[HideInInspector]
	public cyPIPESConfigs sourceConfigs;
	public cyPIPESComms cyPIPESnetwork;
	[HideInInspector]
	//The unit's config data
	public cyPIPESunit unitData;
	string unitID;

	//Per frame lists of application votes for status of each channel on this virtual cyPIPES unit
	//[HideInInspector]
	public List<int> ch1Votes = new List<int> ();
//	[HideInInspector]
	public List<int> ch2Votes = new List<int> ();
//	[HideInInspector]
	public List<int> ch3Votes = new List<int> ();
//	[HideInInspector]
	public List<int> ch4Votes = new List<int> ();
//	[HideInInspector]
	public List<int> ch5Votes = new List<int> ();
//	[HideInInspector]
	public List<int> ch6Votes = new List<int> ();

	//Channel shut-off commands 
	//float signalKillThreshold = 0.2f;
//	bool ch1Kill = false;
//	bool ch2Kill = false;
//	bool ch3Kill = false;
//	bool ch4Kill = false;
//	bool ch5Kill = false;
//	bool ch6Kill = false;

	//packet assembly
	string[] packet = new string[6]{"n","n","n","n","n","n"};
	string[] lastPacket = new string[6]{"n","n","n","n","n","n"};
	string lastCustomPacket;

	//cyPIPES unit default values
	private int _ch1 = 0;
	private int _ch2 = 0;
	private int _ch3 = 0;
	private int _ch4 = 0;
	private int _ch5 = 0;
	private int _ch6 = 0;
	private string _customCommandCH1 = "";
	private string _customCommandCH2 = "";
	private string _customCommandCH3 = "";
	private string _customCommandCH4 = "";
	private string _customCommandCH5 = "";
	private string _customCommandCH6 = "";

	//cyPIPES unit public properties
	public int ch1
	{
		get
		{
			return _ch1;
		}
		set
		{
			//Before assigning value off commands a threshold is allowed to minimize choppy effects
//			if (value != 0) {
//				_ch1 = value;
//			}
//			if (value == 0 && ch1Kill == false) {
//				StartCoroutine (chKilltimeout (signalKillThreshold,1));
//			}
//			if (value == 0 && ch1Kill == true) {
//				_ch1 = value;
//				ch1Kill = false;
//			}
			//basic operation ^^above is more advanced 0 handling stuff^^
			_ch1 = value;
			//Full Off: Wind, Heat, & Other
			if (_ch1 == 0) {
				//set packet[0] to 1
				packet[0] = "0";
			}
			//Full On: Wind, Heat, & Other
			if (_ch1 == 1) {
				//set packet[0] to 1
				packet[0] = "1";
			}
			//Pulse: Wind only
			if (_ch1 == 2) {
				//if ch1 is wind
				if (unitData.ch1Type == "Fan"){
					//set packet[0] to 2
					packet[0] = "2";
				}
			}
			//Breeze: Wind only
			if (_ch1 == 3) {
				//if ch1 is wind
				if (unitData.ch1Type == "Fan"){
					//set packet[0] to 3
					packet[0] = "3";
				}
			}
			//indicate packet update
			packetUpdate(packet);
		}
	}
	public int ch2
	{
		get
		{
			return _ch2;
		}
		set
		{
			//Before assigning value off commands a threshold is allowed to minimize choppy effects
//			if (value != 0) {
//				_ch2 = value;
//			}
//			if (value == 0 && ch2Kill == false) {
//				StartCoroutine (chKilltimeout (signalKillThreshold,2));
//			}
//			if (value == 0 && ch2Kill == true) {
//				_ch2 = value;
//				ch2Kill = false;
//			}
			//basic operation ^^above is more advanced 0 handling stuff^^
			_ch2 = value;
			//Full Off: Wind, Heat, & Other
			if (_ch2 == 0) {
				//set packet[0] to 1
				packet[1] = "0";
			}
			//Full On: Wind, Heat, & Other
			if (_ch2 == 1) {
				//set packet[0] to 1
				packet[1] = "1";
			}
			//Pulse: Wind only
			if (_ch2 == 2) {
				//if ch1 is wind
				if (unitData.ch2Type == "Fan"){
					//set packet[0] to 2
					packet[1] = "2";
				}
			}
			//Breeze: Wind only
			if (_ch2 == 3) {
				//if ch1 is wind
				if (unitData.ch2Type == "Fan"){
					//set packet[0] to 3
					packet[1] = "3";
				}
			}
			//indicate packet update
			packetUpdate(packet);
		}
	}
	public int ch3
	{
		get
		{
			return _ch3;
		}
		set
		{
			//Before assigning value off commands a threshold is allowed to minimize choppy effects
//			if (value != 0) {
//				_ch3 = value;
//			}
//			if (value == 0 && ch3Kill == false) {
//				StartCoroutine (chKilltimeout (signalKillThreshold,3));
//			}
//			if (value == 0 && ch3Kill == true) {
//				_ch3 = value;
//				ch3Kill = false;
//			}
			//basic operation ^^above is more advanced 0 handling stuff^^
			_ch3 = value;
			//Full Off: Wind, Heat, & Other
			if (_ch3 == 0) {
				//set packet[0] to 1
				packet[2] = "0";
			}
			//Full On: Wind, Heat, & Other
			if (_ch3 == 1) {
				//set packet[0] to 1
				packet[2] = "1";
			}
			//Pulse: Wind only
			if (_ch3 == 2) {
				//if ch1 is wind
				if (unitData.ch3Type == "Fan"){
					//set packet[0] to 2
					packet[2] = "2";
				}
			}
			//Breeze: Wind only
			if (_ch3 == 3) {
				//if ch1 is wind
				if (unitData.ch3Type == "Fan"){
					//set packet[0] to 3
					packet[2] = "3";
				}
			}
			//indicate packet update
			packetUpdate(packet);
		}
	}
	public int ch4
	{
		get
		{
			return _ch4;
		}
		set
		{
			//Before assigning value off commands a threshold is allowed to minimize choppy effects
//			if (value != 0) {
//				_ch4 = value;
//			}
//			if (value == 0 && ch4Kill == false) {
//				StartCoroutine (chKilltimeout (signalKillThreshold,4));
//			}
//			if (value == 0 && ch4Kill == true) {
//				_ch4 = value;
//				ch4Kill = false;
//			}
			//basic operation ^^above is more advanced 0 handling stuff^^
			_ch4 = value;
			//Full Off: Wind, Heat, & Other
			if (_ch4 == 0) {
				//set packet[0] to 1
				packet[3] = "0";
			}
			//Full On: Wind, Heat, & Other
			if (_ch4 == 1) {
				//set packet[0] to 1
				packet[3] = "1";
			}
			//Pulse: Wind only
			if (_ch4 == 2) {
				//if ch1 is wind
				if (unitData.ch4Type == "Fan"){
					//set packet[0] to 2
					packet[3] = "2";
				}
			}
			//Breeze: Wind only
			if (_ch4 == 3) {
				//if ch1 is wind
				if (unitData.ch4Type == "Fan"){
					//set packet[0] to 3
					packet[3] = "3";
				}
			}
			//indicate packet update
			packetUpdate(packet);
		}
	}
	public int ch5
	{
		get
		{
			return _ch5;
		}
		set
		{
			//Before assigning value off commands a threshold is allowed to minimize choppy effects
//			if (value != 0) {
//				_ch5 = value;
//			}
//			if (value == 0 && ch5Kill == false) {
//				StartCoroutine (chKilltimeout (signalKillThreshold,5));
//			}
//			if (value == 0 && ch5Kill == true) {
//				_ch5 = value;
//				ch5Kill = false;
//			}
			//basic operation ^^above is more advanced 0 handling stuff^^
			_ch5 = value;
			//Full Off: Wind, Heat, & Other
			if (_ch5 == 0) {
				//set packet[0] to 1
				packet[4] = "0";
			}
			//Full On: Wind, Heat, & Other
			if (_ch5 == 1) {
				//set packet[0] to 1
				packet[4] = "1";
			}
			//Pulse: Wind only
			if (_ch5 == 2) {
				//if ch1 is wind
				if (unitData.ch5Type == "Fan"){
					//set packet[0] to 2
					packet[4] = "2";
				}
			}
			//Breeze: Wind only
			if (_ch5 == 3) {
				//if ch1 is wind
				if (unitData.ch5Type == "Fan"){
					//set packet[0] to 3
					packet[4] = "3";
				}
			}
			//indicate packet update
			packetUpdate(packet);
		}
	}
	public int ch6
	{
		get
		{
			return _ch6;
		}
		set
		{
			//Before assigning value off commands a threshold is allowed to minimize choppy effects
//			if (value != 0) {
//				_ch6 = value;
//			}
//			if (value == 0 && ch6Kill == false) {
//				StartCoroutine (chKilltimeout (signalKillThreshold,6));
//			}
//			if (value == 0 && ch6Kill == true) {
//				_ch6 = value;
//				ch6Kill = false;
//			}
			//basic operation ^^above is more advanced 0 handling stuff^^
			_ch6 = value;
			//Full Off: Wind, Heat, & Other
			if (_ch6 == 0) {
				//set packet[0] to 1
				packet[5] = "0";
			}
			//Full On: Wind, Heat, & Other
			if (_ch6 == 1) {
				//set packet[0] to 1
				packet[5] = "1";
			}
			//Pulse: Wind only
			if (_ch6 == 2) {
				//if ch1 is wind
				if (unitData.ch6Type == "Fan"){
					//set packet[0] to 2
					packet[5] = "2";
				}
			}
			//Breeze: Wind only
			if (_ch6 == 3) {
				//if ch1 is wind
				if (unitData.ch6Type == "Fan"){
					//set packet[0] to 3
					packet[5] = "3";
				}
			}
			//indicate packet update
			packetUpdate(packet);
		}
	}

	//public parameters for not integer custom channel commands
	//WARNING: These commands are not filtered for priority, see Update() and prioritizeVotes() for an example of filtering the integer channel commands for priority.
	//WARNING: Without filtering for priority frame command "effect flimmering" may happen.  Flimmering is not the right word, but its the same type of computational issue.
	public string customCommandCH1
	{
		get{
			return _customCommandCH1;
		}
		set{
			_customCommandCH1 = value;
			if (_customCommandCH1 != "" || _customCommandCH1 != null) {
				customUpdate (_customCommandCH1);
			}
		}
	}
	public string customCommandCH2
	{
		get{
			return _customCommandCH2;
		}
		set{
			_customCommandCH2 = value;
			if (_customCommandCH2 != "" || _customCommandCH2 != null) {
				customUpdate (_customCommandCH2);
			}
		}
	}
	public string customCommandCH3
	{
		get{
			return _customCommandCH3;
		}
		set{
			_customCommandCH3 = value;
			if (_customCommandCH3 != "" || _customCommandCH3 != null) {
				customUpdate (_customCommandCH3);
			}
		}
	}
	public string customCommandCH4
	{
		get{
			return _customCommandCH4;
		}
		set{
			_customCommandCH4 = value;
			if (_customCommandCH4 != "" || _customCommandCH4 != null) {
				customUpdate (_customCommandCH4);
			}
		}
	}
	public string customCommandCH5
	{
		get{
			return _customCommandCH5;
		}
		set{
			_customCommandCH5 = value;
			if (_customCommandCH5 != "" || _customCommandCH5 != null) {
				customUpdate (_customCommandCH5);
			}
		}
	}
	public string customCommandCH6
	{
		get{
			return _customCommandCH6;
		}
		set{
			_customCommandCH6 = value;
			if (_customCommandCH6 != "" || _customCommandCH6 != null) {
				customUpdate (_customCommandCH6);
			}
		}
	}

	void Awake () {

		try{
			//Dynamic reference to source config component
			cyPIPESmasterObj = GameObject.Find ("cyPIPES");
			sourceConfigs = cyPIPESmasterObj.GetComponent<cyPIPESConfigs> ();
			cyPIPESnetwork = cyPIPESmasterObj.GetComponent<cyPIPESComms> ();

			//gather unitID from unit's GameObject name
			unitID = this.name;
			//Checking to make sure that the Object.name changed to unitID and waiting if it's not
			if (unitID == "unitObject(Clone)"){
				StartCoroutine(waitForParse (0.5f));
			}

			//reference specific cyPIPESunit config data
			foreach (string key in sourceConfigs.allCyPIPESunits.Keys) {
				if (sourceConfigs.allCyPIPESunits[key].ID == unitID) {
					if (cyPIPESLogActive){Debug.Log("cyPIPES Log: Virtual cyPIPES unit "+unitID+ " is instatiated and config parameters linked");}
				}
			}
		}catch{
			if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: Unit "+ gameObject.name + " has a null ref. to the cyPIPES prefab. Is it active in the scene?");}
		}
			
		//Debug.Log ("TESTING ch1 Type is " + unitData.ch1posX + " for unit " + unitID + " TESTING");
	}
	
	// Update is called once per frame
	void Update () {

		//consider all votes for each channel's behaviour and trigger parameter change
		if (ch1Votes.Count > 0) {
			ch1 = prioritizeVotes (ch1Votes);
			//Reset list for next frame
			ch1Votes = new List<int>();
		}
		if (ch2Votes.Count > 0) {
			ch2 = prioritizeVotes (ch2Votes);
			//Reset list for next frame
			ch2Votes = new List<int>();
		}
		if (ch3Votes.Count > 0) {
			ch3 = prioritizeVotes (ch3Votes);
			//Reset list for next frame
			ch3Votes = new List<int>();
		}
		if (ch4Votes.Count > 0) {
			ch4 = prioritizeVotes (ch4Votes);
			//Reset list for next frame
			ch4Votes = new List<int>();
		}
		if (ch5Votes.Count > 0) {
			ch5 = prioritizeVotes (ch5Votes);
			//Reset list for next frame
			ch5Votes = new List<int>();
		}
		if (ch6Votes.Count > 0) {
			ch6 = prioritizeVotes (ch6Votes);
			//Reset list for next frame
			ch6Votes = new List<int>();
		}
			
	}

	//Coroutine to wait until vEFDtileRecord is completed
	IEnumerator waitForParse (float waitTime){
		yield return new WaitForSeconds (waitTime);
		//Run Awake again
		Awake ();
	}

	void packetUpdate(string[] packet){
		//compare last packet to new one
		bool changed = false;
		for (int i = 0; i < 6; i++) {
			if (packet [i] != lastPacket [i]) {
				//something has changed
				changed = true;
			}
		}

		//if there is a change send it to cyPIPEScomms to be sent to cyPIPES units, ignore update if there is not change
		if (changed) {
			//create string command (sevent digit unitID) unitIDcommand NO COMMA DELIMITER
			string command = unitID + string.Join("",packet);
			//"You guys silly, I'm still gonna send it...."
			cyPIPESnetwork.sendCommand(command);
		}

		//update last packet
		for (int i = 0; i < 6; i++) {
			lastPacket [i] = packet [i];
		}
	}

	void customUpdate(string packet){
		//compare last packet to new one
		bool changed = false;
		if (packet != lastCustomPacket) {
			changed = true;
		}

		//if there is a change send it to cyPIPEScomms to be sent to cyPIPES units, ignore update if there is not change
		if (changed) {
			string command = unitID + packet;
			cyPIPESnetwork.sendCommand (command);
		}
			
		//update last custom packet
		lastCustomPacket = packet;
	}

	//Shut off all channels on this unit
	public void ALLOFF (){
		for(int i = 0; i < 6; i++) {
			packet [i] = "0";
		}
		packetUpdate (packet);
	}

//	//Channel kill threshold wait coroutine. This gives conflicting on commands priority
//	IEnumerator chKilltimeout (float waitTime, int ch){
//		yield return new WaitForSeconds (waitTime);
//		//kill time expired, property cleared for 0 value
//		if(ch == 1){ch1Kill = true;}
//		if(ch == 2){ch2Kill = true;}
//		if(ch == 3){ch3Kill = true;}
//		if(ch == 4){ch4Kill = true;}
//		if(ch == 5){ch5Kill = true;}
//		if(ch == 6){ch6Kill = true;}
//	}

	//Store votes in lists for each channel.  Per frame processing is handled in Update()
	public void ch1Param (int vote){
		ch1Votes.Add (vote);
	}
	public void ch2Param (int vote){
		ch2Votes.Add (vote);
	}
	public void ch3Param (int vote){
		ch3Votes.Add (vote);
	}
	public void ch4Param (int vote){
		ch4Votes.Add (vote);
	}
	public void ch5Param (int vote){
		ch5Votes.Add (vote);
	}
	public void ch6Param (int vote){
		ch6Votes.Add (vote);
	}

	private int prioritizeVotes (List<int> batch){
		int priorityVote;
		foreach (int vote in batch) {
			//First priority is given if one of the votes is a 1 for full on
			if (vote == 1) {
				priorityVote = 1;
				return priorityVote;
			}
		}

		foreach (int vote in batch) {
			//Second priority is given if one of the votes is a 2 for pulse
			if (vote == 2) {
				priorityVote = 2;
				return priorityVote;
			}
		}

		foreach (int vote in batch) {
			//Third priority is given if one of the votes is a 3 for breeze
			if (vote == 3) {
				priorityVote = 3;
				return priorityVote;
			}
		}
		//in the future new commands beyond 0,1,2,3 need to go here for other types of commands

		//Fourth priority is given if one of the votes is 0 for off
		return 0;
	}
}
