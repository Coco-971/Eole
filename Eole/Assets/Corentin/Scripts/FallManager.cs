﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallManager : MonoBehaviour
{
	[Header("References")]
	public Mover moverRef;
	public Animator fader;
	
	//SFX
	public PlayerSFXManager playerSFXManager;

	[Header("Booleans")]
	public bool grounded;
	public bool respawning;

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

		respawning = false;

		//SFX
		playerSFXManager = GetComponent<PlayerSFXManager>();
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

		if (transform.position.y < lastPos[0].y - minAltitudeToTriggerRespawn && !respawning)
		{
			StartCoroutine(Respawn(lastPos[0]));
			fader.SetBool("InstantFade", true);
			respawning = true;
		}
    }

	IEnumerator Respawn(Vector3 target)
	{
		//SFX
		playerSFXManager.DyingSFX();

		yield return new WaitForSeconds(respawnDelay);
		transform.position = target;

		//SFX
		playerSFXManager.RespawnSFX();

		yield return new WaitForSeconds(0.1f);
		fader.SetBool("InstantFade", false);
		respawning = false;
	}
}
