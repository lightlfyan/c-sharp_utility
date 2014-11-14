using UnityEngine;
using System.Collections;

public class TimedMethod: MonoBehaviour {
    private void Start(){
        StartCoroutine(Tick());
    }

    private IEnumerator  Tick(){
        float delaySeconds = 5.0f;
        while(true){
            print("tick " + Time.time);
            yield return new WaitForSeconds(delaySeconds);
        }
    }
}
