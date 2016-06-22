using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditorInternal;

public class GenAnimationCtl : Editor {

	[MenuItem("Tools/GenAnimationCtl")]
	static void GenCtl(){
		if (Selection.activeGameObject == null) {
			Debug.Log ("please select model prefab");
			return;
		}

		var pathstr = AssetDatabase.GetAssetPath (Selection.activeGameObject);
		var path = Path.GetDirectoryName (pathstr) + "/";

		Debug.Log (path);

		DirectoryInfo dinfo = new DirectoryInfo (path);
		List<AnimationClip> clips = new List<AnimationClip> ();
		foreach(var file in dinfo.GetFiles("*.anim")){
			Debug.Log (path + file.Name);
			clips.Add(AssetDatabase.LoadAssetAtPath<AnimationClip>(path + file.Name));
		}
		var ctl = createcontroller (clips, path, Selection.activeGameObject.name + "ctl");
		var go = Selection.activeGameObject;
		var animator = go.GetComponent<Animator> ();
		if (!animator) {
			animator = go.AddComponent<Animator> ();
		}
		animator.runtimeAnimatorController = ctl;
		//PrefabUtility.ReplacePrefab (go, go);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh ();
	}

	static UnityEditor.Animations.AnimatorController createcontroller(List<AnimationClip> clips, string path, string name){
		UnityEditor.Animations.AnimatorController animatorController = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(path  + name + ".controller");
		UnityEditor.Animations.AnimatorControllerLayer layer = animatorController.layers [0];
		AnimatorStateMachine sm = layer.stateMachine;

		foreach (var clip in clips) {
			var state = sm.AddState (clip.name);
			state.motion = clip;
			if (clip.name == "idle") {
				sm.defaultState = state;
			}
			var asm = sm.AddAnyStateTransition (state);
//			asm.hasFixedDuration = false;
//			asm.duration = 0;
		}
		AssetDatabase.SaveAssets ();
		return animatorController;
	}
}
