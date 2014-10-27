using UnityEngine;
using System.Collections;

public class Radar: MonoBehaviour {
    const float MAX_DISTANCE = 20f;
    const int RADAR_SIZE = 128;

    public Transform playerController;
    public Texture radarBackground;
    public Texture targetBlip;

    private void onGUI(){
        Rect radarBackgroundRect = new Rect(0, 0, RADAR_SIZE, RADAR_SIZE);
        GUI.DrawTexture(radarBackgroundRect, radarBackground);
        GameObject[] cubeGOArray = GameObject.FindGameObjectsWithTag("cube");

        Vector3 palyerPos = playerController.transform.position;
        foreach(GameObject cubeGo in cubeGOArray){
            Vector3 targetPos = cubeGO.transform.position;
            float distanceToTarget = Vector3.Distance(targetPos, playerPos);
            if((distanceToTarget <= MAX_DISTANCE)){
                DrawBlip(playerPos, targetPos, distanceToTarget);
            }
        }
    }

    private void DrawBlip(Vector3 playerPos, Vector3 targetPos, float distanceToTarget){
        float dx = targetPos.x - playerPos.x;
        float dz = targetPos.z - playerPos.z;
        float angleToTarget = Mathf.Atan2(dx, dz) * Mathf.Rad2Deg;

        float anglePlayer = playerController.eulerAngles.y;
        float angleRadarDegrees = angleToTarget - anglePlayer - 90;

        float normalisedDistance = distanceToTarget / MAX_DISTANCE;
        float angleRadians = angleRadarDegrees * Mathf.Deg2Rad;
        float blipX = normalisedDistance * Mathf.Cos(angleRadians);
        float blipY = normalisedDistance * Mathf.Sin(angleRadians);

        blipX *= RADAR_SIZE/2;
        blipY *= RADAR_SIZE/2;

        blipX += RADAR_SIZE/2;
        blipY += RADAR_SIZE/2;
        Rect blipRect = new Rect(blipX - 5, blipY - 5, 10, 10);
        GUI.DrawTexture(blipRect, targetBlip);

    }
}
