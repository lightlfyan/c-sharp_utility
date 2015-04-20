using UnityEngine;
using System.Collections;

public class comb : MonoBehaviour
{

    public int ct;

    public Texture2D t1;
    public Texture2D t2;
    
    Texture2D mainTexture;
    
    // Use this for initialization
    void Start ()
    {
        if (ct == 1) {
            horizontal ();
            return;
        }
        if (ct == 2) {
            vertical ();
            return;
        }
        if (ct == 3) {
            inner ();
            return;
        }
        
    }
    
    void horizontal ()
    {
        var width = t1.width + t2.width;
        mainTexture = new Texture2D (width, t1.height);
        
        for (int i = 0; i < t1.width; i++) {
            for (int j =0; j<t1.height; j++) {
                mainTexture.SetPixel (i, j, t1.GetPixel (i, j));
            }
        }
        
        for (int i=0; i<t2.width; i++) {
            for (int j = 0; j < t2.height; j++) {
                var i1 = t1.width + i;
                
                mainTexture.SetPixel (i1, j, t2.GetPixel (i1, j));
            }
        }
        mainTexture.Apply ();
        
        // unity builtin
        // mainTexture.PackTextures (new Texture2D[]{t1, t2}, 0, width);
        renderer.material.mainTexture = mainTexture;
    }
    
    void vertical ()
    {
        var height = t1.height + t2.height;
        mainTexture = new Texture2D (t1.width, height);
        
        for (int i = 0; i < t1.width; i++) {
            for (int j =0; j<t1.height; j++) {
                mainTexture.SetPixel (i, j, t1.GetPixel (i, j));
            }
        }
        
        for (int i=0; i<t2.width; i++) {
            for (int j = 0; j < t2.height; j++) {
                var j1 = t1.height + j;
                
                mainTexture.SetPixel (i, j1, t2.GetPixel (i, j1));
            }
        }
        mainTexture.Apply ();
        
        renderer.material.mainTexture = mainTexture;
        
    }
    
    void inner ()
    {
        mainTexture = new Texture2D (t1.width, t1.height);
        for (int i = 0; i < t1.width; i++) {
            for (int j =0; j<t1.height; j++) {
            
                if (i <= t2.width && j <= t2.height) {
                    mainTexture.SetPixel (i, j, t2.GetPixel (i, j));
                    continue;
                }
                mainTexture.SetPixel (i, j, t1.GetPixel (i, j));
            }
        }
        
        mainTexture.Apply ();
        renderer.material.mainTexture = mainTexture;
    }
    
    void OnGUI ()
    {
        //GUI.DrawTexture (new Rect (0, 0, mainTexture.width, mainTexture.height), mainTexture);
    }
}
