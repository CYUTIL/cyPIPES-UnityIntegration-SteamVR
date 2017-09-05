using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public static class cyPIPEStileBuild {

	public static GameObject CreateTile(float width, float height, string name, bool collider){
		GameObject go = new GameObject (name);
		MeshFilter mf = go.AddComponent (typeof(MeshFilter)) as MeshFilter;
		MeshRenderer mr = go.AddComponent (typeof(MeshRenderer)) as MeshRenderer;

		//Construct geometry of tile plane
		Mesh m = new Mesh ();
		m.vertices = new Vector3[] {
			new Vector3 (0, 0, 0),
			new Vector3 (width, 0, 0),
			new Vector3 (width, height, 0),
			new Vector3 (0, height, 0)
		};
		m.uv = new Vector2[] {
			new Vector2 (0, 0),
			new Vector2 (0, 1),
			new Vector2 (1, 1),
			new Vector2 (1, 0)
		};
		m.triangles = new int[]{0,1,2,0,2,3};

		//Constucting array for vertices colors
		var colors = new Color[]
		{
			new Color(0.0f, 0.0f, 0.0f, 0.0f),
			new Color(0.0f, 0.0f, 0.0f, 0.0f),
			new Color(0.0f, 0.0f, 0.0f, 0.0f),
			new Color(0.0f, 0.0f, 0.0f, 0.0f)
		};

		//Set shader to be invisble
		mf.mesh = m;
		mr.material = new Material(Shader.Find("Sprites/Default"));
		m.colors = colors;
		mr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
		mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		mr.receiveShadows = false;
		mr.lightProbeUsage = LightProbeUsage.Off;

		//Adding collider
		if (collider) {
			(go.AddComponent (typeof(MeshCollider)) as MeshCollider).sharedMesh = m;
		}

		m.RecalculateBounds ();
		//m.RecalculateNormals ();

		//set plane to cyPIPES layer
		try{
			go.layer = LayerMask.NameToLayer("cyPIPES");
		}catch{
			Debug.LogError ("cyPIPES ERROR: cyPIPES Unity layer missing?");
		}

		return go;
	}

}
