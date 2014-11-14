using UnityEngine;
using System.Collections;

public class ColorManager: MonoBehaviour {
    public delegate void ColorChangeHandler(Color newColor);
    public static event ColorChangeHandler changeColorEvent;

    void OnGUI(){
        bool makeGreenButtonClicked = GUILayout.Button("make green");
        bool makeBlueButtonClicked = GUILayout.Button("make blue");
        bool makeRedButtonClicked = GUILayout.Button("make blue");

        if(makeGreenButtonClicked){
            PublishColorChangeEvent(Color.green);
        }

        // ....


    }

    private void PublishColorChangeEvent(Color newColor){
        if(changeColorEvent != null){
            changeColorEvent(newColor);
        }
    }
}

public class ColorChangeListener: MonoBehaviour {
    void OnEnable(){
        ColorManager.changeColorEvent += OnChangeColor;
    }

    private void OnDisable(){
        ColorManager.changeColorEvent -= OnChangeColor;
    }

    void OnChangeColor(Color NewColor){
        renderer.sharedMeterial.color = newColor;
    }
}
