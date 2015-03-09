using UnityEngine;
using System.Collections;

public class tip : MonoBehaviour
{

    public GameObject target;
    
    RectTransform rtf;

    // Use this for initialization
    void Start ()
    {
        rtf = GetComponent<RectTransform> ();
    }
	
    // Update is called once per frame
    void Update ()
    {
        rtf.anchoredPosition = RectTransformUtility.WorldToScreenPoint (Camera.main, target.transform.position);
    }
}
