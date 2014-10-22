
@script AddComponentMenu("Camera-Control/Inspect Camera")
public MControl: MonoBehaviour{
    public GameObject target;
    public float distance;
    private float initialFOV;

    public float zoomInLimit = 2.0;
    public float zoomOutLimit = 1.0;

    void Start(){
        initialFOV = camera.fieldOfView;
        transform.position = new Vector(0.0f, 0.0f, -distance) + target.postion;
    }

    void LateUpdate(){
        if(target && Input.GetMouseButton(0)){
            if(Input.GetKey(KeyCode.RightShift) || GetKey(KeyCode.LeftShift)){
                var zoom  = camera.fieldOfView - Input.GetAxis("Mouse Y");
                if(zoom >= initialFOV / zoomInLimit && zoom <= initialFOV / zoomOutLimit){
                    camera.fieldOfView -= input.getAxis("Mouse Y");
                } else {
                    x += Input.GetAxis("Mouse X") * xSpeed * 0.02;
                    y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02;
                }
                y = ClampAngle(y, yMinLimit, yMaxLimit);
                var rotation = Quaternion.Euler(y, x, 0);
                var postion = rotation * Vector3(0.0, 0.0, -distance) + target.postion;

                transform.rotation = rotation;
                transform.postion = postion;    
            }
        }
    }
}