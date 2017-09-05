using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cyPIPESunit
{
	//cyPIPES unit json properties
	public string ID;
	public string IP;

	public string ch1Type;
	public float ch1posX;
	public float ch1posY;
	public float ch1posZ;
	public float ch1rotX;
	public float ch1rotY;
	public float ch1rotZ;
	public float ch1scalX;
	public float ch1scalY;
	public float ch1scalZ;

	public string ch2Type;
	public float ch2posX;
	public float ch2posY;
	public float ch2posZ;
	public float ch2rotX;
	public float ch2rotY;
	public float ch2rotZ;
	public float ch2scalX;
	public float ch2scalY;
	public float ch2scalZ;

	public string ch3Type;
	public float ch3posX;
	public float ch3posY;
	public float ch3posZ;
	public float ch3rotX;
	public float ch3rotY;
	public float ch3rotZ;
	public float ch3scalX;
	public float ch3scalY;
	public float ch3scalZ;

	public string ch4Type;
	public float ch4posX;
	public float ch4posY;
	public float ch4posZ;
	public float ch4rotX;
	public float ch4rotY;
	public float ch4rotZ;
	public float ch4scalX;
	public float ch4scalY;
	public float ch4scalZ;

	public string ch5Type;
	public float ch5posX;
	public float ch5posY;
	public float ch5posZ;
	public float ch5rotX;
	public float ch5rotY;
	public float ch5rotZ;
	public float ch5scalX;
	public float ch5scalY;
	public float ch5scalZ;

	public string ch6Type;
	public float ch6posX;
	public float ch6posY;
	public float ch6posZ;
	public float ch6rotX;
	public float ch6rotY;
	public float ch6rotZ;
	public float ch6scalX;
	public float ch6scalY;
	public float ch6scalZ;

	//INPUT FUNCTION Used when reading created cyPIPES unit json files
	public static cyPIPESunit CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<cyPIPESunit> (jsonString);
	}

	//OUTPUT FUNCTION Used when outputting this object data into json formatted string
	public string SaveToString()
	{
		return JsonUtility.ToJson(this,true);
	}

}