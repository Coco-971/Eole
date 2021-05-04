using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class UI_SFXMethods : MonoBehaviour
{
    [FMODUnity.EventRef]
    public static string clickUI_SFX = "event:/UI/InteractWithButton";
    [FMODUnity.EventRef]
    public static string validateUI_SFX = "event:/UI/Validate";
    [FMODUnity.EventRef]
    public static string cancelUI_SFX = "event:/UI/Cancel";


    public static void ClickUI_SFX()
    {
        RuntimeManager.PlayOneShot(clickUI_SFX);
    }

    public static void ValidateUI_SFX()
    {
        RuntimeManager.PlayOneShot(validateUI_SFX);
    }

    public static void CancelUI_SFX()
    {
        RuntimeManager.PlayOneShot(cancelUI_SFX);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
