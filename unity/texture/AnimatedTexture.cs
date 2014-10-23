using UnityEngine;
using System.Collections;

// plane rotation (-90, 0, 0)

public class AnimatedTExture: Monobehaviour {
    public float frameInterval = 0.9f;
    public Texture2D[] imageArray;
    private int imageIndex = 0;

    private void Awake(){
        if(imageArray.Length < 1){
            // debug
        } else {
            StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation() {
        while(true){
            ChangeImage():
            yield return new WaitForSeconds(frameInterval);
        }
    }

    private void ChangeImage(){
        imageIndex++;
        imageIndex = imageIndex % imageArray.Length;
        Texture2D nextImage = imageArray[imageIndex];
        render.material.SetTexture("_MainTex", nextImage);
    }
}
