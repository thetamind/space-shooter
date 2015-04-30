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
	public float tilt;
	public float currentRotation = 0f;
	public float rotationSpeed;
	public Done_Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	 
	private float nextFire;
	
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

		float rotationDegreesDelta = moveHorizontal * rotationSpeed;
		float impulse = moveVertical * 5.0f;

		Vector3 movement = new Vector3 (0.0f, 0.0f, moveVertical) * speed;
		float thrust = moveVertical * speed;
		Vector3 move;
		rigidbody.AddForce(transform.forward * thrust, ForceMode.Acceleration);
		
		rigidbody.position = new Vector3
		(
			Mathf.Clamp (rigidbody.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (rigidbody.position.z, boundary.zMin, boundary.zMax)
		);

		currentRotation += rotationDegreesDelta;
		//rigidbody.rotation = Quaternion.Euler (0.0f, rotationDegreesDelta, rigidbody.velocity.x * -tilt);
		rigidbody.MoveRotation(Quaternion.Euler(0f, currentRotation, 0f));
	}
}
