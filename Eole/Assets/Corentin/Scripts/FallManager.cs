using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallManager : MonoBehaviour
{
	[Header("References")]
	public Mover moverRef;
	public Animator fader;

	[Header("Booleans")]
	public bool grounded;

	[Header("Values")]
	public int secondsSaved;
	float cycler;
	public int minAltitudeToTriggerRespawn;
	public float respawnDelay;

	Vector3[] lastPos;

	void Awake()
	{
		fader = GameObject.Find("Fader").GetComponent<Animator>();
		moverRef = GetComponent<Mover>();
		cycler = 0;
		lastPos = new Vector3[secondsSaved + 1];
	}

	void Update()
    {
		grounded = moverRef.grounded;

		if (grounded)
		{
			lastPos[Mathf.RoundToInt(cycler)] = transform.position;
			if (cycler > secondsSaved)
			{
				cycler = 0;
			}
			cycler += Time.deltaTime;
		}

		if (transform.position.y < lastPos[0].y - minAltitudeToTriggerRespawn)
		{
			StartCoroutine(Respawn(lastPos[0]));
			fader.SetBool("InstantFade", true);
		}
    }

	IEnumerator Respawn(Vector3 target)
	{
		yield return new WaitForSeconds(respawnDelay);
		transform.position = target;
		yield return new WaitForSeconds(0.1f);
		fader.SetBool("InstantFade", false);
	}
}
