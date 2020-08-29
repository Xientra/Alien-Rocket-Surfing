using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bounds))]
public class BoundsEditor : Editor {
	public override void OnInspectorGUI() {
		Bounds bounds = (Bounds)target;

		/*
        if (DrawDefaultInspector()) {
            if (bounds.autoUpdate == true) {
                bounds.UpdateValues();
            }
        }
        */

		if (GUILayout.Button("Generate")) {
			bounds.UpdateValues();
		}

		DrawDefaultInspector();
	}
}
