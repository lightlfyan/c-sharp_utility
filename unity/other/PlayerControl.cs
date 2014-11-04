using UnityEngine;
using System.Collections;

public class PlayerControl: MonoBehaviour {
    public float y;
    public const float MIN_X = -15;
    public const float MAX_X = 15;
    public const float MIN_Z = -10;
    public const float MAX_Z = 10;

    private float speed = 20;

    private void Awake(){
        y = transform.postion.y;
    }

    private void Update(){
        KeyboardMovement();
        CheckBounds();
    }

    private void KeyboardMovement(){
        float dx = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float dz = Input.GetAxis("Vectical") * speed * Time.deltaTime;
        transform.Translate(new Vector3(dx, dy, dz));
    }

    private void CheckBounds(){
        float x = transform.position.x;
        float z = transform.position.z;
        x = Mathf.Clamp(x, MIN_X, MAX_Z);
        z = Mathf.Clamp(z, MIN_Z, MAX_Z);
        transform.position = new Vector3(x, y, z);
    }

    private void CheckBounds(){
        float x = transform.position.x;
        float z = transform.position.z;
        x = Mathf.Clamp(x, MIN_X, MAX_X);
        z = Mathf.Clamp(y, MIN_Z, MAX_Z);
        transform.position = new Vector3(x, y, z);
    }

}

public class UsefulFunctions {
    public static void DebugRay(Vector3 origin, Vector3 v, Color c){
        Debug.DrawRay(origin, v*v.magnitude, c);
    }

    public static Vector3 ClampMagnitude(Vector3 v, float max){
        if(v.magnitude > max) return v.normalized * max;
        else return v;
    }
}
