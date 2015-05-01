using UnityEngine;
using System.Collections;

public class ScreenWrap : MonoBehaviour
{
	private Camera cam;

	public Vector3 viewportPos;
	public Vector3 worldPos;
	
	public Vector3 maybeViewportPos;

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
		
		var sb = new System.Text.StringBuilder();
		for(int i = 0; i < 100; i++)
		{
			float x = (i * 0.1f) - 5.0f;
			float y = Mathf.Abs(x % 1.0f);
			sb.Append(x.ToString("N2"));
			sb.Append(" - ");
			sb.Append(y.ToString("N2"));
			sb.AppendLine();
		}
		
		Debug.Log(sb.ToString());
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
		this.viewportPos = cam.WorldToViewportPoint(transform.position);
		this.worldPos = transform.position;
	}

	private string prevPosLog;
	void DoWrap()
	{
		var worldPosition = transform.position;
		var viewportPosition = cam.WorldToViewportPoint(worldPosition);
		var newViewportPosition = new Vector3(
			viewportPosition.x % 1.0f,
			viewportPosition.y % 1.0f,
			viewportPosition.z
		);
		

		
// if (x < x_min)
//     x = x_max - (x_min - x) % (x_max - x_min);
// else
//     x = x_min + (x - x_min) % (x_max - x_min);
		
		Vector3 newWorldPosition = cam.ViewportToWorldPoint(newViewportPosition);
		newWorldPosition.y = worldPos.y;
			
		this.maybeViewportPos = newViewportPosition;
		
		if (newViewportPosition.x < 0)
		{
			maybeViewportPos.x = 1.0f - (0.0f - newViewportPosition.x) % (1.0f - 0.0f);
			newViewportPosition.x = maybeViewportPos.x; 
		}
		
		if (newViewportPosition.y < 0)
		{
			maybeViewportPos.y = 1.0f - (0.0f - newViewportPosition.y) % (1.0f - 0.0f); 
			newViewportPosition.y = maybeViewportPos.y; 
		}
		
		newWorldPosition = cam.ViewportToWorldPoint(newViewportPosition);
		newWorldPosition.y = worldPos.y;
		
		
		if (viewportPosition != newViewportPosition)
		{
// 			Vector3 newWorldPosition = cam.ViewportToWorldPoint(newViewportPosition);
// 			newWorldPosition.y = worldPos.y;
			
						
			string before = string.Format("B=> Viewport: ({0:N2}, {1:N2}, {2:N2})  World: ({3:N2}, {4:N2}, {5:N2})",
				viewportPosition.x, viewportPosition.y, viewportPosition.z,
				worldPosition.x, worldPosition.y, worldPosition.z
				);
				
			string after = string.Format("A=> Viewport: ({0:N2}, {1:N2}, {2:N2})  World: ({3:N2}, {4:N2}, {5:N2})",
				newViewportPosition.x, newViewportPosition.y, newViewportPosition.z,
				newWorldPosition.x, newWorldPosition.y, newWorldPosition.z
				);
			
			Debug.Log(string.Format("{0}\n{1}", before, after));
			
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
