using UnityEngine;
using System.Collections;

public class FollowWhenClick : MonoBehaviour {

	void OnMouseUp(){
		ISRTSCamera.FollowForMain(transform);
	}
}
