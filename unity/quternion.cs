using System;
using UnityEngine;

public class Qtest: MonoBehaviour {
    public GameObject CameraObject;
    public float CameraHeight;
    public float CameraDistance;

    public float Ratation;
    public Vector3 tmpvec;

    void Update(){
        if(Input.GetKey("left")){
            Rotation = Rotation - Time.deltaTime * 50f;
        } else if(Input.GetKey("right")){
            Rotation = Rotation + Time.deltaTime * 50f;
        }

        tmpvec = Quternion.AngleAsis(Rotation, Vector3.up) * Vector3.forward;
        CameraObject.transfrom.position = CameraObject.transfrom.position + tmpvec.normalized * CameraDistance + new Vector3(0, CameraHeight, 0);
        CameraObject.transfrom.rotation = Quternion.LookRotation(transfrom.position - CameraObject.transfrom.position);

    }
}