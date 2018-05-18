using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exampleManualControlScript : MonoBehaviour {

	manualEvent fxEvent;

	// Use this for initialization
	void Start () {

		fxEvent = gameObject.GetComponent<manualEvent> ();

	}
	
	// Update is called once per frame
	void Update () {

		//Use the P key to manually trigger channel 2 on unit "0000029"
			//NOTE: When you run your code in the editor you will see the unit IDs on the GameObject that the manualEvent.cs component is attached too
		if (Input.GetKeyDown (KeyCode.P)) {
			fxEvent.onUnit ["0000029"].ch2 = 1;
		}
		if (Input.GetKeyUp (KeyCode.P)) {
			fxEvent.onUnit ["0000029"].ch2 = 0;
		}
		
	}
}
