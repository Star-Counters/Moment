using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
public class ParticleEditorWindow : EditorWindow{	
	float startDelay=0f;
	bool includeChilds=true;
	[MenuItem ("CommonTool/Open/ParticleEditorWindow")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		ParticleEditorWindow window = (ParticleEditorWindow)EditorWindow.GetWindow (typeof (ParticleEditorWindow));
	}
	void OnGUI () {
		includeChilds = EditorGUILayout.ToggleLeft ("IncludeChilds",includeChilds);
		startDelay = EditorGUILayout.FloatField ("startDelay",startDelay);
		if (GUILayout.Button ("Apply")) {
			if(includeChilds){
				foreach(Transform transform in Selection.transforms){
					ParticleSystem[] particleSystems=transform.GetComponentsInChildren<ParticleSystem>();
					foreach(ParticleSystem particleSystem in particleSystems){
						particleSystem.startDelay=startDelay;
					}
				}
			}
			else{
				foreach(Transform transform in Selection.transforms){
					ParticleSystem particleSystem = transform.GetComponent<ParticleSystem>();
					if(particleSystem){
						particleSystem.startDelay=startDelay;
					}
				}
			}
		}
	
	}
}
