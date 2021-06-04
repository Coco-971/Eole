using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
	[Header("References")]
	public Rigidbody rb;
	public CapsuleCollider body;
	public Abilities abilitiesRef;
	public Collector collectorRef;
	public LayerMask whatIsGround;
	public Animator animator;
	public PlayerVFXManager playerVFXManager;
	public UIManager UIManager;

	[Header("Booleans")]
	public bool canMove;
	public bool grounded;
	public bool breezing;
	private bool touchedGround;

	[Header("Values")]
	float currentMoveSpeed;
	public float baseMoveSpeed;
	public float inAirMoveSpeed;
	public float maxSpeed;

	Vector3 inputDirection = Vector3.zero;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		body = GetComponent<CapsuleCollider>();
		abilitiesRef = GetComponent<Abilities>();
		collectorRef = GetComponentInChildren<Collector>();
		animator = GetComponentInChildren<Animator>();
		playerVFXManager = GetComponent<PlayerVFXManager>();
		UIManager = GameObject.Find("UI").GetComponent<UIManager>();

		canMove = true;

		currentMoveSpeed = baseMoveSpeed;
	}

	void Update()
	{
		inputDirection.x = Input.GetAxis("Horizontal");
		inputDirection.z = Input.GetAxis("Vertical");

		grounded = Physics.CheckSphere(transform.position, body.radius - 0.1f, whatIsGround);

		if (grounded)
		{
			maxSpeed = 3;
		}

		if (grounded && canMove)
		{
			if(touchedGround)
			{
				playerVFXManager.TouchGroundVFX_Play();
				touchedGround = true;
			}

			if (inputDirection.x != 0 || inputDirection.z != 0)
			{
				animator.SetBool("isMoving", true);
			}
			else
			{
				animator.SetBool("isMoving", false);
			}
		}
		else
		{
			animator.SetBool("isMoving", false);
			touchedGround = false;
		}

		breezing = abilitiesRef.breezing;
	}

	void FixedUpdate()
	{
		if (grounded)
		{
			if (canMove)
			{
				currentMoveSpeed = baseMoveSpeed;
				Vector3 moveDirection = (transform.right * inputDirection.x + transform.forward * inputDirection.z) * (currentMoveSpeed * collectorRef.collectingMoveSpeedMultiplier) * Time.deltaTime * 10;
				moveDirection.y = rb.velocity.y;
				rb.velocity = moveDirection;
			}
		}
		else
		{
			if (canMove)
			{
				currentMoveSpeed = inAirMoveSpeed;
				rb.AddForce((transform.right * inputDirection.x + transform.forward * inputDirection.z) * (currentMoveSpeed * collectorRef.collectingMoveSpeedMultiplier) * Time.deltaTime * 10, ForceMode.Acceleration);
				Vector3 velocityC = rb.velocity;
				velocityC.y = 0;
				velocityC = Vector3.ClampMagnitude(velocityC, maxSpeed);
				velocityC.y = rb.velocity.y;
				rb.velocity = velocityC;
			}
		}
	}

	/*
		Faire version Controller
	*/
}