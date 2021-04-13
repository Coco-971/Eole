using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirColumnManager : MonoBehaviour
{
    [Header("References")]
    public GameObject airColumnVFX;

	[Header("Booleans")]
	public bool isOn;

	[Header("Values")]
	public int minToActivate;
	public int collectibleActivated;
	public float airColumnStrenght;
	public float activationDelay;

	void Awake()
	{
		airColumnVFX = GameObject.Find("AirColumnVFX");
		isOn = false;
		airColumnVFX.SetActive(false);
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
    private void Update()
    {
        if (collectibleActivated >= minToActivate)
        {
			StartCoroutine(Activation());
        }
    }

	IEnumerator Activation()
	{
		yield return new WaitForSeconds(activationDelay);
		airColumnVFX.SetActive(true);
		isOn = true;
	}
}
