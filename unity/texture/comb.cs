using UnityEngine;
using System.Collections;

public class comb : MonoBehaviour
{

	//	public int ct;

	public Texture2D t1;
	Texture2D t2;

	public Texture2D t3;
	public Texture2D t4;
	public Texture2D t5;
    
	Texture2D mainTexture;
    
	// Use this for initialization
	void Start ()
	{
	}

	void combine ()
	{
		Texture2D[] textures = new Texture2D[2]{ t1, t2 };
		Texture2D newMainTexture = new Texture2D (512, 512);
		newMainTexture.PackTextures (textures, 0, newMainTexture.width);
		mainTexture = newMainTexture;
		GetComponent<Renderer> ().material.mainTexture = mainTexture;
	}

	void horizontal ()
	{
		var width = t1.width + t2.width;
		mainTexture = new Texture2D (width, t1.height);

		var cs = t1.GetPixels ();
		var cs2 = t2.GetPixels ();

		for (int i = 0; i < t1.height; i++) {
			for (int j = 0; j < t1.width; j++) {
				var idx = t1.width * i + j;
				mainTexture.SetPixel (j, i, cs [idx]);
			}
		}


		for (int i = 0; i < t2.height; i++) {
			for (int j = 0; j < t2.width; j++) {
				var idx = t2.width * i + j;
				mainTexture.SetPixel (j + t1.width, i, cs2 [idx]);
			}
		}

		mainTexture.Apply ();

		GetComponent<Renderer> ().material.mainTexture = mainTexture;
        
		// unity builtin
		// mainTexture.PackTextures (new Texture2D[]{t1, t2}, 0, width);
//		GetComponent<Renderer> ().material.mainTexture = mainTexture;
	}

	void vertical ()
	{
		var height = t1.height + t2.height;
		mainTexture = new Texture2D (t1.width, height);
        
		for (int i = 0; i < t1.width; i++) {
			for (int j = 0; j < t1.height; j++) {
				mainTexture.SetPixel (i, j, t1.GetPixel (i, j));
			}
		}
        
		for (int i = 0; i < t2.width; i++) {
			for (int j = 0; j < t2.height; j++) {
				var j1 = t1.height + j;
                
				mainTexture.SetPixel (i, j1, t2.GetPixel (i, j1));
			}
		}


		var cs = t1.GetPixels ();
		var cs2 = t2.GetPixels ();

		for (int i = 0; i < t1.height; i++) {
			for (int j = 0; j < t1.width; j++) {
				var idx = t1.width * i + j;
				mainTexture.SetPixel (j, i + t2.height, cs [idx]);
			}
		}


		for (int i = 0; i < t2.height; i++) {
			for (int j = 0; j < t2.width; j++) {
				var idx = t2.width * i + j;
				mainTexture.SetPixel (j, i, cs2 [idx]);
			}
		}



		mainTexture.Apply ();
        
//		GetComponent<Renderer> ().material.mainTexture = mainTexture;
        
	}

	void inner ()
	{
		mainTexture = new Texture2D (t1.width, t1.height);

		var cs = t1.GetPixels ();
		var cs2 = t2.GetPixels ();

		for (int i = 0; i < t1.height; i++) {
			for (int j = 0; j < t1.width; j++) {
				var idx = t1.width * i + j;
				mainTexture.SetPixel (j, i, cs [idx]);
			}
		}


		for (int i = 0; i < t2.height; i++) {
			for (int j = 0; j < t2.width; j++) {
				var idx = t2.width * i + j;
				mainTexture.SetPixel (j, i, cs2 [idx]);
			}
		}

		mainTexture.Apply ();
		GetComponent<Renderer> ().material.mainTexture = mainTexture;
	}


	public static Texture2D Flatten (Texture2D[] combines)
	{

		Texture2D newTexture = new Texture2D (0, 0);

		foreach (Texture2D resizeTex in combines) {
			if (resizeTex.width > newTexture.width) {
				newTexture.Resize (resizeTex.width, newTexture.height);
			}
			if (resizeTex.height > newTexture.height) {
				newTexture.Resize (newTexture.width, resizeTex.height);
			}
		}

		return newTexture;
	}

	void OnGUI ()
	{
//		GUI.DrawTexture (new Rect (0, 0, mainTexture.width, mainTexture.height), mainTexture);
		if (GUI.Button (new Rect (0, 0, 100, 50), "3")) {
			t2 = t3;
//			horizontal ();
			inner ();
//			combine ();
		}

		if (GUI.Button (new Rect (0, 50, 100, 50), "4")) {
			t2 = t4;
			combine ();
		}
		if (GUI.Button (new Rect (0, 100, 100, 50), "5")) {
			t2 = t5;
			combine ();
		}
	}
}
