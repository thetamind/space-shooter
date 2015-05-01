using UnityEngine;
using System.Collections;

[System.Serializable]
public class Done_Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController : MonoBehaviour
{
	public float speed;
	public float rotationSpeed;
	public Done_Boundary boundary;

	ParticleSystem[] particleSystems;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	 
	private float nextFire;

	void Awake()
	{
		particleSystems = GetComponentsInChildren<ParticleSystem>();
	}
	
	void Update ()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			audio.Play ();
		}
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		// Directly control rotation without inertia
		Vector3 rotationEulerAngles = rigidbody.rotation.eulerAngles;
		rotationEulerAngles.y += moveHorizontal * rotationSpeed;

		rigidbody.MoveRotation(Quaternion.Euler(rotationEulerAngles));
		foreach (var system in particleSystems) {
			system.startRotation = rotationEulerAngles.y * Mathf.Deg2Rad;
		}

		// Add thrust with inertia
		float thrust = moveVertical * speed;
		rigidbody.AddForce(transform.forward * thrust, ForceMode.Acceleration);
		
		rigidbody.position = new Vector3
		(
			Wrap(rigidbody.position.x, boundary.xMin, boundary.xMax),
			0.0f, 
			Wrap(rigidbody.position.z, boundary.zMin, boundary.zMax)
		);
	}

	float Wrap(float position, float min, float max)
	{
		if (position < min) return max;
		if (position > max) return min;
		return position;
	}
}
