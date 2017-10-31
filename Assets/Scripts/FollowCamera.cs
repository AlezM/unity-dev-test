using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

	public Transform[] targets;
	public Vector3 offset = Vector3.zero;
	public bool useLerp = false;
	public float speed = 15f;

	int currentTargetIndex;

	void Start () 
	{
		transform.position = targets[currentTargetIndex].position + offset;

		targets [currentTargetIndex].GetComponent<BallController> ().inFocus = true;
	}

	void Update () 
	{
		if (targets[currentTargetIndex]) {
			if (useLerp) 
			{
				transform.position = Vector3.Lerp (transform.position, targets [currentTargetIndex].position + offset, Time.deltaTime * speed);
			} 
			else 
			{
				transform.position = targets [currentTargetIndex].position + offset;
			}

		//	transform.LookAt (targets[currentTargetIndex]);

			if (Input.GetMouseButton (0)) 
			{
				float h = Input.GetAxis ("Mouse X");
				float v = Input.GetAxis ("Mouse Y");

				transform.Rotate (Vector3.up, h * 10, Space.World);
				transform.Rotate (transform.right, v * 10, Space.World);
			}
		}	
	}

	void OnGUI () 
	{
		if (GUI.Button (new Rect (0, 0, 50, 20), "Left")) 
		{
			targets [currentTargetIndex].GetComponent<BallController> ().Focused(false);

			currentTargetIndex = (currentTargetIndex + 1) % targets.Length;

			targets [currentTargetIndex].GetComponent<BallController> ().Focused(true);
		}

		if (GUI.Button (new Rect (50, 0, 50, 20), "Right")) 
		{
			targets [currentTargetIndex].GetComponent<BallController> ().Focused(false);

			currentTargetIndex = (currentTargetIndex > 0)? currentTargetIndex - 1 : targets.Length - 1;

			targets [currentTargetIndex].GetComponent<BallController> ().Focused(true);
		}
	}
}
