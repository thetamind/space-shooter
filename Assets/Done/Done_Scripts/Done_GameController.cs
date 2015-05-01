using UnityEngine;
using System.Collections;

public class Done_GameController : MonoBehaviour
{
	public GameObject[] hazards;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	
	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;
	
	private bool gameOver;
	private bool restart;
	private int score;
	
	void Start ()
	{
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		UpdateScore ();
		StartCoroutine (SpawnWaves ());
	}
	
	void Update ()
	{
		if (restart)
		{
			if (Input.GetKeyDown (KeyCode.R))
			{
				Application.LoadLevel (Application.loadedLevel);
			}
		}
	}
	
	IEnumerator SpawnWaves ()
	{
		Transform boundary = GameObject.FindGameObjectWithTag("Boundary").GetComponent<Transform>();
		Vector3 center = boundary.transform.position;

		yield return new WaitForSeconds (startWait);
		while (true)
		{

			Vector3 r = Random.onUnitSphere; // * boundary.collider.bounds.extents;

			for (int i = 0; i < hazardCount; i++)
			{
				// Random point within donut
				float rho = Random.Range(5.0f, 10.0f);
				float phi = Random.Range(0.0f, 2.0f * Mathf.PI);

				float x = Mathf.Sqrt(rho) * Mathf.Cos(phi);
				float y = Mathf.Sqrt(rho) * Mathf.Sin(phi);

				// Scale and transform to boundary
				x *= boundary.collider.bounds.size.x / 2;
				y *= boundary.collider.bounds.size.z / 2;
				Vector3 outerPoint = new Vector3(x, 0.0f, y + center.z);

				// Linecast from outside boundary towards center
				RaycastHit hit;
				Physics.Linecast(outerPoint, center, out hit);
				Vector3 spawnPosition = hit.point;


				Vector2 rand = Random.insideUnitCircle * boundary.collider.bounds.size.magnitude;
				Vector3 target = new Vector3(rand.x + center.x, center.y, rand.y + center.z);

				Vector3 relativePosition = spawnPosition - target;
				Quaternion spawnRotation = Quaternion.LookRotation(relativePosition);

				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
			
			if (gameOver)
			{
				restartText.text = "Press 'R' for Restart";
				restart = true;
				break;
			}
		}
	}

	public void OnDrawGizmos()
	{
		Transform boundary = GameObject.FindGameObjectWithTag("Boundary").GetComponent<Transform>();
		Vector3 center = boundary.transform.position;

		Gizmos.color = Color.yellow;
		for(int i = 0; i < 25; i++)
		{
			float rho = Random.Range(5.0f, 10.0f);
			float phi = Random.Range(0.0f, 2.0f * Mathf.PI);

			float x = Mathf.Sqrt(rho) * Mathf.Cos(phi);
			float y = Mathf.Sqrt(rho) * Mathf.Sin(phi);

			x *= boundary.collider.bounds.size.x / 2;
			y *= boundary.collider.bounds.size.z / 2;
			Vector3 xxx = new Vector3(x, 0.0f, y + center.z);

			RaycastHit hit;
			Physics.Linecast(xxx, center, out hit);
//			Gizmos.color = Color.red;
//			Gizmos.DrawLine(xxx, center);
			Vector3 hitPoint = hit.point;


			Vector3 relativePos = hitPoint - center;
			Quaternion spawnRotation = Quaternion.LookRotation(relativePos);

			Vector3 close = boundary.collider.ClosestPointOnBounds(xxx);

			Vector3 pos = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
			Gizmos.color = Color.yellow;
// 			Gizmos.DrawSphere(hitPoint, 0.1f);
		}
	}
	
	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}
	
	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}
	
	public void GameOver ()
	{
		gameOverText.text = "Game Over!";
		gameOver = true;
	}
}