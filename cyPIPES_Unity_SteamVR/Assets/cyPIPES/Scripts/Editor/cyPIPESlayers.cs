using UnityEngine;
using System.Collections;
using UnityEditor;

[InitializeOnLoad]
public static class cyPIPESlayers {

	static cyPIPESlayers()
	{
		CreateLayer();
	}

	static void CreateLayer()
	{
		//  https://forum.unity3d.com/threads/adding-layer-by-script.41970/reply?quote=2274824
		SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
		SerializedProperty layers = tagManager.FindProperty("layers");
		bool ExistLayer = false;

		//Prevent duplication of cyPIPES layer
		for (int i = 8; i < layers.arraySize; i++)
		{
			SerializedProperty layerSP = layers.GetArrayElementAtIndex(i);

			//print(layerSP.stringValue);

			if (layerSP.stringValue == "cyPIPES")
			{
				ExistLayer = true;
				break;
			}

		}
		//Set available layer to cyPIPES layer and adjust Physics layer collision matrix
		for (int j = 8; j < layers.arraySize; j++)
		{
			SerializedProperty layerSP = layers.GetArrayElementAtIndex(j);
			if (layerSP.stringValue == "" && !ExistLayer)
			{
				layerSP.stringValue = "cyPIPES";
				tagManager.ApplyModifiedProperties();

				//adjust physics layer collision matrix
				for (int k = 0; k < layers.arraySize; k++) {
					//check each layer
					if(!Physics.GetIgnoreLayerCollision(k,j) && k != j){
						Physics.IgnoreLayerCollision (k, j);
						//Debug.Log (k + " is not ignorging collisions from " + k);
					}
				}

				break;
			}
		}

		//Debug.Log(layers.arraySize);

	}
}
