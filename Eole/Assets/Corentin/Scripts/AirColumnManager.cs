using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AirColumnManager : MonoBehaviour
{
    [Header("References")]
    public GameObject airColumnVFX;

	//SFX
	public AirColumnSFX airColumnSFX;

	[Header("Booleans")]
	public bool isOn;
	public bool activating;
	public bool changing;

	[Header("Values")]
	public int minToActivate;
	public int collectibleActivated;
	public float airColumnStrenght;
	public float activationDelay;

	void Awake()
	{
		airColumnVFX = GetComponentInChildren<VisualEffect>().gameObject;
		isOn = false;
		activating = false;
		airColumnVFX.SetActive(false);

		//SFX
		airColumnSFX = GetComponent<AirColumnSFX>();
	}

	void OnTriggerStay(Collider other)
	{
		if (isOn)
		{
			Rigidbody objRb = other.GetComponent<Rigidbody>();
			objRb.AddForce(transform.up * airColumnStrenght * Time.deltaTime, ForceMode.Impulse);
		}
	}

    //VFX
    void Update()
    {
        if (collectibleActivated >= minToActivate && !activating)
        {
			activating = true;
			StartCoroutine(Activation());
        }
    }

	IEnumerator Activation()
	{
		yield return new WaitForSeconds(activationDelay);
		airColumnVFX.SetActive(true);
		isOn = true;
		airColumnSFX.AirColumnActivation();
	}
}
