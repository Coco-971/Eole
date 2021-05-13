using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabInstantiateTool : MonoBehaviour
{
	public int minRot;
	public int maxRot;
	public float minScale;
	public float maxScale;
	public GameObject[] prefabs;
	int randomPrefab;
	GameObject instantiatedPrefab;

	[ExecuteInEditMode]
	public void InstantiatePrefab()
	{
		randomPrefab = Random.Range(0, prefabs.Length);
		instantiatedPrefab = PrefabUtility.InstantiatePrefab(prefabs[randomPrefab]) as GameObject;
		instantiatedPrefab.transform.position = transform.position;
		instantiatedPrefab.transform.parent = transform;
		int randomizedRot = Random.Range(0, 360);
		instantiatedPrefab.transform.rotation = Quaternion.Euler (0, randomizedRot, 0);
		float randomScale = Random.Range(minScale, maxScale);
		Vector3 randomizedScale = Vector3.one * randomScale;
		instantiatedPrefab.transform.localScale = randomizedScale;
	}

	public void Destroy()
	{
		if (instantiatedPrefab != null)
		{
			DestroyImmediate(instantiatedPrefab);
		}
	}
}