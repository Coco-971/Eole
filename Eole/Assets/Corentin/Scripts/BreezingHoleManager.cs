using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreezingHoleManager : MonoBehaviour
{
	public Transform StartPos;
	public Transform EndPos;

	void Start()
    {
		StartPos = gameObject.transform.Find("StartCollider");
		EndPos = gameObject.transform.Find("EndCollider");
	}
}
