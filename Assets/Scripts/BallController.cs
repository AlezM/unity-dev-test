using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

	class BallPath {
		public float[] x;
		public float[] y;
		public float[] z;

		public Vector3 At (int i) 
		{
			int length = x.Length;
			return new Vector3 (x [i %  length], y [i %  length], z [i %  length]);
		}

		public int Length () 
		{
			return x.Length;
		}
	}

	[SerializeField] string jsonFileName = "ball_path";
	[SerializeField] float ballSpeed;
	[SerializeField] bool useLerp = false;

	public bool inFocus;

	TextAsset json;
	BallPath path;

	int pointIndex = 0;
	bool moving = false;
	float timer = 0;
	Vector3 goalPosition;

	LineRenderer pathLine;

	void Start ()
	{
		json = Resources.Load<TextAsset> (jsonFileName);
		path = JsonUtility.FromJson<BallPath> (json.text);
		pathLine = GetComponent<LineRenderer> ();
		pathLine.positionCount = 0;

		transform.position = path.At (0);
		goalPosition = path.At (0);
	}

	void Update ()
	{
		if (useLerp) 
		{
			transform.position = Vector3.Lerp (transform.position, goalPosition, 0.1f);
		} 
		else 
		{
			transform.position = goalPosition;
		}

		if (pointIndex > 0) {
			pathLine.SetPosition (pointIndex, transform.position);
		}

		if (moving) {
			timer += Time.deltaTime;		

			if ((timer > 1.0f / ballSpeed)) {
				timer = 0;

				if (pointIndex >= path.Length () - 1) {
					moving = false;
					return;
				}

				pointIndex++;
				goalPosition = path.At (pointIndex);

				pathLine.positionCount = pointIndex + 1;
				pathLine.SetPosition (pointIndex, transform.position);
			}
		}
	}

	public void InputHandler (InputInfo inputInfo) 
	{
		if (!moving || inputInfo.doubleClick) 
		{
			transform.position = path.At (0);
			goalPosition = path.At (0);
			pointIndex = 0;
		//	timer = 0;
			moving = true;

			pathLine.positionCount = pointIndex + 1;
			pathLine.SetPosition (pointIndex, path.At (pointIndex));
		}
	}

	public void Focused (bool focus) {
		inFocus = focus;
		if (!focus && moving)
		{
			ballSpeed = 0;
		}
	}

	void OnGUI ()
	{
		if (inFocus)
		{
			ballSpeed = GUI.HorizontalSlider (new Rect (Screen.width / 2 - 100, Screen.height - 30, 200, 20), ballSpeed, 0, 100);
		}
	}
}
