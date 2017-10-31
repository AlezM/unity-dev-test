using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour {

	[System.Serializable]
	public class InputEvent : UnityEvent<InputInfo> {}

	[SerializeField] float doubleClickDelay = 0.4f;

	[SerializeField] InputEvent onClick;

	private float timer = 0;

	Camera cam;

	void Start () 
	{
		cam = Camera.main;
	}

	void Update () {
		timer += Time.deltaTime;
		if (Input.GetMouseButtonDown (0)) {
			bool doubleClick = (timer < doubleClickDelay);

			RaycastHit hit;
			Physics.Raycast (cam.ScreenPointToRay (Input.mousePosition), out hit, 100);

			if (hit.collider != null) 
			{
				BallController ballController = hit.collider.GetComponent<BallController> ();
				if (ballController != null) 
				{
					ballController.InputHandler (new InputInfo (Input.mousePosition, doubleClick));
				}
			}

		//	onClick.Invoke (new InputInfo (Input.mousePosition, doubleClick));

			timer = 0;
		}
	}
}