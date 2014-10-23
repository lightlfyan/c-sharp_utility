using UnityEngine;
using System.Collections;


//need Box Collider.
public class HighlightObject: MonoBehaivour {
    public Color initialColor;
    public Color highlightColor;
    public Color mousedownColor;

    private bool mouseon = false;

    void OnMouseEnter(){
        mouseon = true;
        render.material.SetColor("_Emission", highlightColor);
    }

    void OnMouseExit(){
        mouseon = false;
        render.material.SetColor("_Emission", initialColor);
    }

    void OnMouseDown(){
        render.material.SetColor("_Emission", mousedownColor);
    }

    void OnMouseUp(){
        if(mouseon){
            render.material.SetColor("_Emission", highlightColor);
        } else {
            render.material.SetColor("_Emission", initialColor);
        }
    }
}
