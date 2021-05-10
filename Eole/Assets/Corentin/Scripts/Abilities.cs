using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
	[Header("References")]
	public Rigidbody rb;
	public CapsuleCollider body;
	public Transform cameraRot;
	public Mover moverRef;
	public Animator fader;
	public UIManager UIManager;

	//SFX VFX
    public PlayerVFXManager playerVFXManager;
    public PlayerSFXManager playerSFXmanager;

	[Header("Booleans")]
	public bool canAbility;
	public bool grounded;
	public bool wasAirborn;
	public bool gliding;
	public bool falling;
	public bool breezing;
	public bool breezed;
	public bool inAirColumn;
	public bool inTurbine;
	public bool canTakeHole;

	[Header("Jump")]
	public float jumpForce;

	[Header("Glide")]
	public float glideSmoother;
	public float glideDescentRate;

	[Header("Breeze")]
	public float breezeForce;
	float breezeForceModifier;
	public float breezeDuration;
	public float breezeDurationModifier;
	public float breezeEnergyInSeconds;
	public float breezeTurbineDuration;
	public float breezeTurbineForceAdder;
	public float breezeTurbineMultiplier;
	public float breezeTurbineSecondAdder;

	[Header("Turbulence")]
	public float turbulenceForce;
	public float turbulenceUp;

	[Header("Teleport")]
	public float teleportDelay;

	float timer = 0;
	int randomNumber = 1;
	bool randomizing = false;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		body = GetComponent<CapsuleCollider>();
		cameraRot = GameObject.Find("Camera").transform;
		moverRef = GetComponent<Mover>();
		fader = GameObject.Find("Fader").GetComponent<Animator>();
		playerVFXManager = GetComponent<PlayerVFXManager>();
		playerSFXmanager = GetComponent<PlayerSFXManager>();
		UIManager = GameObject.Find("UI").GetComponent<UIManager>();

		inAirColumn = false;
		canAbility = true;
		breezing = false;
		canTakeHole = true;
		wasAirborn = false;
		falling = false;

		breezeForceModifier = breezeForce;
		breezeEnergyInSeconds = breezeDuration;
		breezeDurationModifier = breezeDuration;
		breezeTurbineMultiplier = 1;
	}

	void Update()
	{
		grounded = moverRef.grounded;

		if (!randomizing)
		{
			StartCoroutine(Randomize());
		}

		if (canAbility)
		{
			//Jump
			if (Input.GetKeyDown(KeyCode.Space) && grounded)
			{
				Vector3 vertical = rb.velocity;
				vertical.y = 0;
				rb.velocity = vertical;
				rb.AddForce(transform.up * jumpForce * 10);
			}

			//Glide
			if (Input.GetKey(KeyCode.Space) && !grounded && !inAirColumn)
			{
				GlideStart();
			}

			if (Input.GetKeyUp(KeyCode.Space) || breezing || inAirColumn)
			{
				GlideEnd();
			}

			//Breeze Energy
			if (Input.GetMouseButtonDown(0) && breezeEnergyInSeconds > 0)
			{
				BreezeStart();
				breezed = true;
			}

			if (breezing)
			{
				if (breezeEnergyInSeconds > 0)
				{
					breezeEnergyInSeconds -= Time.deltaTime;
				}
				else
				{
					BreezeEnd();
				}
			}
			else
			{
				if (breezeEnergyInSeconds < breezeDuration && !breezed)
				{
					breezeEnergyInSeconds += Time.deltaTime;
				}
				else if (breezeEnergyInSeconds > breezeDuration)
				{
					breezeEnergyInSeconds -= Time.deltaTime;
				}
			}

			if (Input.GetMouseButtonUp(0) && breezing)
			{
				BreezeEnd();
			}

			if (grounded)
			{
				breezed = false;

				//SFX
				if (wasAirborn)
				{
					playerSFXmanager.TouchGround();
					wasAirborn = false;
					playerSFXmanager.Falling_OFF();
					falling = true;
				}
			}
			else
			{
				wasAirborn = true;

				if (rb.velocity.y < 0)
				{
					if (falling)
					{
						playerSFXmanager.Falling_ON();
						falling = false;
					}
				}
			}
		}
	}

	void FixedUpdate()
	{
		if (gliding)
		{
			timer += Time.deltaTime;
			if (timer < 1.5)
			{
				rb.AddForce(transform.up * timer * glideSmoother * Time.deltaTime * 10);
			}
			else
			{
				Vector3 vertical = rb.velocity;
				vertical.y = -glideDescentRate;
				rb.velocity = vertical;
			}
		}

		if (breezing)
		{
			rb.AddForce(cameraRot.forward * breezeForceModifier * breezeTurbineMultiplier * Time.fixedDeltaTime, ForceMode.VelocityChange);
		}
	}

	void GlideStart()
	{
		gliding = true;

		playerVFXManager.GlideVFXOpacity_On();
	}

	public void GlideEnd()
	{
		gliding = false;
		timer = 0;

		playerVFXManager.GlideVFXOpacity_Off();
	}

	void BreezeStart()
	{
		moverRef.maxSpeed = 10;
		moverRef.canMove = false;
		breezing = true;
		rb.useGravity = false;

		playerVFXManager.BreezeVFXIntensity_On();
	}

	public void BreezeEnd()
	{
		moverRef.canMove = true;
		breezing = false;
		rb.useGravity = true;

		playerVFXManager.BreezeVFXIntensity_Off();
	}

	IEnumerator Randomize()
	{
		randomizing = true;
		randomNumber = Random.Range(1, 10);
		yield return new WaitForSeconds(3);
		randomizing = false;
	}

	void OnTriggerStay(Collider other)
	{
		if (other.GetComponent<Rigidbody>() == true && breezing)
		{
			Rigidbody objRb = other.GetComponent<Rigidbody>();
			objRb.AddExplosionForce(turbulenceForce, transform.position * randomNumber, 0);
			objRb.AddForce(transform.up * turbulenceUp);
		}

		if (other.tag == "Hole" && canTakeHole && breezing)
		{
			canTakeHole = false;

			fader.SetBool("InstantFade", true);

			if (other.name == "StartCollider")
			{
				StartCoroutine(Teleport(other.GetComponentInParent<BreezingHoleManager>().EndPos));
			}
			else if (other.name == "EndCollider")
			{
				StartCoroutine(Teleport(other.GetComponentInParent<BreezingHoleManager>().StartPos));
			}
		}
	}

	//Collider Manager
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "AirColumn")
		{
			inAirColumn = true;
		}

		if (other.tag == "Turbine" && breezing)
		{
			breezeDurationModifier += breezeTurbineSecondAdder;
			breezeEnergyInSeconds = breezeDurationModifier;
			StartCoroutine(Turbining());

			//SFX
			playerSFXmanager.TakeBooster();
		}

		if (other.tag == "Hole" && canTakeHole && breezing)
		{
			playerSFXmanager.TakePassage();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "AirColumn")
		{
			inAirColumn = false;
		}

		if (other.tag == "Hole")
		{
			StartCoroutine(BreezeHoleCooldown());
		}
	}

	IEnumerator Turbining()
	{
		inTurbine = true;
		float timer = breezeTurbineDuration;
		breezeTurbineMultiplier += breezeTurbineForceAdder;
		while (timer > 0)
		{
			timer -= Time.deltaTime;
			breezeTurbineMultiplier = Mathf.Lerp(breezeTurbineMultiplier, 1, timer * Time.deltaTime);
			yield return null;
		}
		inTurbine = false;
		breezeDurationModifier = breezeDuration;
	}

	IEnumerator Teleport(Transform target)
	{
		yield return new WaitForSeconds(teleportDelay);
		transform.position = target.position;
		yield return new WaitForSeconds(0.1f);
		fader.SetBool("InstantFade", false);
	}

	IEnumerator BreezeHoleCooldown()
	{
		yield return new WaitForSeconds(2);
		canTakeHole = true;
	}

	/*
		Faire version Controller
	*/
}