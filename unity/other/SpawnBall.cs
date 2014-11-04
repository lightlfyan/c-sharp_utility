using UnityEngine;
using System.Collections;

public class SpawnBall: MonoBehaviour {
    public GameObject prefabBall;
    public GameObject[] respawns;
    public const float FIRE_DELAY = 0.25f;

    private float nextFireTime = 0f;

    private void Start(){
        respawns = GameObject.FindGameObjectsWithTag("Respawn");
    }

    void Updte(){
        if(Time.time > nextFireTime){
            CheckFireKey();
        }
    }

    void CheckFireKey(){
        if(Input.GetButton("Fire1")){
            CreateSphere();
            nextFireTime = Time.time + FIRE_DELAY;
        }
    }

    private void CreateSphere(){
        int r = Random.Range(0, respawns.Length);
        GameObject spawnPoint = respawns[r];
        if(spawnPoint){
            Instantiate(prefab_ball, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }
}
