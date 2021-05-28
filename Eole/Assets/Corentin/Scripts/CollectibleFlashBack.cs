using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleFlashBack : MonoBehaviour
{
	[Header("References")]
	public GameObject closestAirColumn;
	public Transform player;
	public Collector collectorRef;
	
	//SFX VFX
    public CollectibleVFXManager collectibleVFXManager;
	public PlayerSFXManager playerSFXManager;

	[Header("Values")]
	public float maxDistanceToExit;

	public string text;

	[Header("Booleans")]
	public bool collectibleActivated;
	public bool alreadyActivated;

	public int activatedTimes;

	void Awake()
	{
		collectibleVFXManager = GetComponent<CollectibleVFXManager>();
		player = GameObject.Find("Player").transform;
		collectorRef = GameObject.Find("Camera").GetComponent<Collector>();
		playerSFXManager = FindObjectOfType<PlayerSFXManager>();

		alreadyActivated = false;

		closestAirColumn = FindClosestAirColumn();
	}

	public GameObject FindClosestAirColumn()
	{
		GameObject[] column;
		column = GameObject.FindGameObjectsWithTag("AirColumn");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject obj in column)
		{
			Vector3 diff = obj.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance)
			{
				closest = obj;
				distance = curDistance;
			}
		}
		return closest;
	}

	public void PlayFlashBack()
	{
		collectibleActivated = true;
		alreadyActivated = true;
		collectorRef.collectingMoveSpeedMultiplier = 0.5f;

		transform.GetChild(0).gameObject.SetActive(true);

		playerSFXManager.SetMusicToFlashBackMode(true);
	}

	void Update()
	{
		float distance = Vector3.Distance(transform.position, player.position);

		if (distance > maxDistanceToExit && collectibleActivated)
		{
			collectorRef.playerVFXManager.CollectibleVolume_OFF();
			collectorRef.collecting = false;

			DisableCollectible();
		}
	}

	public void DisableCollectible()
	{
		if (activatedTimes < 1)
		{
			closestAirColumn.GetComponent<AirColumnManager>().collectibleActivated++;

			playerSFXManager.SetMusicProgressionGoal(closestAirColumn.GetComponent<AirColumnManager>().collectibleActivated);

			//SFX VFX
			collectibleVFXManager.CollectibleValidationVFX();
			closestAirColumn.GetComponent<AirColumnSFX>().AirColumnUpdate();
		}
		activatedTimes++;
		StartCoroutine(Disappear());
		collectibleActivated = false;
		collectorRef.collectingMoveSpeedMultiplier = 1;
	}

	IEnumerator Disappear()
	{
		yield return new WaitForSeconds(collectorRef.playerVFXManager.ghostFadeDuration);
		transform.GetChild(0).gameObject.SetActive(false);

		playerSFXManager.SetMusicToFlashBackMode(false);
	}

	/*
		Faire version Controller
	*/
}
