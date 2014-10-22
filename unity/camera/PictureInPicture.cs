using UnityEngine;

public class PictureInPiture: MonoBehaviour {
    public enum HorizontalAligment{left, center, right};
    public enum VerticalAlignment {top, middle, bottom};
    public HorizontalAligment horizontalAligment = HorizontalAligment.left;
    public VerticalAlignment verticalAlignment = VerticalAlignment.top;
    public enum ScreenDimensions{pixels, screen_percentage};
    public ScreenDimensions dimensionsIn = ScreenDimensions.pixels;

    public int width = 50;
    public int height = 50;
    public float xOffset = 0f;
    public float yOffset = 0f;
    public bool update = true;
    private int hsize, vsize, hloc, vloc;

    void Start(){
        AdjustCamera();
    }
    void Update(){
        if(update){
            AdjustCamera();
        }
    }

    void AdjustCamera(){
        if(dimensionsIn == ScreenDimensions.screen_percentage){
            hsize = Mathf.RoundToInt(width*0.01f * Screen.width);
            vsize = Mathf.RoundToInt(height*0.01f * Screen.height);
        } else {
            hsize = width;
            vsize = height;
        }

        if(horizontalAligment == HorizontalAligment.left){
            holc = Mathf.RoundToInt(xOffset*0.01f*Screen.width);
        } else if(horizontalAligment == HorizontalAligment.right){
            holc = Mathf.RoundToInt((Screen.width-hsize) - (xOffset * 0.01f * Screen.width));
        } else {
            holc = Mathf.RoundToInt(((Screen.width * 0.5f) - (hsize*0.5f)) - (xOffset * 0.01f * Screen.height));
        }

        if(verticalAlignment == VerticalAlignment.top){
            vloc = Mathf.RoundToInt((Screen.height - vsize) - (yOffset * 0.01f * Screen.height));
        } else if(verticalAlignment == VerticalAlignment.bootom){
            vloc = Mathf.RoundToInt(yOffset * 0.01f * Screen.height);
        } else {
            vloc = Mathf.RoundToInt(((Screen.height * 0.5f) - (vsize * 0.5f)) - (yoo
                * 0.01f * Screen.height));
        }

        camera.pixelRect = new Rect(hloc, vloc, hsize, vsize);

    }
}