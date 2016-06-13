using UnityEngine;
using System.Collections;

public class ellipse : MonoBehaviour {

	public GameObject center;

	public float a=3;
	public float b=5;

	public float angle = 0;
	public float rot = 30;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		angle += 100 * Time.deltaTime;
		var x = a * Mathf.Cos(Mathf.Deg2Rad * angle);
		var y = b * Mathf.Sin(Mathf.Deg2Rad * angle);

//		var forward =  Quaternion.AngleAxis(rot, Vector3.forward) * new Vector3 (x, y, 0);
		var forward = Quaternion.Euler (new Vector3 (rot, rot, rot)) * new Vector3 (x, y, 0);

		transform.position = center.transform.position + forward;
		return;
	}
}
