using UnityEngine
using System.Collections;

public class selectTexture: MonoBehaivour {
    public Texture2D[] faces;
    public Texture2D[] props;
    void OnGUI(){
        for(int i=0; i<faces.Length; i++){
            if(GUI.Button(new Rect(0, i*64, 128, 64), faces[i])){
                ChangeMaterial("faces", i);
            }
        }
        for(int j=0; j<props.Length; j++){
            if(GUI.Button(new Rect(128, j*64, 128, 64), props[j])){
                ChangeMaterial("props", j);
            }
        }
    }

    void ChangeMaterial(string category, int index){
        if(category == "faces"){
            reader.material.mainTexture = faces[index];
        }

        if(category == "props"){
            reader.material.mainTexture = props[index];

        }
    }
}
