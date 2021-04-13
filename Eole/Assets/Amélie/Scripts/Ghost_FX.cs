using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_FX : MonoBehaviour
{
    public Transform playerTransform;

    public MeshRenderer ghostRenderer;

    public string ghostMaterialThreshloldPropertyName = "alphaClipThreshold";
    public float fadeDistance = 5;
    public float lerpMaxDistance = 10;
    public AnimationCurve ghostFadeCurve;

    private float distanceToPlayer;


    private void GhostFade()
    {
        distanceToPlayer = Mathf.Sqrt(Mathf.Pow(playerTransform.position.x - transform.position.x, 2)  + Mathf.Pow(playerTransform.position.z - transform.position.z, 2));       

        if(distanceToPlayer <= fadeDistance)
        {
            ghostRenderer.materials[0].SetFloat(ghostMaterialThreshloldPropertyName, 0);
        }

        if(fadeDistance < distanceToPlayer && distanceToPlayer < lerpMaxDistance)
        {
            ghostRenderer.materials[0].SetFloat(ghostMaterialThreshloldPropertyName, ghostFadeCurve.Evaluate((distanceToPlayer - fadeDistance) / lerpMaxDistance));
        }

        if(distanceToPlayer >= lerpMaxDistance)
        {
            ghostRenderer.materials[0].SetFloat(ghostMaterialThreshloldPropertyName, 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("no playerTransform referenced to GhostFX script");
        }
        if (ghostRenderer == null)
        {
            Debug.LogError("no playerTransform referenced to GhostFX script");
        }

        distanceToPlayer = Mathf.Sqrt(Mathf.Pow(playerTransform.position.x - transform.position.x, 2) + Mathf.Pow(playerTransform.position.z - transform.position.z, 2));
    }

    // Update is called once per frame
    void Update()
    {
        GhostFade();
    }
}
