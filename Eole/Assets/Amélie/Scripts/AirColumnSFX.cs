using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;

public class AirColumnSFX : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string airColumnUpdate;
    [FMODUnity.EventRef]
    public string airColumnActivation;
    [FMODUnity.EventRef]
    public string airColumnIdleON;

    FMOD.Studio.EventInstance eAirColumnIdleONInstance;
    ATTRIBUTES_3D Attributes;

    // Methods

    public void AirColumnUpdate()
    {
        RuntimeManager.PlayOneShot(airColumnUpdate, transform.position);
    }

    public void AirColumnActivation()
    {
        RuntimeManager.PlayOneShot(airColumnActivation, transform.position);

        eAirColumnIdleONInstance.start();
    }


    // Start is called before the first frame update
    void Start()
    {
        eAirColumnIdleONInstance = RuntimeManager.CreateInstance(airColumnIdleON);
        Attributes = RuntimeUtils.To3DAttributes(transform.position+ new Vector3(0,3,0));

        eAirColumnIdleONInstance.set3DAttributes(Attributes);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
