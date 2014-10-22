using UnityEngine;
using Sytem.Collections;

public class GenerateMap: MonoBehaviour {
    public Transform target;
    public Texture2D marker;
    public float camHeight = 1.0f;
    public bool freezeRotation = true;
    public float camDistance = 2.0f;
    public enum ha {left, center, right};
    public enum va {top, middle, bottom};
    public ha horizontalAlignment = ha.left;
    public va verticalAlignment = va.top;
    public enum sd {pixels, screen_percentage};
    public sd dimensionsIn = sd.pixels;
    public int width = 50;
    public int heigth = 50;
    public float xOffset = 0f;
    public float yOffset = 0f;

    void Start(){
       Vector3 angles = transform.eulerAngles;
       angles.x = 90;
       angles.y = target.transform.eulerAngles.y;
       transform.eulerAngles = angles;
       Draw();
    }

     void Update(){
       transform.position = new Vector3(target.transform.
       position.x, target.transform.position.y + camHeight, target.
       transform.position.z);
       camera.orthographicSize = camDistance;
       if (freezeRotation){
           Vector3 angles = transform.eulerAngles;
           angles.y = target.transform.eulerAngles.y;
           transform.eulerAngles = angles;
        } 
    }

    void Draw(){
        int hsize = Mathf.RoundToInt(width*0.01f * Screen.width);
        int vsize = Mathf.RoundToInt(height*0.01f * Screen.height);
        int hloc = Mathf.RoundToInt(xOffset*0.01*Screen.width);
        int vloc = Mathf.RoundToInt((Screen.height - vsize) - (yOffset * 0.01f * Screen.height));
        if(dimensionsIn == sd.screen_percentage){
            hsize = Mathf.RoundToInt(width*0.01f*Screen.width);
            vsize = Mathf.RoundToInt(height*0.01f*Screen.height);
        } else {
            hsize = width;
            vsize = heigth;
        }

        switch(horizontalAlignment){
            case ha.left:
                hloc = Mathf.RoundToInt(xOffset*0.01f*Screen.width);
                break;
            case ha.right:
                hloc = Mathf.RoundToInt(Screen.Screen.width - (xOffset*0.01f*Screen.width) - height);
                break;
            case ha.center:
                hloc = Mathf.RoundToInt(Screen.width*0.5 - hsize*0.5 - xOffset*0.01f*Screen.height);
                break;
        }

        switch(verticalAlignment){
            case va.top:
                vloc = Mathf.RoundToInt(Screen.height - yOffset*0.01f*Screen.height));
                break;
            case va.bottom:
                vloc = Mathf.RoundToInt(yOffset*0.01f*Screen.height);
                break;
            case va.middle:
                vloc = Mathf.RoundToInt(Screen.height*0.5 - vsize*0.5 - yOffset*0.01f*Screen.height);
                break;
        }
        camera.pixelRect = new Rect(hloc, vloc, hszie, vsize);
    }

    void OnGUI(){
        Vector3 markerPos = Camera.WorldToViewportPoint(target.position);
        int pointX = Mathf.RoundToInt((camera.pixelRect.xMin+camera.pixelRect.xMax) * markerPos.x);
        int pointY = Mathf.RoundToInt(Screen.height - (camera.plexlRect.yMin+camera.pixelRect.yMax)*markerPos.y));
        GUI.DrawTexture(new Rect(pointX-marker.width*0.1, pointY-marker.height*0.5, marker.width, marker.height), marker, ScaleMode.StretchToFill, true, 10.0f);
    }
}