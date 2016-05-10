using UnityEngine;
using System.Collections;

public class followfinger : MonoBehaviour {

	public Vector3 target ;
	private Vector3 speed;

	public bool move = false;

	private Quaternion rot;

	// Use this for initialization
	void Start () {
		target = transform.position;
		rot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {

		if(move && Input.GetMouseButton(0)){
			var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				if(hit.collider.gameObject != this.gameObject){
					target = hit.point;
				}
			}
		}

		var head = transform.position + transform.forward * 0.5f;
		var tail = transform.position - transform.forward * 0.5f;


		if((target - transform.position).sqrMagnitude < 0.25){
			return;
		}

		if((target-head).sqrMagnitude > (target-tail).sqrMagnitude){
			transform.rotation = Quaternion.LookRotation (target - transform.position);
		} else {
			transform.rotation = Quaternion.LookRotation (transform.position - target);
		}

		transform.position = Vector3.SmoothDamp (transform.position, target, ref speed, 0.1f);
	}

	void OnMouseDown() {
		Debug.Log ("click");
		move = true;
	}

	void OnMouseUp() {
		Debug.Log ("out");
		move = false;
	}
}
