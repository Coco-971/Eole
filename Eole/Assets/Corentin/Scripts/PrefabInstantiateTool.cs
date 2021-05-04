using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabInstantiateTool : MonoBehaviour
{
	public GameObject[] prefabs;
	int randomPrefab;

	[ExecuteInEditMode]
	public void InstantiatePrefab()
	{
		randomPrefab = Random.Range(0, prefabs.Length);
		Object tempPrefab = PrefabUtility.InstantiatePrefab(prefabs[randomPrefab]);
		GameObject instantiatedPrefab = (UnityEngine.GameObject)tempPrefab;
		instantiatedPrefab.transform.position = transform.position;
		int randomizedRot = Random.Range(0, 360);
		instantiatedPrefab.transform.rotation = Quaternion.Euler (0, randomizedRot, 0);
	}
}
