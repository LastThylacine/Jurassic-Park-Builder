using UnityEngine;
using UnityEditor;
using System.Collections;

public class ISRTSCameraEditorMenuBar {

	[MenuItem("Tools/iS.RTS Camera/Create From Preset/iS.RTS Camera")]
	static public void CreateFromPreset(){
			GameObject preset =	AssetDatabase.LoadMainAssetAtPath("Assets/iS.RTS Camera/Editor/Presets/iS.RTS Camera.prefab") as GameObject;
			GameObject obj = MonoBehaviour.Instantiate (preset, Vector3.zero, Quaternion.identity) as GameObject;
			
			obj.name = "iS.RTS Camera";
			Selection.activeGameObject = obj;
	}

	[MenuItem("Tools/iS.RTS Camera/Create From Preset/iS.RTS Camera 2D")]
	static public void CreateFromPreset2D(){
		GameObject preset =	AssetDatabase.LoadMainAssetAtPath("Assets/iS.RTS Camera/Editor/Presets/iS.RTS Camera2D.prefab") as GameObject;
		GameObject obj = MonoBehaviour.Instantiate (preset, Vector3.back, Quaternion.identity) as GameObject;

		obj.name = "iS.RTS Camera2D";
		Selection.activeGameObject = obj;
	}

	[MenuItem("Tools/iS.RTS Camera/Make Selected GameObject As.../iS.RTS Camera")]
	static public void MakeAsCamera(){
		if (Selection.activeGameObject)
			Selection.activeGameObject.AddComponent<ISRTSCamera> ();
	}

	[MenuItem("Tools/iS.RTS Camera/Make Selected GameObject As.../iS.RTS Camera 2D")]
	static public void MakeAsCamera2D(){
		if (Selection.activeGameObject)
			Selection.activeGameObject.AddComponent<ISRTSCamera2D> ();
	}
}
