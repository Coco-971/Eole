using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class Ghost_FX : MonoBehaviour
{
    public Transform playerTransform;

    public MeshRenderer ghostRenderer;

    public string ghostMaterialThreshloldPropertyName = "alphaClipThreshold";
    public float fadeDistance = 5;
    public float lerpMaxDistance = 10;
    public AnimationCurve ghostFadeCurve;

    private float distanceToPlayer;

    [Header("SFX")]
    [FMODUnity.EventRef]
    public string ghostEvent;

    private bool canPlaySFX = false;

    FMOD.Studio.EventInstance eGhostONInstance;
    ATTRIBUTES_3D Attributes;

    private void GhostSFX_ON()
    {
        if (canPlaySFX)
        {
            canPlaySFX = false;
        }
    }

    private void GhostSFX_OFF()
    {
        if (canPlaySFX)
        {
            canPlaySFX = false;
        }
    }

    private void GhostFade()
    {
        distanceToPlayer = Mathf.Sqrt(Mathf.Pow(playerTransform.position.x - transform.position.x, 2)  + Mathf.Pow(playerTransform.position.z - transform.position.z, 2));       

        if(distanceToPlayer <= fadeDistance)
        {
            ghostRenderer.materials[0].SetFloat(ghostMaterialThreshloldPropertyName, 0);
            GhostSFX_OFF();
        }

        if(fadeDistance < distanceToPlayer && distanceToPlayer < lerpMaxDistance)
        {
            ghostRenderer.materials[0].SetFloat(ghostMaterialThreshloldPropertyName, ghostFadeCurve.Evaluate((distanceToPlayer - fadeDistance) / lerpMaxDistance));
            canPlaySFX = true;
        }

        if(distanceToPlayer >= lerpMaxDistance)
        {
            ghostRenderer.materials[0].SetFloat(ghostMaterialThreshloldPropertyName, 1);
            GhostSFX_ON();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playerTransform == null)
        {
            print("no playerTransform referenced to GhostFX script");
        }
        if (ghostRenderer == null)
        {
            print("no playerTransform referenced to GhostFX script");
        }

        distanceToPlayer = Mathf.Sqrt(Mathf.Pow(playerTransform.position.x - transform.position.x, 2) + Mathf.Pow(playerTransform.position.z - transform.position.z, 2));

        eGhostONInstance = RuntimeManager.CreateInstance(ghostEvent);
        Attributes = RuntimeUtils.To3DAttributes(transform.position + new Vector3(0, 1, 0));

        eGhostONInstance.set3DAttributes(Attributes);
    }

    // Update is called once per frame
    void Update()
    {
        GhostFade();
    }
}
