using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using FMOD;
using FMODUnity;

public class CollectibleVFXManager : MonoBehaviour
{
    private PlayerSFXManager playerSFXManager;

    public VisualEffect collectibleVFX;
    public Transform airColumnTransform;

    string eventValidationNameVFX = "CollectibleValidation";
    string boolPropertyValidationName = "isAlreadyCollected";
    string windColumnPositionPropertyName = "WindColumnPosition";
    string masterAlphaPropertyName = "masterAlpha";

    [FMODUnity.EventRef]
    private string collectibleIdleONEvent = "event:/Collectibles/CollectibleIdleON";

    FMOD.Studio.EventInstance eCollectibleIdleInstance;
    ATTRIBUTES_3D Attributes;


    public void CollectibleVFX_ON()
    {
        collectibleVFX.SetFloat(masterAlphaPropertyName, 1);
    }

    public void CollectibleVFX_OFF()
    {
        collectibleVFX.SetFloat(masterAlphaPropertyName, 0);
    }

    public void CollectiblePickSFX() // appel quand le personnage ramasse/pose un collectible
    {
        playerSFXManager.PickCollectible();
    }

    public void CollectibleValidationVFX()
    {
        collectibleVFX.SendEvent(eventValidationNameVFX);
        collectibleVFX.SetBool(boolPropertyValidationName, true);

        eCollectibleIdleInstance.stop((FMOD.Studio.STOP_MODE)STOP_MODE.AllowFadeout);
        playerSFXManager.ActivateCollectible();
    }

	void Awake()
	{
		collectibleVFX = GetComponentInChildren<VisualEffect>();
        playerSFXManager = GameObject.Find("Player").GetComponent<PlayerSFXManager>();
		StartCoroutine(SecondFrame());

        eCollectibleIdleInstance = RuntimeManager.CreateInstance(collectibleIdleONEvent);
        Attributes = RuntimeUtils.To3DAttributes(transform.position);

        eCollectibleIdleInstance.set3DAttributes(Attributes);
        eCollectibleIdleInstance.start();
	}

	IEnumerator SecondFrame()
	{
		yield return new WaitForEndOfFrame();
		if (GetComponent<Collectible3D>())
		{
			airColumnTransform = GetComponent<Collectible3D>().closestAirColumn.transform;
		}
		else
		{
			airColumnTransform = GetComponent<CollectibleFlashBack>().closestAirColumn.transform;
		}	
		collectibleVFX.SetVector3(windColumnPositionPropertyName, airColumnTransform.position);
	}
}
