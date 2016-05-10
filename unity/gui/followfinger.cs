using UnityEngine;
using System.Collections;


public class followfinger : MonoBehaviour
{

	public Vector3 target;
	private Vector3 speed;

	public bool move = false;

	private Quaternion rot;

	// Use this for initialization
	void Start()
	{
		target = transform.position;
	}
	
	// Update is called once per frame
	void Update()
	{

		if (move && Input.GetMouseButton(0)) {
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
				if (hit.collider.gameObject != this.gameObject) {
					target = hit.point + new Vector3(0, 1, 0);
				}
			}
		}


		if ((target - transform.position).sqrMagnitude < 0.25) {
			return;
		}
		var newpos = Vector3.SmoothDamp(transform.position, target, ref speed, 0.1f);
		Debug.DrawLine(transform.position, newpos, Color.red, 0.5f); 
		transform.position = newpos;
	}

	void OnMouseDown()
	{
		Debug.Log("click");
		move = true;
	}

	void OnMouseUp()
	{
		Debug.Log("out");
		move = false;
	}
}

