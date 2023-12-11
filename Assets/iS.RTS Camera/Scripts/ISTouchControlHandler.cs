using UnityEngine;
using System.Collections;

public class ISTouchControlHandler : MonoBehaviour {

	public Vector2 dragValue;
	public float scaleValue;
	public float rotateValue;

	static private ISTouchControlHandler self;

	static public Vector2 currentDragValue{get{ return self.dragValue;}}
	static public float currentScaleValue{get{ return self.scaleValue;}}
	static public float currentRotateValue{get{ return self.rotateValue;}}

	float DPI = 1;

	void Awake(){
		self = this;
		DPI = (float)Mathf.Max (Screen.dpi, 1);
	}

	void Update(){
		CalculateDrag();
		CalculateScale();
		CalculateRotate();
	}

	Vector2 DeltaMovementForTouch(int fingerID){
		Touch touch = Input.touches [fingerID];
		return touch.deltaPosition / DPI;
	}

	float DistanceForTouch(int fingerA, int fingerB){
		return (Input.touches [0].position - Input.touches [1].position).magnitude / DPI;
	}

	void CalculateDrag(){
		if(Input.touchCount != 1){
			dragValue = Vector2.zero;
			return;
		}
		if(Input.touches[0].phase != TouchPhase.Moved){
			dragValue = Vector2.zero;
			return;
		}

		if(Input.touchCount > 0){
			dragValue = -DeltaMovementForTouch(0);
		}
	}

	float lastDist = 0;
	void CalculateScale(){
		if(Input.touchCount != 2){
			scaleValue = 0f;
			lastDist = 0;
			return;
		}

		if (Input.touches [0].phase != TouchPhase.Moved && Input.touches [1].phase != TouchPhase.Moved) {
			scaleValue = 0;
			if (Input.touches [0].phase == TouchPhase.Ended && Input.touches [1].phase == TouchPhase.Ended) lastDist = 0;

			return;
		}

		float curDist = DistanceForTouch(0,1);
		if(lastDist == 0) lastDist = curDist;
		scaleValue = (curDist - lastDist)* -0.01f;
		lastDist = curDist;
	}

	float lastAngle;
	void CalculateRotate(){
		if(Input.touchCount != 2){
			rotateValue = 0;
			lastAngle = 0;
			return;
		}
		Vector2 v2 = (Input.touches[1].position-Input.touches[0].position) / DPI;
		float curAngle = Mathf.Atan2(v2.y,v2.x)*Mathf.Rad2Deg;
		if(lastAngle == 0) lastAngle = curAngle;

		rotateValue = curAngle-lastAngle;
		lastAngle = curAngle;
	}
	
}
