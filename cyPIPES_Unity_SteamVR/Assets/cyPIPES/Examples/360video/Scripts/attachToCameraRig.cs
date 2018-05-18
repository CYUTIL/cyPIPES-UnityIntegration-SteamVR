using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class attachToCameraRig : MonoBehaviour {

	public Transform vrPlaySpaceObj;

	void Update () {

		if (UnityEngine.XR.XRSettings.enabled) {
			if (vrPlaySpaceObj != null) {
				gameObject.transform.localPosition = vrPlaySpaceObj.transform.localPosition;
			}
		}
		
	}
}
