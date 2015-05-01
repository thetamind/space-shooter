using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
	private Camera cam;

	public Vector3 viewportPos;
	public Vector3 worldPos;

	void Start()
	{
		cam = Camera.main;
		if (cam == null)
		{
			Debug.LogWarning("Can't find main camera. Skipping.");
			DestroyImmediate(this);
		}
	}
	
	float Wrap(float position, float min, float max)
	{
		if (position < min) return max - (min - position) % (max - min);
		if (position > max) return min + (position - min) % (max - min);
		return position;
	}
	
	void LateUpdate()
	{
		DoWrap();
		this.viewportPos = cam.WorldToViewportPoint(transform.position);
		this.worldPos = transform.position;
	}

	void DoWrap()
	{
		var worldPosition = transform.position;
		var viewportPosition = cam.WorldToViewportPoint(worldPosition);
		
		var newViewportPosition = new Vector3(
			Wrap(viewportPosition.x, 0.0f, 1.0f),
			Wrap(viewportPosition.y, 0.0f, 1.0f),
			viewportPosition.z
		);
		
		Vector3 newWorldPosition = cam.ViewportToWorldPoint(newViewportPosition);
		// Lock object to plane; seems to shift due to camera projection
		newWorldPosition.y = worldPos.y;
		
		if (viewportPosition != newViewportPosition)
		{
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
	}
}
