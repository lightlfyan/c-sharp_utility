using UnityEngine;
using System.Collections;

public class DisableWhenInvisible: MonoBehaviour {
    public Transform player;

    void OnBecameVisible(){
        enabled = true;
        print("cube became visible again");
    }

    void OnBecameInvisible(){
        enabled = false;
        print("invisible");
    }

    private void OnGUI(){
        float d = Vector3.Distance(tranfomr.position, player.position);
        GUILayout.Label("distance from player to cube =" + d);
    }

      private void OnTriggerEnter(Collider hitObjectCollider) {
   if (hitObjectCollider.CompareTag("Player"))
   enabled = true;
   }
   private void OnTriggerExit(Collider hitObjectCollider) {
   if (hitObjectCollider.CompareTag("Player"))
   enabled = false;
   }

}
