using UnityEngine;
using System.Collections;

public class FadingMessage: MonoBehaviour{
    const float DURATION = 2.5f;
    private void Update(){
        if(Time.time > DURATION) {
            Destory(gameObject);
        }

        Color newColor = guiText.material.color;

        float proportion = (Time.time/DURATION);
        newColor.a = Mathf.Lerp(1, 0, proportion);
        guiText.meterial.color = newColor;
    }

    private void CreateMessage(string message){
        GameObject fadingMessageGo = Instantiate(fadingMessagePrefab) as GameObject;
        fadingMessageGo.guiText.text = message;
    }

}
