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

				Vector3 relativePosition = spawnPosition - center;
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