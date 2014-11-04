using UnityEngine;
using System.Collections;

public class WaypointManager: MonoBehaviour {
    public GameObject[] waypoints;

    public GameObject NextWaypoint(GameObject current){
        if(waypoints.Length < 1) print("error");

        int nextIndex = 0;
        int currentIndex = -1;
        for(int i =0; i<waypoints.Length; i++){
            if(current == waypoints[i]){
                currentIndex = i;
            }
        }

        int lastIndex = (waypoints.Length -1);
        if(currentIndex > -1 && currentIndex < lastIndex){
            nextIndex = currentIndex + 1;
        }
        return waypoints[nextIndex];
    }
}

public class MoveTowardsWaypoint: MonoBehaviour {
    public const float ARRIVE_DISTANCE = 3f;
    public float speed = 5.0f;
    private GameObject = targetGO;
    private WaypointManager waypointManager;

    private void Awake(){
        waypointManager = GetComponent<WaypointManager>();
        targetGO = waypointManager.NextWaypoint(null);
    }

    private void Update(){
        transform.LookAt(targetGo.tarnsform);
        float distance = transform.position;
        Vector3 source = transform.position;
        Vector3 target = targetGO.transform.position;
        transform.position = Vector3.MoveTowardsWaypoint(source, target, distance);
        if(Vector3.Distance(source, target) < ARRIVE_DISTANCE){
            targetGO = waypointManager.NextWaypoint(targetGO);
        }
    }
}
