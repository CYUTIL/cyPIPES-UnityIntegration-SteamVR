using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class cyPIPESComms : MonoBehaviour {

	[Header("cyPIPES Logs and Errors Options")]
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Logs.")]
	public bool cyPIPESLogActive;
	[Tooltip("On by default. Toggle off to turn off cyPIPES Debug.Errors.")]
	public bool cyPIPESErrorsActive;
	[HideInInspector]
	public cyPIPESConfigs configs;
	[HideInInspector]
	public cyPIPESparsePost parser;
	string IP = "";
	int port;

	//UDP Variables
	IPEndPoint remoteEndPoint; //from System.Net
	UdpClient client; //from System.Net.Sockets

	// Use this for initialization
	void Start () {
		port = 5005;
	}
	
	// Update is called once per frame
	void Update () {
		//keep checking for IP from configs
		if (IP == "") {
			IP = configs.rootUnitAddress;
			//when root config is found initialize UDP comms
			if (IP.Length > 3) {
				init();
			}
		}
	}

	public void init(){
		//point UDP client towards cyPIPES root unit
		remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
		client = new UdpClient();
	}

	public void sendCommand (string command){
		
		try{
			byte[] data = Encoding.UTF8.GetBytes(command);

			//Send IT!
			client.Send(data, data.Length, remoteEndPoint);
			if (cyPIPESLogActive){Debug.Log ("cyPIPES Log: Command sent over network >>> " + command);}

		}catch (Exception err){
			if (cyPIPESErrorsActive) {Debug.LogError ("cyPIPES ERROR: cyPIPESComms UDP system error <" + err.ToString()+ ">");}
		}

	}

	void OnApplicationQuit(){
		//Shut all chs off
		foreach (string unit in parser.allUnitObjects.Keys) {
			string offCommand = unit + "000000";
			sendCommand(offCommand);
		}
		if (cyPIPESLogActive){Debug.Log ("cyPIPES Log: All cyPIPES channels off commands sent during application quit routine.");}
	}
}
