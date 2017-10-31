using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputInfo 
{
	public Vector2 clickPosition;
	public bool doubleClick;

	public InputInfo (Vector2 clickPosition, bool doubleClick)
	{
		this.clickPosition = clickPosition;
		this.doubleClick = doubleClick;
	}
}
