using UnityEngine;
using System.Collections;

public class CenterController3 : CenterController {

	public void OnSliderChange(float priority)
	{
		Store.Priority = (int)(priority * 10);
	}
}
