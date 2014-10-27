using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickUp: MonoBehaviour {
    public enum PickUpCategory{
        KEY, HEALTH, SCORE
    }

    public Texture icon;
    public int points;
    public string fitsLockTag;
    public PickUpCategory catgegory;
}

public class GeneralInventory: MonoBehaviour {
    const int ICON_HEIGHT = 32;
    private List<PickUP> inventory = new List<PickUP>();

    private void OnGUI(){
        Rect r = new Rect(0, 0, Screen.width/2, ICON_HEIGHT);
        GUILayout.BeginArea(r);
        GUILayout.BeginHoriazontal();
        DispalyInventory();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void DispalyInventory(){
        inventory.Foreach(item -> GUILayout.Label(item.icon));
    }

    private void OnTriggerEnter(Collider hitCollider){
        if("pickup" == hitCollider.tag){
            PickUp item = hitCollider.GetComponent<PickUp)();
            inventory.Add(item);
            Destory(hitCollider.gameObject);
        }
    }
}
