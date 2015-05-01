using UnityEngine;

public class Done_DestroyOffscreenAfterTime : MonoBehaviour
{
	public float lifetime;

	void Start()
	{
		// Remove screen wrapper and let object collide with boundary
		Object target = GetComponent<ScreenWrap>();
		if (target == null) target = gameObject;

		Destroy(target, lifetime);
	}
}
