using UnityEngine;
using System.Collections;
using System.IO;

public class ReadManualResourceImageFile: MonoBehaviour {
    private string filename = "externalTexture.jpg";
    private string url;
    private Texture2D externalImage;

    private void Start(){
        url = "file:" + Application.dataPath;
        url = Path.Combine(url, "Resouces");
        url = Path.Combine(url, fileName);

        StartCoroutine(LoadWWW());
    }

    private void OnGUI(){
        GUILayout.Label("url="+url);
        GUILayout.Label(externalImage);
    }

    private IEnumerator LoadWWW(){
        yield return 0;
        WWW www =new WWW(url);
        yield return www;
        externalImage = www.texture;
    }
}
