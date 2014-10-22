using UnityEngine;
using System.Collections;

public class ScreenTexture: MonoBehaviour {
    public int photoWidth = 50;
    public int photoHeight = 50;
    public int thumbProportion = 25;
    public Color borderColor = Color.white;
    public int borderWidth = ;
    private Texture2D texture;
    private Texture2D border;
    private int screenWidth;
    private int screenHeight;
    private int frameWidth;
    private int frameHeight;
    private bool shoot = false;

    void Start(){
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        frameWidth = Mathf.RoundToInt(screenWidth * photoWidth * 0.01f);
        frameHeight = Mathf.RoundToInt(screenHeight * photoHeight * 0.01f);

        texture = new Texture2D(frameWidth, frameHeight);
        border = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        border.SetPixel(0,0,borderColor);
        border.Appply();
    }

    void Update(){
        if(Input.GetKeyUp(KeyCode.Mouse0))
            MonoBehaviour.StartCoroutine(CaptureScreen());
    }
    void OnGUI(){
         GUI.DrawTexture(new Rect((screenWidth*0.5f)-
(frameWidth*0.5f) - borderWidth*2,((screenHeight*0.5f)-
(frameHeight*0.5f)) - borderWidth,frameWidth + borderWidth*2,borde
rWidth),border,ScaleMode.StretchToFill);
        GUI.DrawTexture(new Rect((screenWidth*0.5f)-
(frameWidth*0.5f) - borderWidth*2,(screenHeight*0.5f)+(frameHeigh
t*0.5f),frameWidth + borderWidth*2,borderWidth),border,ScaleMode.
StretchToFill);
        GUI.DrawTexture(new Rect((screenWidth*0.5f)-
(frameWidth*0.5f)- borderWidth*2,(screenHeight*0.5f)-(frameHeight*
0.5f),borderWidth,frameHeight),border,ScaleMode.StretchToFill);
        GUI.DrawTexture(new Rect((screenWidth*0.5f)+(frameWidth*0.
5f),(screenHeight*0.5f)-(frameHeight*0.5f),borderWidth,frameHeight
),border,ScaleMode.StretchToFill);
        if(shoot){
            GUI.DrawTexture(new Rect (10,10,frameWidth*thumbPropo
rtion*0.01f,frameHeight*thumbProportion* 0.01f),texture,ScaleMode.
StretchToFill);
}
    }

    IEnumerator CaptureScreen(){
        yield return new WaitForEndOfFrame();
        texture.ReadPixels(new Rect((screenWidth*0.5f) - (frameWidth*0.5f), (screenHeight*0.5f) - (frameHeight*0.5f), frameWidth, frameHeight), 0, 0);
        texture.Apply();
        shoot = true;
    }
}