using UnityEngine;
using System.Collections;

public class Compass: MonoBehaviour{
    public Transform playerController;
    public Texture compassBackground;
    public Texture playerBlip;

    private void OnGUI(){
        Rect compassBackgroundRect = new Rect(0, 0, 128, 128);
        GUI.DrawTexture(compassBackgroundRect,compassBackground);
    }

    private Rect CalcPlayerBlipTextrueRect(){
        float angleDegrees = playerController.eulerAngles.y - 90;
        float angleRadians= angleDegrees * Mathf.Deg2Rad;

        float blipX = 16 * Mathf.Cos(angleRadians);
        float blipY = 16 * Mathf.Sin(angleRadians);

        blipX += 64;
        blipY += 64;

        return new Rect(blipX - 5, blipY-5, 10, 10);
    }
}
