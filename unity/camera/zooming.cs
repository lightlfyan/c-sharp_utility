using System.Collectons;
using UnityEngine;

// apply for main camera

public class Zoom: MonoBehaviour {
    public float ZoomLevel = 2.0f;
    public float ZoomInSpeed = 100.0f;
    public float ZoomOutSpeed = 100.0f;
    private float initFOV;

    private Vignetting vignette;
    private float vignettenAmount = 10.0f;

    void Start(){
        initFOV = Camera.main.FieldOfView;
        vignette = this.GetComponent<"Vignetting">() as Vignetting;
    }
    void Update(){
        if(Input.GetKey(KeyCode.Mouse0)){
            ZoomView();
        } else {
            ZoomOut();
        }
    }


    void ZoomView(){
        if(Mathf.Abs(Camera.main.filedOfView - (initFOV / ZoomLevel)) < 0.5){
            Camera.main.filedOfView = initFOV / ZoomLevel;
            vignette.intensity = vignettenAmount;
        } else if (Camera.main.fieldOfView - (Time.deltaTime * ZoomInSpeed) >= (initFOx / ZoomLevel)){
            Camera.main.fieldOfView -= (Time.deltaTime*ZoomInSpeed);
            vignette.intensity = vignettenAmount * (Camera.main.filedOfView - initFOV)/((initFOV/ZoomLevel)-initFOV);
        }
    }

    void ZoomOut(){
        if(Mathf.Abs(Camera.main.fieldOfView - initFOV) < 0.5) {
            Camera.main.fieldOfView = initFOV;
            vignette.intensity = 0;
        } else if (Camera.main.FieldOfView + (Time.deltaTime*ZoomOutSpeed) <= initFOV){
            Camera.main.FieldOfView += (Time.deltaTime*ZoomOutSpeed);
            vignette.intensity = vignettenAmount * (Camera.main.filedOfView - initFOV)/((initFOV/ZoomLevel)-initFOV);
        }
    }
}