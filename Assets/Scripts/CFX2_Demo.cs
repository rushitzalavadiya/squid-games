using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class CFX2_Demo : MonoBehaviour
{
	public bool orderedSpawns = true;

	public float step = 1f;

	public float range = 5f;

	private float order = -5f;

	public Material groundMat;

	public Material waterMat;

	public GameObject[] ParticleExamples;

	private int exampleIndex;

	private string randomSpawnsDelay = "0.5";

	private bool randomSpawns;

	private bool slowMo;

	private void OnMouseDown()
	{
		RaycastHit hitInfo = default(RaycastHit);
		if (GetComponent<Collider>().Raycast(Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition), out hitInfo, 9999f))
		{
			GameObject gameObject = spawnParticle();
			gameObject.transform.position = hitInfo.point + gameObject.transform.position;
		}
	}

	private GameObject spawnParticle()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(ParticleExamples[exampleIndex]);
		gameObject.transform.position = new Vector3(0f, gameObject.transform.position.y, 0f);
		gameObject.SetActive(value: true);
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			gameObject.transform.GetChild(i).gameObject.SetActive(value: true);
		}
		return gameObject;
	}

	private void OnGUI()
	{
		GUILayout.BeginArea(new Rect(5f, 20f, Screen.width - 10, 60f));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Effect");
		if (GUILayout.Button("<"))
		{
			prevParticle();
		}
		GUILayout.Label(ParticleExamples[exampleIndex].name, GUILayout.Width(210f));
		if (GUILayout.Button(">"))
		{
			nextParticle();
		}
		GUILayout.Label("Click on the ground to spawn selected particles", GUILayout.Width(150f));
		if (GUILayout.Button(CFX_Demo_RotateCamera.rotating ? "Pause Camera" : "Rotate Camera"))
		{
			CFX_Demo_RotateCamera.rotating = !CFX_Demo_RotateCamera.rotating;
		}
		if (GUILayout.Button(randomSpawns ? "Stop UnityEngine.Random Spawns" : "Start UnityEngine.Random Spawns", GUILayout.Width(140f)))
		{
			randomSpawns = !randomSpawns;
			if (randomSpawns)
			{
				StartCoroutine("RandomSpawnsCoroutine");
			}
			else
			{
				StopCoroutine("RandomSpawnsCoroutine");
			}
		}
		randomSpawnsDelay = GUILayout.TextField(randomSpawnsDelay, 10, GUILayout.Width(42f));
		randomSpawnsDelay = Regex.Replace(randomSpawnsDelay, "[^0-9.]", "");
		if (GUILayout.Button(GetComponent<Renderer>().enabled ? "Hide Ground" : "Show Ground", GUILayout.Width(90f)))
		{
			GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
		}
		if (GUILayout.Button(slowMo ? "Normal Speed" : "Slow Motion", GUILayout.Width(100f)))
		{
			slowMo = !slowMo;
			if (slowMo)
			{
				Time.timeScale = 0.33f;
			}
			else
			{
				Time.timeScale = 1f;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	private IEnumerator RandomSpawnsCoroutine()
	{
		while (true)
		{
			GameObject gameObject = spawnParticle();
			if (orderedSpawns)
			{
				gameObject.transform.position = base.transform.position + new Vector3(order, gameObject.transform.position.y, 0f);
				order -= step;
				if (order < 0f - range)
				{
					order = range;
				}
			}
			else
			{
				gameObject.transform.position = base.transform.position + new Vector3(UnityEngine.Random.Range(0f - range, range), 0f, UnityEngine.Random.Range(0f - range, range)) + new Vector3(0f, gameObject.transform.position.y, 0f);
			}
			yield return new WaitForSeconds(float.Parse(randomSpawnsDelay));
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
		{
			prevParticle();
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
		{
			nextParticle();
		}
	}

	private void prevParticle()
	{
		exampleIndex--;
		if (exampleIndex < 0)
		{
			exampleIndex = ParticleExamples.Length - 1;
		}
		if (ParticleExamples[exampleIndex].name.Contains("Splash") || ParticleExamples[exampleIndex].name.Contains("Skim"))
		{
			GetComponent<Renderer>().material = waterMat;
		}
		else
		{
			GetComponent<Renderer>().material = groundMat;
		}
	}

	private void nextParticle()
	{
		exampleIndex++;
		if (exampleIndex >= ParticleExamples.Length)
		{
			exampleIndex = 0;
		}
		if (ParticleExamples[exampleIndex].name.Contains("Splash") || ParticleExamples[exampleIndex].name.Contains("Skim"))
		{
			GetComponent<Renderer>().material = waterMat;
		}
		else
		{
			GetComponent<Renderer>().material = groundMat;
		}
	}
}
