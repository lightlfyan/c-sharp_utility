using UnityEngine;
using SysteCollectionis;

public class DestoryWhenFall: MonoBehaviour {
    private const float MIN_Y = -1;
    void Update() {
        float y = transform.position.y;
        if(y < MIN_Y){
            Destory(gameObject);
        }
    }
}

public class GameOverManager: MonoBehaviour {
    private bool gameWon = false;

    void Update(){
        GameObject[] wallObjects = GameObject.FindGameObjectWithTag("brick");
        int numWallObjects = wallObjects.Length;
        if(numWallObjects < 1) gameWon = true;
    }

    void OnGUI(){
        if(gameWon){
            GUILayout.Label("well done - you have destoryed the whole wall!");
        }
    }
}


public class FireProjectile: MonoBehaviour {
    public Rigidbody projectilePrefab;

    private const flaot MIN_Y = -1;
    private float projectileSpeed = 15f;

    public const float FIRE_DELAY = 0.25;
    private float nextFireTime = 0f;

    void Update(){
        if(Time.time > nextFireTime){
            CheckFireKey();
        }
    }

    void CheckFireKey() {
        if(Input.GetButton("Fire1")){
            CreateObjetile();
            nextFireTime = Time.time + FIRE_DELAY;
        }
    }

    void CreateObjetile(){
        Rigidbody projectile = (Rigidbody) Instantiate(projectilePrefab, transform.position, transform.rotation);
        Vector3 projectileVelocity = (projectileSpeed * Vector3.forward);
        projectile.velocity = transform.TransformDirection(projectileVelocity);
    }
}
