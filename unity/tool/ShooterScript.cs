using UnityEngine;
using System.Collections;

public class ShooterScript: MonoBehaviour {
    public Rigidbody projectile;
    public float projectileForce = 5000.0f;
    public bool compensateBulletSpeed = true;
    public float slowMotionBar;
    public Texture2D slowMotionBar;
    public float slowMotionTotalTime = 10.0f;
    public float recoverTimeRate = 0.5f;

    private bool isSlowMotionMode = false;
    private float remainingSloMoTime;
    private float startSloMoTimestamp;
    private float elapsedTime = 0.0f;
    private float actualForce;

    private void Start(){
        Screen.showCursor = false;
        remainingSloMoTime = slowMotionTotalTime;
        actualForce = projectileForce;
    }

    private void Update(){
        CheckUserInput();
        if(isSlowMotionMode)
            SlowMotionModeActions();
        else
            NormalTimeActions();
    }

    private void CheckUserInput(){
        if(Input.GetButtonDown("Fire1")){
            Rigidbody clone = (Rigidboy) Instantiate(projectile, Camera.main.transform.position, Camera.main.transform.rotation);
            clone.AddForce(clone.transform.forward * actualForce);
        }
        if(Input.GetButtonDown("Fire2") && remainingSloMoTime > 0){
            ActivateSloMo();
        }
        if(Input.GetButtonUp("Fire2")){
            DeactivateSloMo();
        }
    }

    private void SlowMotionModeActions(){
        elapsedTime = (Time.time - startSloMoTimestamp) * slowMotionSpeed;
        remainingSloMoTime = slowMotionTotalTime - elapsedTime;
        if(elapsedTime >= slowMotionTotalTime){
            DeactivateSloMo();
        }
    }

    private void NormalTimeActions(){
        if(remainingSloMoTime < slowMotionTotalTime){
            remainingSloMoTime += Time.deltaTime * recoverTimeRate;
        } else {
            remainingSloMoTime = slowMotionTotalTime;
        }
    }

    private void ActivateSloMo(){
        Time.tiemScale = 1.0f / slowMotionSpeed;
        Time.fixedDeltaTime = 0.02f / slowMotionSpeed;
        actualForce = projectileForce * slowMotionSpeed;
        isSlowMotionMode = true;

        float timeLeftToRefill = (slowMotionTotalTime - remainingSloMoTime) / slowMotionSpeed;
        startSloMoTimestamp = (Time.time - timeLeftToRefill);
    }

    private void DeactivateSloMo(){
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
        actualForce = projectileForce;
        isSlowMotionMode = false;
    }

    private void OnGUI(){
        float proportionRemaining = remainingSloMoTime/slowMotionTotalTime;
        float barDisplayWidth = slowMotionBar.width * proportionRemaining;

        int proportionRemaininInt = (int)(proportionRemaining * 100);
        string percentageString = proportionRemaininInt + "%";
        GUI.Label(new Rect(10, 15, 40, 20))
    }

}
