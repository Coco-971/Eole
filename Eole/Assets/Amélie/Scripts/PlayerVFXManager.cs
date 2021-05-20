using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Rendering;
using FMODUnity;

public class PlayerVFXManager : MonoBehaviour
{
    [Header("References")]
    public GameObject playerVFX;
    public PlayerSFXManager playerSFXManager;
    public Camera mainCamera;
    public VisualEffect cameraVFXGraph;


    [Header("Glide VFX")]
    public AnimationCurve glideVFX_alphaCurve;
    public string glideVFX_AlphaPropertyName = "glide_MasterAlpha";

    private bool glideVFX_isActive = false;
    private float glideVFX_timer = 0f;


    [Header("Breeze VFX")]
    public AnimationCurve breezeVFX_intensityVolumeCurve;
    public AnimationCurve breezeVFX_intensityFOVCurve;
    public string breezeVFX_AlphaPropertyName = "breeze_MasterAlpha";

    public Vector2 breezeVFX_minMaxFOV = new Vector2(60, 80);

    private bool breezeVFX_isActive = false;
    private float breezeVFX_timer = 0f;

    [Header("Trail Particles")]
    public string trailParticlesVFX_RatePropertyName = "trailParticles_Rate";
    public Vector2 trailParticlesVFX_rateMinMax = new Vector2(0, 3);

    [Header("PostProcess")]
    public Volume collectible3D_Volume;
    public Volume collectibleFlashback_Volume;

    private bool isCollectible3DPickedUp;
    private bool isCollectibleFlashbackPickedUp;
    private float collectible3DVolumeTimer;
    private float collectibleFlashbackVolumeTimer;

    [Header("Ghosts")]
    public Material ghostMaterial;

    public AnimationCurve ghostFadeCurve;
    public float ghostFadeDuration = 2;
    private float ghostMaterial_timer;
    private bool canSeeGhosts = false;


    void Awake()
	{
        playerVFX = GameObject.Find("PlayerVFX");

		mainCamera = GameObject.Find("Camera").GetComponent<Camera>();

        cameraVFXGraph = GameObject.Find("CameraVFX").GetComponent<VisualEffect>();

        playerSFXManager = GetComponent<PlayerSFXManager>();

        collectible3D_Volume = GameObject.Find("Collectible3D_Volume").GetComponent<Volume>();
        collectibleFlashback_Volume = GameObject.Find("CollectibleFlashback_Volume").GetComponent<Volume>();

		playerVFX.GetComponent<Volume>().weight = 0;

        playerVFX.GetComponent<VisualEffect>().SetFloat(trailParticlesVFX_RatePropertyName, trailParticlesVFX_rateMinMax.x); // Getting the min value

        canSeeGhosts = false;
	}

    // Ghost Methods

    public void Ghosts_ON() // Quand on doit activer l'effet
    {
        ghostMaterial_timer = 0;
        canSeeGhosts = true;
        playerSFXManager.GhostON();
    }

    public void Ghosts_OFF() // Quand on doit désactiver l'effet
    {
        canSeeGhosts = false;
        playerSFXManager.GhostOFF();
    }

    private void Ghost_Fader() // Dans update
    {
        if (canSeeGhosts)
        {
            ghostMaterial_timer = Mathf.Clamp01(ghostMaterial_timer += Time.deltaTime / ghostFadeDuration);

        }
        else
        {
            ghostMaterial_timer = Mathf.Clamp01(ghostMaterial_timer -= Time.deltaTime / ghostFadeDuration);
        }

        ghostMaterial.SetFloat("_cutoffHeight", ghostFadeCurve.Evaluate(ghostMaterial_timer)); // The value of this curve must stay between -0.5 and 2.75
    }

	// Glide Methods
	public void GlideVFXOpacity_On()
    {
        if (!glideVFX_isActive)
        {
            glideVFX_isActive = true;

            TrailParticlesVFX_On();

            playerSFXManager.Glide_ON();
        }
    }

    public void GlideVFXOpacity_Off()
    {
        if (glideVFX_isActive)
        {
            glideVFX_isActive = false;

            TrailParticlesVFX_Off();

            playerSFXManager.Glide_OFF();
        }
    }

    // Breeze Methods
    public void BreezeVFXIntensity_On()
    {
        if (!breezeVFX_isActive)
        {
            breezeVFX_isActive = true;

            mainCamera.fieldOfView = breezeVFX_minMaxFOV.y; // Getting the max value

            TrailParticlesVFX_On();
            playerSFXManager.BreezeSFX_ON();
        }
    }

    public void BreezeVFXIntensity_Off()
    {
        if (breezeVFX_isActive)
        {
            breezeVFX_isActive = false;

            mainCamera.fieldOfView = breezeVFX_minMaxFOV.x; // Getting the min Value

            TrailParticlesVFX_Off();
            playerSFXManager.BreezeSFX_OFF();
        }
    }

    // Trail Particles Methods
    public void TrailParticlesVFX_On()
    {
        playerVFX.GetComponent<VisualEffect>().SetFloat(trailParticlesVFX_RatePropertyName, trailParticlesVFX_rateMinMax.y); // Getting the max value
    }

    public void TrailParticlesVFX_Off()
    {
        playerVFX.GetComponent<VisualEffect>().SetFloat(trailParticlesVFX_RatePropertyName, trailParticlesVFX_rateMinMax.x); // Getting the min value
    }


    // Touch Ground Effect
    public void TouchGroundVFX_Play()
    {
        playerVFX.GetComponent<VisualEffect>().SendEvent("TouchGround"); // Touch Ground Event
    }

    // Volume Collectible

    public void CollectibleVolume_ON(int index) // index 0 = 3D, index 1 = Flashback
    {
        if(index == 0)
        {
            isCollectible3DPickedUp = true;
        }
        else
        {
            isCollectibleFlashbackPickedUp = true;
            Ghosts_ON();
        }
    }

    public void CollectibleVolume_OFF()
    {
        isCollectible3DPickedUp = false;
        isCollectibleFlashbackPickedUp = false;
        Ghosts_OFF();
    }


    void Update()
    {
        // Glide

        if (glideVFX_isActive)
        {
            glideVFX_timer = Mathf.Clamp01(glideVFX_timer += Time.deltaTime*2.5f);
        }
        else
        {
            glideVFX_timer = Mathf.Clamp01(glideVFX_timer -= Time.deltaTime*5f);
        }

        playerVFX.GetComponent<VisualEffect>().SetFloat(glideVFX_AlphaPropertyName, glideVFX_alphaCurve.Evaluate(glideVFX_timer));        // glide vfx opacity

        // Breeze



        // Collectible Volume

        if (isCollectible3DPickedUp)
        {
            collectible3DVolumeTimer = Mathf.Clamp01(collectible3DVolumeTimer += Time.deltaTime * 2f);
        }
        else
        {
            collectible3DVolumeTimer = Mathf.Clamp01(collectible3DVolumeTimer -= Time.deltaTime * 2f);
        }

        if (isCollectibleFlashbackPickedUp)
        {
            collectibleFlashbackVolumeTimer = Mathf.Clamp01(collectibleFlashbackVolumeTimer += Time.deltaTime * 2f);
        }
        else
        {
            collectibleFlashbackVolumeTimer = Mathf.Clamp01(collectibleFlashbackVolumeTimer -= Time.deltaTime * 2f);
        }

        collectible3D_Volume.weight = breezeVFX_intensityVolumeCurve.Evaluate(collectible3DVolumeTimer);

        collectibleFlashback_Volume.weight = breezeVFX_intensityVolumeCurve.Evaluate(collectibleFlashbackVolumeTimer);

        Ghost_Fader();

        playerVFX.GetComponent<Volume>().weight = breezeVFX_intensityVolumeCurve.Evaluate(breezeVFX_timer);

        cameraVFXGraph.SetFloat(breezeVFX_AlphaPropertyName, breezeVFX_intensityVolumeCurve.Evaluate(breezeVFX_timer)); // wind effect around camera opacity



    }

    private void FixedUpdate()
    {
        if (breezeVFX_isActive)
        {
            breezeVFX_timer = Mathf.Clamp01(breezeVFX_timer += Time.deltaTime * 2f);
        }
        else
        {
            breezeVFX_timer = Mathf.Clamp01(breezeVFX_timer -= Time.deltaTime * 3f);
        }

        mainCamera.fieldOfView = Mathf.Lerp(breezeVFX_minMaxFOV.x, breezeVFX_minMaxFOV.y, breezeVFX_intensityFOVCurve.Evaluate(breezeVFX_timer)); // interpolates beetween the min and max FOV values with the curve read by the timer
    }
}
