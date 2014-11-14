using UnityEngine;
using System.Collections;

public class StereoCam: MonoBehaviour {
    public float parallaxDistance = 10.0f;
    public float eyeDistance =0.05f;
    private bool showGUI = false;

    private GameObject lCamera;
    private GameObject rCamera;
    private GameObject parallax;

    void Start(){
        lCamera = new GameObject("LCamera");
        Lcamera.AddComponent<Camera>();
        Lcamera.AddComponent<GUILayer>();
        Lcamera.AddComponent("FlareLayer");
        Lcamera.camera.CopyFrom(Camera.main);

        rCamera = (GameObject) Instantiate(lCamera, transform.position, transform.rotation);
        rCamera.name = "rCamera";
        lCamera.transform.parent = Camera.main.transform;
        rCamera.transform.parent = Camera.main.transform;

        parallax = new GameObject("parallax");
        parallax.transform.parent = Cream.main.transform;
        parallax.transform.localPosition = Vector3.Zero;

        Vector3 parallaxPosition = parallax.transform.localPosition;

        parallaxPosition.z = parallaxDistance;
        parallax.transform.localPosition = parallaxPosition;
        camera.enabled = false;

        lCamera.camera.rect = new Rect(0, 0, 0.5f, 1);
        rCamera.camera.rect = new Rect(0.5f, 0, 0.5f, 1);
        lCamera.transform.localPosition = Vector3.zero;
        rCamera.transform.localPosition = Vector3.zero;

        Vector3 cameraPosition = lCamera.localPosition;
        cameraPosition.x = -eyeDistance;
        lCamera.transform.localPosition = cameraPosition;
        cameraPosition.x = eyeDistance;
        rCamera.transform.localPosition = cameraPosition;



    }

    void Update(){
        lCamera.transform.LookAt(parallax.transform);
        rCamera.transform.LookAt(parallax.transform);
        if(Input.GetKeyUP(KeyCode.Escape)){
            if(showGUI){
                showGUI = false;
            } else {
                showGUI = true;
            }
        }

        if(showGUI){
            if(Input.GetKey(KeyCode.Alpha1)){
                eyeDistance -= 0.001f;
            }
            if(Input.GetKey(KeyCode.Alpha2)){
                eyeDistance += 0.001f;
            }
            if(Input.GetKey(KeyCode.Alpha3)){
                parallaxDistance -= 0.001f;
            }
            if(Input.GetKey(KeyCode.Alpha4)){
                parallaxDistance += 0.001f;
            }

            Vector3 cameraPosition = lCamera.transform.localPosition;
            cameraPosition.x = -eyeDistance;
            lCamera.transform.localPosition = cameraPosition;
            cameraPosition.x = eyeDistance;
            rCamera.transform.localPosition = cameraPosition;

            Vector3 parallaxPosition = parallax.transform.localPosition;

            parallaxPosition.z = parallaxDistance;
            parallax.transform.localPosition = parallaxPosition;
        }
    }

    void OnGUI(){
        for(int n = 0; n<2; n++){
            if(showGUI){
                GUI.Lable(new Rect((Screen.width*0.5f *n) + 10, 30, 500, 20), "eye distance: " + eyeDistance.ToString("F")+ ". press 1 to decrease and 2 to increase.");
                GUI.Lable(new Rect((Screen.width*0.5f *n) + 10, 70, 500, 20), "parallax distance: " + parallaxDistance.ToString("F")+ ". press 1 to decrease and 2 to increase.");
            } else {
                GUI.Lable(new Rect((Screen.width*0.5f * n) + 10, 10, 500, 20), "press esc to adjust stereo parameters");
            }

        }
    }
}
