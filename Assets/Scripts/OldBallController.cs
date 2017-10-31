using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldBallController : MonoBehaviour {

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

	[SerializeField] string jsonFileName;
	[SerializeField] float ballSpeed;
	float prevBallSpeed;

	TextAsset json;
	BallPath path;

	int pointIndex = 0;
	Vector3 goalPosition;

	void Start ()
	{
		json = Resources.Load<TextAsset> (jsonFileName);
		path = JsonUtility.FromJson<BallPath> (json.text);

		prevBallSpeed = ballSpeed;
	}

	void Update ()
	{
		transform.position = Vector3.Lerp (transform.position, goalPosition, Time.deltaTime * ballSpeed / 4);

		if (ballSpeed != prevBallSpeed) 
		{
			CancelInvoke ();
			if (pointIndex != 0) 
			{
				Invoke ("Move", 1 / ballSpeed);
			}
			prevBallSpeed = ballSpeed;
		}
	}

	public void InputHandler (InputInfo inputInfo) 
	{
		if (!IsInvoking ()) 
		{
			transform.position = path.At (0);
			Invoke ("Move", 0);
			Debug.Log ("New Invoke!");
		}
	}

	void Move () 
	{
		if (pointIndex >= path.Length ()) 
		{
			pointIndex = 0;
			CancelInvoke ();
			return;
		}
			
		goalPosition = path.At (pointIndex);
		pointIndex++;

		Invoke ("Move", 1 / ballSpeed);
	}

	void OnGUI () 
	{
		ballSpeed = GUI.HorizontalSlider (new Rect (Screen.width/2 - 100, Screen.height - 30, 200, 20), ballSpeed, 0, 25);
	}
}
