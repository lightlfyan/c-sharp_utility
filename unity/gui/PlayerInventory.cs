using UnityEngine;
using System.Collections;

public class PlayerInventory: MonoBehaviour{
    private bool isCarryingKey = false;

    public Texture keyIcon;
    public Texture emptyIcon;

    private void onGUI(){
        string keyMessage = "bag is empty";
        if(isCarryingKey){
            GUILayout.Label(keyIcon);
            keyMessage = "carrying: [key]";

        } else {
            GUILayout.Label(emptyIcon);
        }
        GUILayout.Label(keyMessage);
    }

    private void OnTriggerEnter(Collider hitCollider){
        if("key" == hitCollider.tag){
            isCarryingKey = true;
            Destory(hitCollider.gameObject);
        }
    }
}
