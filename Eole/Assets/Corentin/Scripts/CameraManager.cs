using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	[Header("References")]
	public Transform playerRot;

	[Header("Booleans")]
	public bool canLook;

	[Header("Values")]
	public float MouseSensitivity = 100f;
	public float mouseX;
	public float mouseY;

	public float xRotation = 0f;

	void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		canLook = true;
	}

	void Update()
	{
		mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
		mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		if (canLook)
		{
			transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
			playerRot.Rotate(Vector3.up * mouseX);
		}
	}
}
