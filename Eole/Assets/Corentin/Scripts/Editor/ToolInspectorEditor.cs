using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabInstantiateTool))]
public class ToolInspectorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		PrefabInstantiateTool script = (PrefabInstantiateTool)target;
		if (GUILayout.Button("Instatiate"))
		{
			script.Destroy();
			script.InstantiatePrefab();
		}

		if (GUILayout.Button("Destroy"))
		{
			script.Destroy();
		}		
	}
}