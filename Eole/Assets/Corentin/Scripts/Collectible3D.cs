using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible3D : MonoBehaviour
{
	[Header("References")]
	public Camera cameraRef;
	public Collector collectorRef;
	public GameObject closestAirColumn;
	public Transform player;
	public Texture2D baseCursor;
	public Texture2D handCursor;

    public CollectibleVFXManager collectibleVFXManager;

	[Header("Values")]
	[Range(0f, 2f)] public float distanceToCamera;

	[Header("Booleans")]
	public bool alreadyActivated;
	public bool collectibleActive;

	int activatedTimes;
	public Vector3 targetPos;
	Vector3 initialPos;
	Quaternion initialRot;

	void Awake()
	{
		cameraRef = GameObject.Find("Camera").GetComponent<Camera>();
		collectorRef = cameraRef.GetComponent<Collector>();
		player = FindObjectOfType<Mover>().transform;
		collectibleVFXManager = GetComponent<CollectibleVFXManager>();

		alreadyActivated = false;
		collectibleActive = false;

		closestAirColumn = FindClosestAirColumn();

		initialPos = transform.position;
		initialRot = transform.rotation;
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

	public void InspectObject()
	{
		collectibleActive = true;
		alreadyActivated = true;

		StartCoroutine(LerpTorwards(initialPos, targetPos));

		//VFX
		collectibleVFXManager.CollectibleVFX_OFF();
	}

	void Update()
	{
		targetPos = Vector3.Lerp(transform.position, cameraRef.transform.position, 0.5f) + cameraRef.transform.forward * distanceToCamera;

		if (collectibleActive)
		{
			if (Input.GetMouseButton(0))
			{
				Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto);
				Vector3 rotation = player.right * Input.GetAxis("Mouse Y") + player.forward * -Input.GetAxis("Mouse X");
				transform.Rotate(rotation * Time.deltaTime * 1000, Space.World);
			}
			else
			{
				Cursor.SetCursor(baseCursor, Vector2.zero, CursorMode.Auto);
			}
		}
	}

	public void DisableCollectible()
	{
		collectibleActive = false;

		StartCoroutine(LerpTorwards(transform.position, initialPos));
		transform.rotation = initialRot;

		if (activatedTimes < 1)
		{
			closestAirColumn.GetComponent<AirColumnManager>().collectibleActivated++;
		}
		activatedTimes++;

		//VFX
		collectibleVFXManager.CollectibleVFX_ON();
		if (activatedTimes <= 1)
		{
			collectibleVFXManager.CollectibleValidationVFX();
		}
	}

	IEnumerator LerpTorwards(Vector3 start, Vector3 target)
	{
		float progress = 0;
		float t = 0;
		while (transform.position != target)
		{
			progress = t / collectorRef.duration;
			float curveValue = collectorRef.lerpCurve.Evaluate(progress);
			transform.position = Vector3.Lerp(start, target, curveValue);
			t += Time.deltaTime;
			yield return null;
		}
	}

	/*
		Faire version Controller
	*/
}
