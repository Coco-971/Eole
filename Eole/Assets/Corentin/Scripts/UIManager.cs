using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	[Header("References")]
	public GameObject InGameUI;
	public GameObject PauseUI;
	public CameraManager cameraManagerRef;
	public Mover moverRef;
	public Abilities abilitiesRef;
	public Collector collectorRef;

	[Header("Values")]
	float initialCameraRot;

	[Header("Booleans")]
	public bool paused;

	void Awake()
	{
		InGameUI = GameObject.Find("InGameUI");
		PauseUI = GameObject.Find("PauseUI");
		cameraManagerRef = GameObject.Find("Camera").GetComponent<CameraManager>();
		moverRef = GameObject.Find("Player").GetComponent<Mover>();
		abilitiesRef = GameObject.Find("Player").GetComponent<Abilities>();
		collectorRef = GameObject.Find("Camera").GetComponent<Collector>();

		InGameUI.SetActive(true);
		PauseUI.SetActive(false);
	}

	void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (!paused)
			{
				Pause();
			}
			else
			{
				Resume();
			}
		}
	}

	void Pause()
	{
		paused = true;
		InGameUI.SetActive(false);
		PauseUI.SetActive(true);
		cameraManagerRef.canLook = false;
		initialCameraRot = cameraManagerRef.xRotation;
		moverRef.canMove = false;
		abilitiesRef.canAbility = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		if (moverRef.grounded)
		{
			moverRef.rb.velocity = Vector3.zero;
		}

		if (abilitiesRef.gliding || abilitiesRef.breezing)
		{
			abilitiesRef.GlideEnd();
			abilitiesRef.BreezeEnd();
		}
	}

	public void Resume()
	{
		paused = false;
		InGameUI.SetActive(true);
		PauseUI.SetActive(false);

		if (collectorRef.collecting && collectorRef.closestCollectible.GetComponent<Collectible3D>())
		{
			cameraManagerRef.canLook = false;
			cameraManagerRef.xRotation = initialCameraRot;
			moverRef.canMove = false;
			abilitiesRef.canAbility = false;
		}
		else
		{
			cameraManagerRef.canLook = true;
			cameraManagerRef.xRotation = initialCameraRot;
			moverRef.canMove = true;
			abilitiesRef.canAbility = true;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	public void Reset()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void Save()
	{

	}

	public void Quit()
	{
		Application.Quit();
	}
}
