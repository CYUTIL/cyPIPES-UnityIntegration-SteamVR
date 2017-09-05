using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class cyPIPES_360_FX_orchestration : MonoBehaviour {

	[Tooltip("If you are animating with a 360 video this should be the cyPIPESVideoSphere object.")]
	public GameObject video360Obj;
	public VideoPlayer video360;
	[Tooltip("As you are editing change this number to where your 360 video and fx timeline should start. Set back to 0 when done.")]
	public int jumpToFrame;
	[Tooltip("Use the animator component attached to this object to control effects")]
	public Animator fxTimeline;

	public bool startVideoWithSpaceBar;
	public bool startVideoAtRuntime;
	[Tooltip("Set this value true from another script to play the video from a custom event.")]
	public bool startVideoExternally;
	public bool resetVideoWithR_Key;

	[HideInInspector]
	public GameObject frontFX_wind;
	[HideInInspector]
	public GameObject frontFX_heat;

	[HideInInspector]
	public GameObject backFX_wind;
	[HideInInspector]
	public GameObject backFX_heat;

	[HideInInspector]
	public GameObject leftFX_wind;
	[HideInInspector]
	public GameObject leftFX_heat;

	[HideInInspector]
	public GameObject rightFX_wind;
	[HideInInspector]
	public GameObject rightFX_heat;

	[HideInInspector]
	public GameObject topFX_wind;
	[HideInInspector]
	public GameObject topFX_heat;

	[HideInInspector]
	public GameObject bottomFX_wind;
	[HideInInspector]
	public GameObject bottomFX_heat;

	[Tooltip("Animate this value between 0,1,2,& 3 with your content")]
	public float a_frontWindFX;
	[Tooltip("Animate this value between 0 & 1 with your content")]
	public float a_frontHeatFX;
	[Tooltip("Animate this value between 0,1,2,& 3 with your content")]
	public float b_backWindFX;
	[Tooltip("Animate this value between 0 & 1 with your content")]
	public float b_backHeatFX;
	[Tooltip("Animate this value between 0,1,2,& 3 with your content")]
	public float c_leftWindFX;
	[Tooltip("Animate this value between 0 & 1 with your content")]
	public float c_leftHeatFX;
	[Tooltip("Animate this value between 0,1,2,& 3 with your content")]
	public float d_rightWindFX;
	[Tooltip("Animate this value between 0 & 1 with your content")]
	public float d_rightHeatFX;
	[Tooltip("Animate this value between 0,1,2,& 3 with your content")]
	public float e_topWindFX;
	[Tooltip("Animate this value between 0 & 1 with your content")]
	public float e_topHeatFX;
	[Tooltip("Animate this value between 0,1,2,& 3 with your content")]
	public float f_bottomWindFX;
	[Tooltip("Animate this value between 0 & 1 with your content")]
	public float f_bottomHeatFX;

	windFX_cyPIPES frontWind;
	heatFX_cyPIPES frontHeat;
	windFX_cyPIPES backWind;
	heatFX_cyPIPES backHeat;
	windFX_cyPIPES leftWind;
	heatFX_cyPIPES leftHeat;
	windFX_cyPIPES rightWind;
	heatFX_cyPIPES rightHeat;
	windFX_cyPIPES topWind;
	heatFX_cyPIPES topHeat;
	windFX_cyPIPES bottomWind;
	heatFX_cyPIPES bottomHeat;

	void Start () {
		//video360 = video360Obj.GetComponent<VideoPlayer> ();

		frontWind = frontFX_wind.GetComponent<windFX_cyPIPES> ();
		frontHeat = frontFX_heat.GetComponent<heatFX_cyPIPES> ();
		backWind = backFX_wind.GetComponent<windFX_cyPIPES> ();
		backHeat = backFX_heat.GetComponent<heatFX_cyPIPES> ();
		leftWind = leftFX_wind.GetComponent<windFX_cyPIPES> ();
		leftHeat = leftFX_heat.GetComponent<heatFX_cyPIPES> ();
		rightWind = rightFX_wind.GetComponent<windFX_cyPIPES> ();
		rightHeat = rightFX_heat.GetComponent<heatFX_cyPIPES> ();
		topWind = topFX_wind.GetComponent<windFX_cyPIPES> ();
		topHeat = topFX_heat.GetComponent<heatFX_cyPIPES> ();
		bottomWind = bottomFX_wind.GetComponent<windFX_cyPIPES> ();
		bottomHeat = bottomFX_heat.GetComponent<heatFX_cyPIPES> ();

		//if runtime
		if (video360 != null && fxTimeline != null) {
			if (startVideoAtRuntime) {
				//Start 360 at jumpto frame
				video360.Play ();
				video360.frame = jumpToFrame;
				Debug.Log ("360 Video Playing at runtime from frame " + jumpToFrame);
				if (jumpToFrame != 0) {
					Debug.Log ("Warning: cyPIPES FX are not not sycronized.");
				}
				//Start animation clip
				fxTimeline.SetBool("playFX",true);
				fxTimeline.SetBool("stopFX",false);
			}
		}
	}
	
	void Update () {

		//Manage 360 video and fx timeline
		if (video360 != null && fxTimeline != null) {
			//spacebar
			if (startVideoWithSpaceBar) {
				if (Input.GetKeyDown (KeyCode.Space)) {
					//Start 360 at jumpto frame
					video360.Play ();
					video360.frame = jumpToFrame;
					Debug.Log ("360 Video Playing now from frame " + jumpToFrame);
					if (jumpToFrame != 0) {
						Debug.Log ("Warning: cyPIPES FX are not not sycronized.");
					}
					//Start animation clip
					fxTimeline.SetBool("playFX",true);
					fxTimeline.SetBool("stopFX",false);
				}
			}

			//Start video with external script event
			if (startVideoExternally) {
				//Start 360 at jumpto frame
				video360.Play ();
				video360.frame = jumpToFrame;
				Debug.Log ("360 Video Playing now from frame " + jumpToFrame);
				if (jumpToFrame != 0) {
					Debug.Log ("Warning: cyPIPES FX are not not sycronized.");
				}
				//Start animation clip
				fxTimeline.SetBool("playFX",true);
				fxTimeline.SetBool("stopFX",false);
				startVideoExternally = false;
			}

			//reset
			if (resetVideoWithR_Key) {
				if (Input.GetKeyDown (KeyCode.R)) {
					video360.frame = 0;
					video360.Stop ();
					Debug.Log ("360 Video resetting now");
					//Stop animation clip
					fxTimeline.SetBool("playFX",false);
					fxTimeline.SetBool("stopFX",false);
				}
			}
		}

		//Manage FX
		//FRONT
		//off
		if (a_frontWindFX == 0.0f) {
			frontWind.intensity = 0;
			frontWind.range = 0.0f;
		}
		//Breeze
		if (a_frontWindFX > 0.0f && a_frontWindFX < 2.0f) {
			frontWind.intensity = 3;
			frontWind.range = 5.0f;
		}
		//Pulse
		if (a_frontWindFX >= 2.0f && a_frontWindFX < 3.0f) {
			frontWind.intensity = 2;
			frontWind.range = 5.0f;
		}
		//On
		if (a_frontWindFX >= 3.0f) {
			frontWind.intensity = 1;
			frontWind.range = 5.0f;
		}
		//heat off
		if (a_frontHeatFX == 0.0f) {
			frontHeat.range = 0.0f;
		}
		//heat on
		if (a_frontHeatFX > 0.0f) {
			frontHeat.range = 5.0f;
		}

		//BACK
		//off
		if (b_backWindFX == 0.0f) {
			backWind.intensity = 0;
			backWind.range = 0.0f;
		}
		//Breeze
		if (b_backWindFX > 0.0f && b_backWindFX < 2.0f) {
			backWind.intensity = 3;
			backWind.range = 5.0f;
		}
		//Pulse
		if (b_backWindFX >= 2.0f && b_backWindFX < 3.0f) {
			backWind.intensity = 2;
			backWind.range = 5.0f;
		}
		//On
		if (b_backWindFX >= 3.0f) {
			backWind.intensity = 1;
			backWind.range = 5.0f;
		}
		//heat off
		if (b_backHeatFX == 0.0f) {
			backHeat.range = 0.0f;
		}
		//heat on
		if (b_backHeatFX > 0.0f) {
			backHeat.range = 5.0f;
		}

		//LEFT
		//off
		if (c_leftWindFX == 0.0f) {
			leftWind.intensity = 0;
			leftWind.range = 0.0f;
		}
		//Breeze
		if (c_leftWindFX > 0.0f && c_leftWindFX < 2.0f) {
			leftWind.intensity = 3;
			leftWind.range = 5.0f;
		}
		//Pulse
		if (c_leftWindFX >= 2.0f && c_leftWindFX < 3.0f) {
			leftWind.intensity = 2;
			leftWind.range = 5.0f;
		}
		//On
		if (c_leftWindFX >= 3.0f) {
			leftWind.intensity = 1;
			leftWind.range = 5.0f;
		}
		//heat off
		if (c_leftHeatFX == 0.0f) {
			leftHeat.range = 0.0f;
		}
		//heat on
		if (c_leftHeatFX > 0.0f) {
			leftHeat.range = 5.0f;
		}

		//RIGHT
		//off
		if (d_rightWindFX == 0.0f) {
			rightWind.intensity = 0;
			rightWind.range = 0.0f;
		}
		//Breeze
		if (d_rightWindFX > 0.0f && d_rightWindFX < 2.0f) {
			rightWind.intensity = 3;
			rightWind.range = 5.0f;
		}
		//Pulse
		if (d_rightWindFX >= 2.0f && d_rightWindFX < 3.0f) {
			rightWind.intensity = 2;
			rightWind.range = 5.0f;
		}
		//On
		if (d_rightWindFX >= 3.0f) {
			rightWind.intensity = 1;
			rightWind.range = 5.0f;
		}
		//heat off
		if (d_rightHeatFX == 0.0f) {
			rightHeat.range = 0.0f;
		}
		//heat on
		if (d_rightHeatFX > 0.0f) {
			rightHeat.range = 5.0f;
		}

		//TOP
		//off
		if (e_topWindFX == 0.0f) {
			topWind.intensity = 0;
			topWind.range = 0.0f;
		}
		//Breeze
		if (e_topWindFX > 0.0f && e_topWindFX < 2.0f) {
			topWind.intensity = 3;
			topWind.range = 5.0f;
		}
		//Pulse
		if (e_topWindFX >= 2.0f && e_topWindFX < 3.0f) {
			topWind.intensity = 2;
			topWind.range = 5.0f;
		}
		//On
		if (e_topWindFX >= 3.0f) {
			topWind.intensity = 1;
			topWind.range = 5.0f;
		}
		//heat off
		if (e_topHeatFX == 0.0f) {
			topHeat.range = 0.0f;
		}
		//heat on
		if (e_topHeatFX > 0.0f) {
			topHeat.range = 5.0f;
		}

		//BOTTOM
		//off
		if (f_bottomWindFX == 0.0f) {
			bottomWind.intensity = 0;
			bottomWind.range = 0.0f;
		}
		//Breeze
		if (f_bottomWindFX > 0.0f && f_bottomWindFX < 2.0f) {
			bottomWind.intensity = 3;
			bottomWind.range = 5.0f;
		}
		//Pulse
		if (f_bottomWindFX >= 2.0f && f_bottomWindFX < 3.0f) {
			bottomWind.intensity = 2;
			bottomWind.range = 5.0f;
		}
		//On
		if (f_bottomWindFX >= 3.0f) {
			bottomWind.intensity = 1;
			bottomWind.range = 5.0f;
		}
		//heat off
		if (f_bottomHeatFX == 0.0f) {
			bottomHeat.range = 0.0f;
		}
		//heat on
		if (f_bottomHeatFX > 0.0f) {
			bottomHeat.range = 5.0f;
		}
		
	}
}
