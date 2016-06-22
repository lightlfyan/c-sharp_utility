using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditorInternal;

public class LoadAni : Editor
{
	[MenuItem ("Tools/GenAnimationCtl")]
	static void GenCtl ()
	{
		if (Selection.activeGameObject == null) {
			Debug.Log ("please select model prefab");
			return;
		}

		var pathstr = AssetDatabase.GetAssetPath (Selection.activeGameObject);
		var path = Path.GetDirectoryName (pathstr) + "/";

		List<AnimationClip> clips = new List<AnimationClip> ();

		var gos = AssetDatabase.LoadAllAssetsAtPath (pathstr);
		foreach (var go in gos) {
			if (go is AnimationClip) {
				if (go.name.StartsWith ("_")) {
					continue;
				}
				clips.Add (go as AnimationClip);
			}
		}
		var ctl = createcontroller (clips, path, Selection.activeGameObject.name + "ctl");
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh ();
	}

	static UnityEditor.Animations.AnimatorController createcontroller (List<AnimationClip> clips, string path, string name)
	{
		UnityEditor.Animations.AnimatorController animatorController = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath (path + name + ".controller");
		UnityEditor.Animations.AnimatorControllerLayer layer = animatorController.layers [0];
		AnimatorStateMachine sm = layer.stateMachine;

		foreach (var clip in clips) {
			animatorController.AddParameter (clip.name, UnityEngine.AnimatorControllerParameterType.Trigger);
			var state = sm.AddState (clip.name);
			state.motion = clip;
			if (clip.name == "Idle") {
				sm.defaultState = state;
			}
			var asm = sm.AddAnyStateTransition (state);
			asm.AddCondition (AnimatorConditionMode.If, 0, clip.name);
//			asm.hasFixedDuration = false;
//			asm.duration = 0;
		}
		AssetDatabase.SaveAssets ();
		return animatorController;
	}
}
