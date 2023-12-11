using UnityEngine;
using UnityEditor;
using System.Collections;

namespace SPINACH.Editor{

	public enum InputType
	{
		KeyOrMouseButton,
		MouseMovement,
		JoystickAxis,
	};

	public class ISEInputAxisReader : MonoBehaviour {
		static public string[] GetInputAxisList(){
			var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];

			SerializedObject obj = new SerializedObject(inputManager);

			SerializedProperty axisArray = obj.FindProperty("m_Axes");

			string[] list = new string[axisArray.arraySize];

			for (int i = 0; i < axisArray.arraySize; ++i) {
				var axis = axisArray.GetArrayElementAtIndex (i);

				list[i] = axis.FindPropertyRelative ("m_Name").stringValue;
			}

			return list;
		}
	}
}