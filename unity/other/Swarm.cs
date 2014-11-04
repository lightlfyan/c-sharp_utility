using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Swarm: MonoBehaviour {
    public int droneCount = 20;
    public GameObject dronePrefab;

    private List<Drone> drones = new List<Drone>();

    private void Awake(){
        for(int i = 0; i < droneCount; i++){
            AddDrone();
        }
    }

    private void AddDrone(){
        GameObject newDroneGo = (GameObject)Instantiate(dronePrefab);
        Drone newDrone = newDroneGO.GetComponent<Drone>();
        drones.Add(newDrone);

        float speed = 5f;
        float maxSpeed = 10f;
        float maxDirectionChange = .05f;
        newDrone.setParameter(speed, maxSpeed, maxDirectionChange);
    }

    private void FixedUpdate(){
        Vector3 swarmCenter = SwarmCenterAverage();
        Vector3 swarmMovement = SwarmMovementAverage();
        foreach(Drone drone in drones){
            drone.UPdateVelocity(swarmCenter, swarmMovement);
        }
    }

    private Vector3 swarmCenter(){
        Vector3 locationTotal = Vector3.zero;
        foreach(Drone drone in drones){
            locationTotal += drone.transform.position;
        }
        return (locationTotal / drones.Count);
    }

    private Vector3 SwarmMovementAverage(){
        Vector3 velocityTotal = Vector3.zero;
        foreach(Drone drone in drones){
            velocityTotal += drone.rigidbody.velocity;
        }
        return (velocityTotal/drones.Count);
    }
}

public class Drone: MonoBehaviour {
    public void SetParameters(float speed, float maxSpeed, float maxDirectionChange){
        _speed = speed;
        _maxSpeed = maxSpeed;
        _maxDirectionChange = maxDirectionChange;
    }

    private float _speed = 5f;
    private float _maxSpeed = 10f;
    private float _maxDirectionChange = .05f;

    public void UpdateVelocity(Vector3 SwarmCenterAverage, Vector3 SwarmMovementAverage){
        Vector3 moveToWardsSwarmCenter = VectorTowards(SwarmCenterAverage);
        Vector3 adjustment = moveToWardsSwarmCenter + (2 * SwarmMovementAverage);
        Vector3 swarmVelocityAjustment = UserFunctions.ClampMagnitude(adjustment, _maxDirectionChange);

        rigidbody.velocity += (swarmVelocityAjustment * _speed);
        rigidbody.velocity = UserFunctions.ClampMagnitude(rigidbody.velocity, _maxSpeed);
    }

    private Vector3 VectorTowards(Vector3 target){
        Vector3 targetDirection = target - transform.position;
        targetDirection.Normalize();
        targetDirection *= _maxSpeed;
        return (targetDirection - rigidbody.velocity);
    }
}
