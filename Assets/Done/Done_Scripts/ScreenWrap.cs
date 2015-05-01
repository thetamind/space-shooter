using UnityEngine;
using System.Collections;

public class ScreenWrap : MonoBehaviour
{
	private Camera cam;

	bool isWrappingX;
	bool isWrappingY;

	void Start()
	{
		cam = Camera.main;
		if (cam == null)
		{
			Debug.LogWarning("Can't find main camera. Skipping.");
			DestroyImmediate(this);
		}
	}

	void OnBecameVisible()
	{
		Debug.Log("Became visible");
// 		DoWrap();
	}

	void OnBecameInvisible()
	{
		Debug.Log("Became INvisible");
// 		DoWrap();
	}
	
	void LateUpdate()
	{
		DoWrap();
	}

	private string prevPosLog;
	void DoWrap()
	{
		var viewportPosition = cam.WorldToViewportPoint(transform.position);
		var newViewportPosition = new Vector3(
			viewportPosition.x % 1.0f,
			viewportPosition.y % 1.0f,
			viewportPosition.z
		);
		
		
		if (viewportPosition != newViewportPosition)
		{
			Debug.Log("WRAPPED");
			Vector3 newWorldPosition = cam.ViewportToWorldPoint(newViewportPosition);
			
			string toLog = string.Format("Viewport: ({0:N2}, {1:N2}, {2:N2})  World: ({3:N2}, {4:N2}, {5:N2})",
				viewportPosition.x, viewportPosition.y, viewportPosition.z,
				newWorldPosition.x, newWorldPosition.y, newWorldPosition.z
				);
			
			if (prevPosLog != toLog) Debug.Log(toLog);
			prevPosLog = toLog;
			
			transform.position = newWorldPosition;
		}
		
		
		
		
// 		string toLog = string.Format("Viewport: ({0:N2}, {1:N2}, {2:N2})  wrappingX: {3} wrappingY: {4}", viewportPosition.x, viewportPosition.y, viewportPosition.z,
// 			isWrappingX, isWrappingY);
// 			
// 		if (prevPosLog != toLog) Debug.Log(toLog);
// 		prevPosLog = toLog;
		
// 		var withinViewport = true;
// 		if (viewportPosition.x > 1 || viewportPosition.x < 0) withinViewport = false;
// 		if (viewportPosition.y > 1 || viewportPosition.y < 0) withinViewport = false;
// 		
// 		var isVisible = gameObject.renderer.isVisible;
// 		var isVisible = withinViewport;
// 
// 		if (isVisible)
// 		{
// 			isWrappingX = false;
// 			isWrappingY = false;
// 			return;
// 		}

// 		if (isWrappingX && isWrappingY)
// 		{
// 			return;
// 		}

// 		var newPosition = transform.position;
// 		var newViewportPosition = viewportPosition;

// 		if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
// 		{
// 			float newViewport = viewportPosition.x % 1.0f;
// 			newViewportPosition.x = newViewport;
// 			Vector3 nnn = cam.ViewportToWorldPoint(newViewportPosition);
// 			Debug.LogWarning(string.Format("Wrapping X: {0:N2} => {1:N2}", newPosition.x, nnn.x));
// 			
// 			newPosition.x = nnn.x;
// 			//newPosition.x = -newPosition.x;
// 
// 			isWrappingX = true;
// 		}
// 
// 		if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
// 		{
// 			float newViewport = viewportPosition.y % 1.0f;
// 			newViewportPosition.x = newViewport;
// 			Vector3 nnn = cam.ViewportToWorldPoint(newViewportPosition);
// 			Debug.LogWarning(string.Format("Wrapping Y: {0:N2} => {1:N2}", newPosition.z, nnn.z));
// 			
// 			newPosition.z = nnn.z;
// 			//newPosition.z = -newPosition.z;
// 
// 			isWrappingY = true;
// 		}
// 
// 		transform.position = newPosition;
	}
}
