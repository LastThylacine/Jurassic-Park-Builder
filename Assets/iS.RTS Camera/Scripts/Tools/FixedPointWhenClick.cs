using UnityEngine;
using System.Collections;

public class FixedPointWhenClick : MonoBehaviour {

	public Transform point;

	void OnMouseUp(){
				ISRTSCamera.LockFixedPointForMain(point);
	}
}
