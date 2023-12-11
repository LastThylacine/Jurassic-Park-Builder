using SPINACH.Editor;
using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(ISRTSCamera))]
public class ISRTSCameraEditor : Editor {

	private ISRTSCamera mCam;
	private bool baseSetting;
	private bool boundSetting;
	private bool followSetting;
	private bool mouseSetting;
	private bool keyboardSetting;
	private bool touchSetting;
	private bool previewing;
	private Vector3 op;

	void Awake () {
		mCam = target as ISRTSCamera;
		baseSetting = EditorPrefs.GetBool ("ISRCEBS", false);
		boundSetting = EditorPrefs.GetBool("ISRCEBOS",false);
		followSetting = EditorPrefs.GetBool("ISRCEFS",false);
		mouseSetting = EditorPrefs.GetBool ("ISRCEMS", false);
		keyboardSetting = EditorPrefs.GetBool ("ISRCEKS", false);
		touchSetting = EditorPrefs.GetBool ("ISRCETS", false);
		previewing = EditorPrefs.GetBool ("ISRCEPING", false);
	}

	public override void OnInspectorGUI () {

		Undo.RecordObject (mCam, "ISRTS Camera Setting");

		baseSetting = EditorGUILayout.Foldout(baseSetting,"Basic Setting");
		if (baseSetting) {
			EditorGUILayout.LabelField ("Smoothing Settings");
			mCam.movementLerpSpeed = EditorGUILayout.FloatField("  Movement Lerp Speed",mCam.movementLerpSpeed);
			mCam.rotationLerpSpeed = EditorGUILayout.FloatField("  Rotation Lerp Speed",mCam.rotationLerpSpeed);
			EditorGUILayout.Space ();

			EditorGUILayout.LabelField ("Scroll Settings");

			ISRTSCScrollAnimationType tempType = mCam.scrollAnimationType;
			tempType = (ISRTSCScrollAnimationType)EditorGUILayout.EnumPopup ("  Animation Type", tempType);

			if (tempType != mCam.scrollAnimationType 
				&& EditorUtility.DisplayDialog ("Replacing Changes", "If you switch to another animation type, your settings in current mode will be replaced or modified.", "Continue", "Cancel")) {
				mCam.scrollAnimationType = tempType;
			}


			switch (mCam.scrollAnimationType) {
			case ISRTSCScrollAnimationType.Simple:
				
				Keyframe f_minHigh = mCam.scrollHigh.keys [0];
				Keyframe f_maxHigh = mCam.scrollHigh.keys [mCam.scrollHigh.keys.Length - 1];

				f_minHigh.value = EditorGUILayout.FloatField ("    Min High", f_minHigh.value);
				f_maxHigh.value = EditorGUILayout.FloatField ("    Max High", f_maxHigh.value);

				mCam.scrollHigh = AnimationCurve.Linear (0, f_minHigh.value, 1, f_maxHigh.value);

				EditorGUILayout.Space ();

				Keyframe f_minAngle = mCam.scrollXAngle.keys [0];
				Keyframe f_maxAngle = mCam.scrollXAngle.keys [mCam.scrollXAngle.keys.Length - 1];

				f_minAngle.value = EditorGUILayout.FloatField ("    Min Angle", f_minAngle.value);
				f_maxAngle.value = EditorGUILayout.FloatField ("    Max Angle", f_maxAngle.value);
				f_minAngle.outTangent = EditorGUILayout.FloatField ("    Increase Rate", f_minAngle.outTangent);

				f_maxAngle.inTangent = 1f;

				mCam.scrollXAngle = new AnimationCurve(f_minAngle,f_maxAngle);

				break;

			case ISRTSCScrollAnimationType.Advanced:
				mCam.scrollXAngle = EditorGUILayout.CurveField(new GUIContent("    Scroll X Angle","Scroll X Angle Animation"),mCam.scrollXAngle);
				mCam.scrollHigh = EditorGUILayout.CurveField(new GUIContent("    Scroll High","Scroll High Animation"),mCam.scrollHigh);
				break;
			}

			EditorGUILayout.Space ();

			mCam.scrollValue = EditorGUILayout.Slider("  Start Scroll Value",mCam.scrollValue,0f,1f);
			EditorGUILayout.Space ();

			EditorGUILayout.LabelField ("Casting Settings");
			mCam.groundHighTest = EditorGUILayout.Toggle("  Ground Check",mCam.groundHighTest);
			if(mCam.groundHighTest){
				mCam.groundLayer = EditorGUILayout.LayerField("  Ground Layer",mCam.groundLayer);
			}

			EditorGUILayout.Space ();

			EditorGUILayout.LabelField ("Preview");
			previewing = EditorGUILayout.Toggle ("  Preview Settings", previewing);
			if (previewing) mCam.Adjust2AttitudeBaseOnCurrentSetting ();

			EditorGUILayout.Space ();
			
			EditorGUILayout.LabelField ("UGUI");
			mCam.automaticallyDisableControlOverUGUI = EditorGUILayout.Toggle(
				new GUIContent("  Auto Disable Control", "Disable camera control when mouse is over UI"),
				mCam.automaticallyDisableControlOverUGUI);
			
			EditorGUILayout.Space ();
		}

		boundSetting = EditorGUILayout.Foldout(boundSetting,"Bound");
		if(boundSetting){

			mCam.bound.xMin = EditorGUILayout.FloatField("  Min X",mCam.bound.xMin);
			mCam.bound.xMax = EditorGUILayout.FloatField("  Max X",mCam.bound.xMax);
			mCam.bound.yMin = EditorGUILayout.FloatField("  Min Z",mCam.bound.yMin);
			mCam.bound.yMax = EditorGUILayout.FloatField("  Max Z",mCam.bound.yMax);

			if (GUILayout.Button ("Use Suggested Values") && EditorUtility.DisplayDialog("Replacing Your Setting","Use suggested value will replace your current settings.","Confirm","Cancel")) {
				Bounds[] discoveredBounds;

				MeshRenderer[] renderers = Resources.FindObjectsOfTypeAll<MeshRenderer>();
				discoveredBounds = new Bounds[renderers.Length];

				EditorUtility.DisplayProgressBar ("Calculating...", "Finding objects...", 0);
				for (int i = 0; i < discoveredBounds.Length; i++)
					discoveredBounds [i] = renderers [i].bounds;

				EditorUtility.DisplayProgressBar ("Calculating...", "Calculating bounds along X...", 0.25f);
				float endValues = Mathf.Infinity;
				for (int i = 0; i < discoveredBounds.Length; i++) {
					if (endValues > discoveredBounds [i].min.x)
						endValues = discoveredBounds [i].min.x;
				}
				mCam.bound.xMin = endValues;

				EditorUtility.DisplayProgressBar ("Calculating...", "Calculating bounds along X...", 0.5f);
				endValues = Mathf.NegativeInfinity;
				for (int i = 0; i < discoveredBounds.Length; i++) {
					if (endValues < discoveredBounds [i].max.x)
						endValues = discoveredBounds [i].max.x;
				}
				mCam.bound.xMax = endValues;

				EditorUtility.DisplayProgressBar ("Calculating...", "Calculating bounds along Z...", 0.75f);
				endValues = Mathf.Infinity;
				for (int i = 0; i < discoveredBounds.Length; i++) {
					if (endValues > discoveredBounds [i].min.z)
						endValues = discoveredBounds [i].min.z;
				}
				mCam.bound.yMin = endValues;

				EditorUtility.DisplayProgressBar ("Calculating...", "Calculating bounds along Z...", 0.99f);
				endValues = Mathf.NegativeInfinity;
				for (int i = 0; i < discoveredBounds.Length; i++) {
					if (endValues < discoveredBounds [i].max.z)
						endValues = discoveredBounds [i].max.z;
				}
				mCam.bound.yMax = endValues;

				EditorUtility.ClearProgressBar ();
			}
				
			EditorGUILayout.HelpBox("The white rectangle in scene view will help you configure scene bounds.",MessageType.Info);

			EditorGUILayout.Space ();
		}

		followSetting = EditorGUILayout.Foldout(followSetting,"Follow and Fixed Point");
		if(followSetting){
			mCam.allowFollow = EditorGUILayout.Toggle("  Allow Follow",mCam.allowFollow);
			if(mCam.allowFollow){
				mCam.unlockWhenMove = EditorGUILayout.Toggle("  Unlock When Move",mCam.unlockWhenMove);
			}
			else{
				EditorGUILayout.HelpBox("Enable Follow to let your camera focus something on center of screen or go to a fixed point.",MessageType.Info);
			}

			EditorGUILayout.Space ();
		}

		mouseSetting = EditorGUILayout.Foldout(mouseSetting,"Mouse Control Setting");
		if (mouseSetting) {
			mCam.screenEdgeMovementControl = EditorGUILayout.Toggle("  Screen Edge Movement",mCam.screenEdgeMovementControl);
			if(mCam.screenEdgeMovementControl){
				if(EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android || EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS || EditorUserBuildSettings.activeBuildTarget == BuildTarget.WSAPlayer){
					EditorGUILayout.HelpBox("Please notice that mouse control is not supported on mobile platform. Mouse control is enabled now for you to debug in editor, but it will disable automatically when you build.",MessageType.Warning);
				}
				mCam.desktopMoveSpeed = EditorGUILayout.FloatField("    Move Speed",mCam.desktopMoveSpeed);
			}
			mCam.mouseDragControl = EditorGUILayout.Toggle("  Drag Control",mCam.mouseDragControl);
			if (mCam.mouseDragControl) {
				mCam.mouseDragButton = System.Convert.ToInt32(EditorGUILayout.EnumPopup ("    Move Button", (iSMouseButton)mCam.mouseDragButton));
				mCam.mouseRotateButton = System.Convert.ToInt32(EditorGUILayout.EnumPopup ("    Rotate Button", (iSMouseButton)mCam.mouseRotateButton));
				mCam.desktopMoveDragSpeed = EditorGUILayout.FloatField("    Move Speed",mCam.desktopMoveDragSpeed);
				mCam.desktopRotateSpeed = EditorGUILayout.FloatField("    Rotate Speed",mCam.desktopRotateSpeed);

				if (mCam.mouseDragButton == mCam.mouseRotateButton) {
					EditorGUILayout.HelpBox("Control button overlapping.",MessageType.Warning);
				}
			}

			mCam.mouseScrollControl = EditorGUILayout.Toggle("  Scroll Control",mCam.mouseScrollControl);
			if (mCam.mouseScrollControl) {
				mCam.desktopScrollSpeed = EditorGUILayout.FloatField("    Scroll Speed",mCam.desktopScrollSpeed);
			}

			EditorGUILayout.Space ();
		}
		else{
			EditorGUILayout.HelpBox("Enable Mouse Control to control camera with mouse.",MessageType.Info);
		}

		keyboardSetting = EditorGUILayout.Foldout(keyboardSetting,"Keyboard Control Setting");
		if (keyboardSetting) {
			mCam.keyBoardControl = EditorGUILayout.Toggle("  Enabled",mCam.keyBoardControl);
			if(mCam.keyBoardControl){
				mCam.desktopMoveSpeed = EditorGUILayout.FloatField("  Move Speed",mCam.desktopMoveSpeed);
				mCam.desktopRotateSpeed = EditorGUILayout.FloatField("  Rotate Speed",mCam.desktopRotateSpeed);

				EditorGUILayout.Space ();

				string[] axisList = ISEInputAxisReader.GetInputAxisList ();


				mCam.horizontalKeyboardAxis = StringPopup ("  Horizontal Axis", axisList, mCam.horizontalKeyboardAxis);
				mCam.verticalKeyboardAxis = StringPopup ("  Vertical Axis", axisList, mCam.verticalKeyboardAxis);
				mCam.rotateAxis = StringPopup ("  Rotate Axis", axisList, mCam.rotateAxis);

			}
			else{
				EditorGUILayout.HelpBox("Enabel Keyboard Control to control camera with keyboard.",MessageType.Info);
			}

			EditorGUILayout.Space ();
		}

		touchSetting = EditorGUILayout.Foldout (touchSetting, "Touch Control Setting");
		if (touchSetting) {
			mCam.touchControl = EditorGUILayout.Toggle("  Enabled",mCam.touchControl);
			if(mCam.touchControl){
				
				ISTouchControlHandler[] handler = GameObject.FindObjectsOfType<ISTouchControlHandler>();
				if (PrefabUtility.GetPrefabType(mCam.gameObject) != PrefabType.Prefab && handler.Length <= 0){
					EditorGUILayout.HelpBox ("Touch input requires at less one ISTouchControlHandler to work. Please use Add Component to add one to any GameObject to continue. Or you can :", MessageType.Error);
					if (GUILayout.Button ("Add to this GameObject")) {
						mCam.gameObject.AddComponent<ISTouchControlHandler> ();
					}
				} else {
					mCam.touchMoveSpeed = EditorGUILayout.FloatField("  Move Speed",Mathf.Max(mCam.touchMoveSpeed,0.000001f));
					mCam.touchRotateSpeed = EditorGUILayout.FloatField("  Rotate Speed",Mathf.Max(mCam.touchRotateSpeed,0.000001f));
					mCam.touchScrollSpeed = EditorGUILayout.FloatField("  Scroll Speed",Mathf.Max(mCam.touchScrollSpeed,0.000001f));
				}
			}
			else{
				EditorGUILayout.HelpBox("Enable Touch control to control camera with touch screen.",MessageType.Info);
			}

			EditorGUILayout.Space ();
		}

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		EditorGUILayout.HelpBox ("Store your current setting into preset, so you can use them later by \" Tools > ISRTS Camera > Create From Preset\".",MessageType.Info,true);

		if (GUILayout.Button ("Replace Preset with Current Settings") 
		&& EditorUtility.DisplayDialog("Replacing Preset","You are about to replace existing preset with your current setting, this cannot be undo.","Comfirm","Cancel")) {

			PrefabUtility.CreatePrefab ("Assets/iS.RTS Camera/Editor/Presets/iS.RTS Camera.prefab", mCam.gameObject);
		}

		if (GUI.changed) {
			EditorPrefs.SetBool ("ISRCEBS", baseSetting);
			EditorPrefs.SetBool ("ISRCEBOS", boundSetting);
			EditorPrefs.SetBool ("ISRCEFS", followSetting);
			EditorPrefs.SetBool ("ISRCEMS", mouseSetting);
			EditorPrefs.SetBool ("ISRCEKS", keyboardSetting);
			EditorPrefs.SetBool ("ISRCETS", touchSetting);
			EditorPrefs.GetBool ("ISRCEPING", previewing);

			EditorUtility.SetDirty (mCam);
		}
	}

	string StringPopup(string label, string[] list, string currentValue){
		int[] optionList = new int[list.Length];
		int curID = 0;
		for (int i = 0; i < list.Length; i++){
			optionList [i] = i;
			if (list [i] == currentValue)
				curID = i;
		}

		curID = EditorGUILayout.IntPopup (label, curID, list, optionList);
		return list [curID];
	}
}
