using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collector : MonoBehaviour
{
	[Header("References")]
	public Rigidbody rb;
	public GameObject closestCollectible;
	public Camera cameraRef;
	public Mover moverRef;
	public Abilities abilitiesRef;
	public CameraManager cameraManagerRef;
	public PlayerVFXManager playerVFXManager;
	public LayerMask whatIsObstacles;

	[Header("Booleans")]
	bool visible;
	public bool collectable;
	public bool collecting;
	public bool readyToSwitch;

	[Header("Values")]
	public float distanceToCollect;
	float initialCameraRot;
	public float collectingMoveSpeedMultiplier;

	[Header("Pick Up Curve")]
	public AnimationCurve lerpCurve;
	public float duration;

	void Awake()
	{
		rb = GetComponentInParent<Rigidbody>();
		cameraRef = GetComponent<Camera>();
		moverRef = GetComponentInParent<Mover>();
		abilitiesRef = GetComponentInParent<Abilities>();
		cameraManagerRef = GetComponent<CameraManager>();
		playerVFXManager = GameObject.Find("Player").GetComponent<PlayerVFXManager>();

		collecting = false;
		readyToSwitch = true;

		collectingMoveSpeedMultiplier = 1;
	}

	public GameObject FindClosestCollectible()
	{
		GameObject[] collectibles;
		collectibles = GameObject.FindGameObjectsWithTag("Collectibles");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject obj in collectibles)
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

	void Update()
	{
		closestCollectible = FindClosestCollectible();
		Collectible3D collectible3D = closestCollectible.GetComponent<Collectible3D>();
		CollectibleFlashBack collectibleFlashBack = closestCollectible.GetComponent<CollectibleFlashBack>();

		Vector3 directionToClosestCollectible = (closestCollectible.transform.position - transform.position).normalized;
		float distanceToPlayer = Vector3.Distance(transform.position, closestCollectible.transform.position);
		RaycastHit hit;
		visible = Physics.Raycast(transform.position, directionToClosestCollectible, out hit, distanceToPlayer, whatIsObstacles);

		//LetGo
		if (Input.GetKeyDown(KeyCode.E) && collecting && readyToSwitch)
		{
			collecting = false;

			//VFX
			playerVFXManager.CollectibleVolume_OFF();

			if (collectible3D)
			{
				cameraManagerRef.xRotation = initialCameraRot;
				StartCoroutine(CollectorCooldown(duration));
				collectible3D.DisableCollectible();
				moverRef.canMove = true;
				cameraManagerRef.canLook = true;
				abilitiesRef.canAbility = true;
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			else
			{
				collectibleFlashBack.DisableCollectible();
				StartCoroutine(CollectorCooldown(playerVFXManager.ghostFadeDuration));
			}
		}

		//PickUp
		else if (Input.GetKeyDown(KeyCode.E) && collectable && !collecting && readyToSwitch)
		{
			collecting = true;
			abilitiesRef.GlideEnd();
			abilitiesRef.BreezeEnd();
			if (collectible3D)
			{
				initialCameraRot = cameraManagerRef.xRotation;
				StartCoroutine(CollectorCooldown(duration));
				StartCoroutine(LerpTorwards(collectible3D.targetPos));
				rb.velocity = Vector3.zero;
				moverRef.canMove = false;
				cameraManagerRef.canLook = false;
				abilitiesRef.canAbility = false;
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				collectible3D.InspectObject();

				//VFX
				playerVFXManager.CollectibleVolume_ON(0);
			}
			else
			{
				StartCoroutine(CollectorCooldown(playerVFXManager.ghostFadeDuration));
				closestCollectible.GetComponent<CollectibleFlashBack>().PlayFlashBack();

				//VFX
				playerVFXManager.CollectibleVolume_ON(1);
			}
		}

		if (distanceToPlayer < distanceToCollect && !visible)
		{
			collectable = true;
			closestCollectible.GetComponentInChildren<SpriteRenderer>().enabled = true;
			if (collecting)
			{
				closestCollectible.GetComponentInChildren<SpriteRenderer>().enabled = false;
			}
		}
		else
		{
			collectable = false;
			closestCollectible.GetComponentInChildren<SpriteRenderer>().enabled = false;
		}
	}

	IEnumerator LerpTorwards(Vector3 target)
	{
		float progress = 0;
		float t = 0;
		Vector3 direction = target - transform.position;
		Quaternion rotation = Quaternion.LookRotation(direction);
		while (transform.rotation != rotation && t < duration)
		{
			progress = t / duration;
			float curveValue = lerpCurve.Evaluate(progress);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, curveValue);
			t += Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator CollectorCooldown(float duration)
	{
		if (readyToSwitch)
		{
			readyToSwitch = false;
		}
		else
		{
			readyToSwitch = true;
		}
		yield return new WaitForSeconds(duration);
		if (readyToSwitch)
		{
			readyToSwitch = false;
		}
		else
		{
			readyToSwitch = true;
		}
	}

	/*
		Faire version Controller
	*/
}
