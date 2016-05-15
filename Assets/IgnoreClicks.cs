using UnityEngine;

public class IgnoreClicks : MonoBehaviour, ICanvasRaycastFilter
{
	public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
	{
		return false;
	}
}