using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(ISTouchControlHandler))]
public class ISTouchHandlerEditor : Editor {

	private ISTouchControlHandler mTH;

	void Awake () {
		mTH = target as ISTouchControlHandler;
	}

	public override void OnInspectorGUI () {
		EditorGUILayout.LabelField("Current Drag Value :");
		EditorGUILayout.LabelField ("  X : " + mTH.dragValue.x + "  Y : " + mTH.dragValue.y);
		EditorGUILayout.LabelField ("Current Scale Value : " + mTH.scaleValue);
		EditorGUILayout.LabelField("Current Rotote Value : "+mTH.rotateValue);

		if (GUI.changed) {
			EditorUtility.SetDirty(mTH);
		}
	}
}
