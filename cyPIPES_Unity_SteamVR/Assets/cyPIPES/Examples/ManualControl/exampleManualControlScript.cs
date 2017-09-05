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

		//Use the spacebar to manually trigger channel 2 on unit "0000011"
			//NOTE: When you run your code in the editor you will see the unit IDs on the GameObject that the manualEvent.cs component is attached too
		if (Input.GetKeyDown (KeyCode.Space)) {
			fxEvent.onUnit ["0000011"].ch2 = 1;
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			fxEvent.onUnit ["0000011"].ch2 = 0;
		}
		
	}
}
