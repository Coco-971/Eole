using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CollectibleVFXManager : MonoBehaviour
{
    public VisualEffect collectibleVFX;
    public Transform airColumnTransform;

    string eventValidationNameVFX = "CollectibleValidation";
    string boolPropertyValidationName = "isAlreadyCollected";
    string windColumnPositionPropertyName = "WindColumnPosition";
    string masterAlphaPropertyName = "masterAlpha";


    public void CollectibleVFX_ON()
    {
        collectibleVFX.SetFloat(masterAlphaPropertyName, 1);
    }

    public void CollectibleVFX_OFF()
    {
        collectibleVFX.SetFloat(masterAlphaPropertyName, 0);
    }

    public void CollectibleValidationVFX()
    {
        collectibleVFX.SendEvent(eventValidationNameVFX);
        collectibleVFX.SetBool(boolPropertyValidationName, true);
    }

	void Awake()
	{
		collectibleVFX = GetComponentInChildren<VisualEffect>();
		StartCoroutine(SecondFrame());
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
