using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLookAt : MonoBehaviour
{
	[Header("References")]
	public Transform playerCamera;
	public Transform parent;
	public Transform player;

	void Awake()
	{
		playerCamera = FindObjectOfType<Camera>().transform;
		parent = transform.parent;
		player = GameObject.Find("Player").transform;
	}

	void Update()
	{ 
		transform.LookAt(playerCamera);
		parent.LookAt(player);
	}
}
