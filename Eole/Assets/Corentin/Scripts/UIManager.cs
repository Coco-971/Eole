using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
	[Header("References")]
	public GameObject InGameUI;
	public GameObject PauseUI;
	public GameObject collectibleTextGameObject;
	public GameObject text;
	public TextMeshProUGUI collectibleText;
	GameObject player;
	Mover moverRef;
	Abilities abilitiesRef;
	GameObject mainCamera;
	CameraManager cameraManagerRef;
	Collector collectorRef;
	public Image jaugeFill;
	public Volume pauseVolume;

	[Header("Values")]
	float initialCameraRot;

	[Header("Booleans")]
	public bool paused;

	void Awake()
	{
		InGameUI = GameObject.Find("InGameUI");
		PauseUI = GameObject.Find("PauseUI");
		collectibleTextGameObject = GameObject.Find("CollectibleText");
		collectibleText = collectibleTextGameObject.GetComponent<TextMeshProUGUI>();
		player = GameObject.Find("Player");
		moverRef = player.GetComponent<Mover>();
		abilitiesRef = player.GetComponent<Abilities>();
		mainCamera = GameObject.Find("Camera");
		cameraManagerRef = mainCamera.GetComponent<CameraManager>();
		collectorRef = mainCamera.GetComponent<Collector>();

		InGameUI.SetActive(true);
		PauseUI.SetActive(false);

		text.SetActive(false);
		collectibleTextGameObject.SetActive(false);
	}

	void Update()
    {
		jaugeFill.fillAmount = abilitiesRef.breezeEnergyInSeconds / abilitiesRef.breezeDuration;

		if (collectorRef.collecting)
		{
			collectibleTextGameObject.SetActive(true);
			if (collectorRef.closestCollectible.GetComponent<Collectible3D>())
			{
				text.SetActive(true);
			}
				
			collectibleText.SetText(collectorRef.closestCollectibleText);
		}
		else
		{
			text.SetActive(false);
			collectibleTextGameObject.SetActive(false);
		}

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
		pauseVolume.weight = 1;

		if (moverRef.grounded)
		{
			moverRef.rb.velocity = Vector3.zero;
		}

		if (abilitiesRef.gliding || abilitiesRef.breezing)
		{
			abilitiesRef.GlideEnd();
			abilitiesRef.BreezeEnd();
		}

		//SFX
		UI_SFXMethods.CancelUI_SFX();
	}

	public void Resume()
	{
		paused = false;
		InGameUI.SetActive(true);
		PauseUI.SetActive(false);
		pauseVolume.weight = 0;

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
		//SFX
		UI_SFXMethods.ValidateUI_SFX();
	}

	public void Reset()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		//SFX
		UI_SFXMethods.ClickUI_SFX();
	}

	public void Save()
	{
		SaveSystem.Save(player);
	}

	public void LoadSave()
	{
		LevelData data = SaveSystem.LoadSave();

		Vector3 position;
		position.x = data.playerPos[0];
		position.y = data.playerPos[1];
		position.z = data.playerPos[2];
		player.transform.position = position;
	}

	public void Quit()
	{
		Application.Quit();
	}
}
