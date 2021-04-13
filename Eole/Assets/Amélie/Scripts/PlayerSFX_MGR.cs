using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX_MGR : MonoBehaviour
{
    [Header("Player Footsteps SFX")]
    public AudioSource footStepSource;

    public Vector2 footStepPitchValuesMinMax = new Vector2(.7f,1.3f ) ;
    public Vector2 footStepVolumeValuesMinMax = new Vector2(.4f, .6f);
    public List<AudioClip> footStepConcrete_SFX;
    public List<AudioClip> footStepLeaves_SFX;
    private int footStepCount = 0;


    [Header("Player Breeze SFX")]
    public AudioSource breezeSource;

    public AnimationCurve breezeSFX_Curve;

    private bool breezeSFX_isActive = false;
    private float breezeSFX_timer = 0f;



    [Header("Player Glide VFX")]
    public AudioSource glideSource;

    public AnimationCurve glideSFX_VolumeCurve;
    public bool glideSFX_isActive = false;
    private float glideSFX_timer = 0f;


    [Header("Not Grounded")]
    public AudioSource playerFlagsSource;
    public AudioSource notGroundedSource;

    public AnimationCurve notGroundedSFX_Curve;
    public bool notGroundedSFX_isActive = false;
    private float notGroundedSFX_timer = 0f;



    // Not Grounded Methods

    public void notGroundedSFX_On()
    {
        if (!notGroundedSFX_isActive)
        {
            notGroundedSFX_isActive = true;
        }
    }

    public void notGroundedSFX_Off()
    {
        if (notGroundedSFX_isActive)
        {
            notGroundedSFX_isActive = false;
        }
    }


    // Glide Methods

    public void GlideSFX_On()
    {
        if (!glideSFX_isActive)
        {
            glideSFX_isActive = true;
        }
    }

    public void GlideSFX_Off()
    {
        if (glideSFX_isActive)
        {
            glideSFX_isActive = false;
        }
    }


    // Footsteps Method

    public void FootStepSFX_Launch(int indexSurface) // For alpha : Concrete = 0, Leaves = 1
    {
        if(indexSurface == 0)
        {
            footStepSource.clip = footStepConcrete_SFX[footStepCount];
        }
        else if(indexSurface == 1)
        {
            footStepSource.clip = footStepLeaves_SFX[footStepCount];
        }

        //footStepCount = (footStepCount + 1) % 2;

        footStepSource.pitch = Random.Range(footStepPitchValuesMinMax.x, footStepPitchValuesMinMax.y);

        footStepSource.volume = Random.Range(footStepVolumeValuesMinMax.x, footStepVolumeValuesMinMax.y);
        footStepSource.Play();
    }

    // Breeze Methods

    public void BreezeSFX_ON()
    {
        breezeSource.Play();

        breezeSFX_isActive = true;
    }

    public void BreezeSFX_OFF()
    {
        breezeSFX_isActive = false;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            BreezeSFX_ON();
        }

        if (Input.GetMouseButtonUp(0))
        {
            BreezeSFX_OFF();
        }

        // Not Grounded

        if (notGroundedSFX_isActive)
        {
            notGroundedSFX_timer = Mathf.Clamp01(notGroundedSFX_timer += Time.deltaTime * 2f);
        }
        else
        {
            notGroundedSFX_timer = Mathf.Clamp01(notGroundedSFX_timer -= Time.deltaTime * 5f);
        }

        notGroundedSource.volume = notGroundedSFX_Curve.Evaluate(notGroundedSFX_timer);
        playerFlagsSource.volume = notGroundedSFX_Curve.Evaluate(notGroundedSFX_timer);


        // Glide

        if (glideSFX_isActive)
        {
            glideSFX_timer = Mathf.Clamp01(glideSFX_timer += Time.deltaTime * 2.5f);
        }
        else
        {
            glideSFX_timer = Mathf.Clamp01(glideSFX_timer -= Time.deltaTime * 5f);
        }

        glideSource.volume = glideSFX_VolumeCurve.Evaluate(glideSFX_timer);

        //Breeze

        if (breezeSFX_isActive)
        {
            breezeSFX_timer = Mathf.Clamp01(breezeSFX_timer += Time.deltaTime);
        }
        else
        {
            breezeSFX_timer = Mathf.Clamp01(breezeSFX_timer -= Time.deltaTime);
        }

        breezeSource.volume = breezeSFX_Curve.Evaluate(breezeSFX_timer);

    }
}
