using UnityEngine;
using System.Collections;

public class CountdownTimer: MonoBehavior {
    private float secondsLeft = 3f;

    private void OnGUI(){
        if(secondsLeft > 0){
            GUILayout.Label("countdown seconds remaing = " + (int)secondsLeft);
        }
        else {
            GUILayout.Label("had finished");
        }
    }

    private void Update() {
        secondsLeft -= Time.deltaTime;
    }
}
