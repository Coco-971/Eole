using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;

public class PlayerSFXManager : MonoBehaviour
{

    [Header("Player")]

    [FMODUnity.EventRef]
    public string dyingEvent;
    [FMODUnity.EventRef]
    public string respawnEvent;
    [FMODUnity.EventRef]
    public string footstepEvent;
    [FMODUnity.EventRef]
    public string touchGroundEvent;
    [FMODUnity.EventRef]
    public string glideEvent;
    [FMODUnity.EventRef]
    public string fallingEvent;

    FMOD.Studio.EventInstance ePlayerGlideInstance;
    FMOD.Studio.EventInstance ePlayerFallingInstance;

    [Header("Breeze")]

    [FMODUnity.EventRef]
    public string breezeEvent;
    [FMODUnity.EventRef]
    public string breezeActivationEvent;

    [FMODUnity.ParamRef]
    public string breezeParameter; // not an event

    [FMODUnity.EventRef]
    public string takePassage;
    [FMODUnity.EventRef]
    public string takeBooster;

    private bool breezeSFX_isActive = false;
    private float breezeSFX_timer = 0;

    FMOD.Studio.EventInstance ePlayerBreezeInstance;
    FMOD.Studio.PARAMETER_ID breezeParameterId;

    [Header("Collectible")]

    [FMODUnity.EventRef]
    public string pickCollectibleEvent;

    [FMODUnity.EventRef]
    public string activateCollectibleEvent;

    [Header("Ghost")]

    [FMODUnity.EventRef]
    public string ghostEvent;

    FMOD.Studio.EventInstance eGhostONInstance;



    void BreezeParameter()
    {
        ePlayerBreezeInstance = RuntimeManager.CreateInstance(breezeEvent);

        FMOD.Studio.PARAMETER_DESCRIPTION breezeParam;

        RuntimeManager.StudioSystem.getParameterDescriptionByName(breezeParameter, out breezeParam);
        breezeParameterId = breezeParam.id;

        ePlayerBreezeInstance.start();
    }

    public void BreezeSFX_ON()
    {
        breezeSFX_isActive = true;

        RuntimeManager.PlayOneShot(breezeActivationEvent);
    }

    public void BreezeSFX_OFF()
    {
        breezeSFX_isActive = false;
    }
    
    public void BreezeSFX_Update()
    {
        if (breezeSFX_isActive)
        {
            breezeSFX_timer = Mathf.Clamp01(breezeSFX_timer += Time.deltaTime / .5f);
        }
        else
        {
            breezeSFX_timer = Mathf.Clamp01(breezeSFX_timer -= Time.deltaTime / 2f);
        }

        RuntimeManager.StudioSystem.setParameterByID(breezeParameterId, breezeSFX_timer);
    }

    public void TakePassage()
    {
        RuntimeManager.PlayOneShot(takePassage);
    }

    public void TakeBooster()
    {
        RuntimeManager.PlayOneShot(takeBooster);
    }


    // Player Collectible Methods

    public void PickCollectible()
    {
        RuntimeManager.PlayOneShot(pickCollectibleEvent);
    }

    public void ActivateCollectible()
    {
        RuntimeManager.PlayOneShot(activateCollectibleEvent);
    }

    public void GhostON()
    {
        eGhostONInstance.start();
    }

    public void GhostOFF()
    {
        eGhostONInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    // Player Methods

    public void Footstep()
    {
        RuntimeManager.PlayOneShot(footstepEvent);
    }

    public void TouchGround()
    {
        RuntimeManager.PlayOneShot(touchGroundEvent);
    }

    public void DyingSFX()
    {
        RuntimeManager.PlayOneShot(dyingEvent);
    }

    public void RespawnSFX()
    {
        RuntimeManager.PlayOneShot(respawnEvent);
    }


    // Glide Methods
    public void Glide_ON()
    {
        ePlayerGlideInstance.start();
    }

    public void Glide_OFF()
    {
        ePlayerGlideInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    // Falling Methods
    public void Falling_ON()
    {
        ePlayerFallingInstance.start();
    }

    public void Falling_OFF()
    {
        ePlayerFallingInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }



    // Start is called before the first frame update
    void Start()
    {
        ePlayerGlideInstance = RuntimeManager.CreateInstance(glideEvent);
        ePlayerFallingInstance = RuntimeManager.CreateInstance(fallingEvent);

        BreezeParameter();
        eGhostONInstance = RuntimeManager.CreateInstance(ghostEvent);
    }

    // Update is called once per frame
    void Update()
    {
        BreezeSFX_Update();


        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            UI_SFXMethods.CancelUI_SFX();
        }
    }
}
