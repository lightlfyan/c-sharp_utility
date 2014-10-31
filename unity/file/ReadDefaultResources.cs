using UnityEngine;
using System.Collections;

public class ReadDefaultResources: MonoBehaviour {
    public string filename = "externalTexture";
    public string textname = "test.txt";
    private Textrue2D externalImage;
    private string textfileContents;

    private void Start() {
        externalImage = Resources.Load(filename) as Textrue2D;
        TextAsset textAsset = Resouces.Load(textname) as TextAsset;
        textfileContents = textAsset.text;

        //audio.clip = Resources.Load(audioflile) as AudioClip;
        // auido.Play();
    }

    private OnGUI(){
        GUILayout.Label(externalImage);
        GUILayout.Label(textfileContents);
    }
}
