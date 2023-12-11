using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// ISRTS Camera
/// Created by SPINACH.
/// 
/// © 2013 - 2016 SPINACH All rights reserved.
/// </summary>

public enum iSMouseButton{
	Left,Right,Middle
}

public class ISRTSCamera : MonoBehaviour {
	
	public ISRTSCScrollAnimationType scrollAnimationType;
	public AnimationCurve scrollXAngle = AnimationCurve.Linear(0,0,1,1);
	public AnimationCurve scrollHigh = AnimationCurve.Linear(0,0,1,1);
	public float groundHigh;
	public float minHigh;
	public bool groundHighTest;
	public int groundLayer;

	public float scrollValue;
	public bool unlockWhenMove;

	public float movementLerpSpeed;
	public float rotationLerpSpeed;

	public Rect bound;
	public Transform followingTarget;
	public Transform fixedPoint;

	public float desktopScrollSpeed;
	public float desktopMoveSpeed;
	public float desktopMoveDragSpeed;
	public float desktopRotateSpeed;

	public float touchMoveSpeed;
	public float touchScrollSpeed;
	public float touchRotateSpeed; 

	public string horizontalKeyboardAxis;
	public string verticalKeyboardAxis;
	public string rotateAxis;

	public int mouseDragButton;
	public int mouseRotateButton;

	public Vector3 objectPos;

	private Transform selfT;

	public float wantYAngle;
	public float wantXAngle;

	public bool keyBoardControl;
	public bool mouseRotateControl;
	public bool screenEdgeMovementControl;
	public bool mouseDragControl;
	public bool mouseScrollControl;
	public bool touchControl;
	public bool allowFollow;

	public bool controlDisabled = false;
	public bool automaticallyDisableControlOverUGUI = true;
	
	static private ISRTSCamera self;

	private LayerMask groundMask;

	#region runtime_control_switch
	public void KeyboardControl(bool enable){
		keyBoardControl = enable;
		if (keyBoardControl) StartCoroutine (UpdateKeyboardControl ());
	}

	public void ScreenEdgeMovementControl(bool enable){
		
		if (Application.isMobilePlatform) return;

		screenEdgeMovementControl = enable;
		if (screenEdgeMovementControl) StartCoroutine (UpdateScreenEdgeControl ());
	}

	public void MouseDragControl(bool enable){

		if (Application.isMobilePlatform) return;

		mouseDragControl = enable;
		if (mouseDragControl) StartCoroutine (UpdateMouseDragControl ());
	}

	public void MouseScrollControl(bool enable){

		if (Application.isMobilePlatform) return;

		mouseScrollControl = enable;
		if (mouseScrollControl) StartCoroutine (UpdateMouseScrollControl ());
	}

	public void TouchControl(bool enable){
		touchControl = enable;
		if (touchControl) StartCoroutine (UpdateTouchControl ());
	}
	#endregion

	#region static_methods
	static public ISRTSCamera GetInstantiated(){ return self;}

	static public void LockFixedPointForMain(Transform pos){
		if(self.allowFollow){
			self.followingTarget = null;
			self.fixedPoint = pos;

			//Set the wantPos to make camera more smooth when leave fixed point.
			self.objectPos.x = pos.position.x;
			self.objectPos.z = pos.position.z;
		}
	}

	static public void UnlockFixedPointForMain(){
		self.fixedPoint = null;
	}

	static public void JumpToTargetForMain(Transform target){
		self.objectPos.x = target.position.x;
		self.objectPos.z = target.position.z;
	}

	static public void FollowForMain(Transform target){
		if(self.allowFollow){
			self.fixedPoint = null;
			self.followingTarget = target;
		}
	}

	static public void CancelFollowForMain(){
		self.followingTarget = null;
	}

	static public Transform GetFollowingTarget(){return self.followingTarget;}
	static public Transform GetFixedPoint(){return self.fixedPoint;}
	#endregion

		public void LockFixedPoint(Transform pos){
		if(self.allowFollow){
			self.followingTarget = null;
			self.fixedPoint = pos;
		}
	}

	public void UnlockFixedPoint(){
		self.fixedPoint = null;
	}

	public void Follow(Transform target){
		if(self.allowFollow){
			self.fixedPoint = null;
			self.followingTarget = target;
		}
	}

	public void CancelFollow(){
		self.followingTarget = null;
	}

	public Vector3 CalculateCurrentObjectPosition(){
		
		float dist = objectPos.y * Mathf.Tan((90f - wantXAngle) * Mathf.Deg2Rad);

		Vector3 objectPosDir = -(transform.rotation * (-Vector3.forward * dist));
		return transform.position + objectPosDir;
	}

	/// <summary>
	/// Adjust to the attitude base on current setting.
	/// Editor use this method to generate preview.
	/// </summary>
	public void Adjust2AttitudeBaseOnCurrentSetting(){
		groundMask = 1<<groundLayer;
		objectPos = CalculateCurrentObjectPosition ();
		scrollValue = Mathf.Clamp01(scrollValue);

		float currentGroundHigh = groundHigh;

		RaycastHit hit;
		Vector3 emitPos = objectPos;
		emitPos.y += 9999f;
		if (groundHighTest && Physics.Raycast (emitPos, -Vector3.up, out hit, Mathf.Infinity, groundMask)) {
			currentGroundHigh = hit.point.y;
		}

		emitPos = transform.position;
		emitPos.y += 9999f;
		if (groundHighTest && Physics.Raycast (emitPos, -Vector3.up, out hit, Mathf.Infinity, groundMask)) {
			currentGroundHigh = Mathf.Max(currentGroundHigh,hit.point.y);
		}

		Vector3 rot = transform.eulerAngles;
		rot.x = ISMath.WrapAngle(rot.x);
		rot.y = ISMath.WrapAngle(rot.y);
		wantYAngle = rot.y;

		objectPos.y = scrollHigh.Evaluate(scrollValue);
		wantXAngle = scrollXAngle.Evaluate(scrollValue);

		Quaternion targetRot = Quaternion.Euler(wantXAngle,wantYAngle,0f);
		transform.rotation = targetRot;

		float dist = objectPos.y * Mathf.Tan((90f - wantXAngle) * Mathf.Deg2Rad);   

		Vector3 cameraPosDir = targetRot * (Vector3.forward * dist);

		Vector3 cameraPos = objectPos - cameraPosDir;
		cameraPos.y = objectPos.y+currentGroundHigh;

		transform.position = cameraPos;
	}

	void Awake () {
		self = this;
		selfT = transform;
		groundMask = 1<<groundLayer;
	}

	public void Start(){
		objectPos = CalculateCurrentObjectPosition();
		scrollValue = Mathf.Clamp01(scrollValue);
		objectPos.y = scrollHigh.Evaluate(scrollValue);
		wantXAngle = scrollXAngle.Evaluate(scrollValue);

		Vector3 rot = selfT.eulerAngles;
		rot.x = ISMath.WrapAngle(rot.x);
		rot.y = ISMath.WrapAngle(rot.y);
		wantYAngle = rot.y;
		rot.x = scrollXAngle.Evaluate(scrollValue);
		wantXAngle = rot.x;
		selfT.eulerAngles = rot;

		StartCoroutine (UpdateTransform ());

		KeyboardControl (keyBoardControl);
		ScreenEdgeMovementControl (screenEdgeMovementControl);
		MouseDragControl (mouseDragControl);
		MouseScrollControl (mouseScrollControl);
		TouchControl (touchControl);
	}

	private void Update() {
		if (automaticallyDisableControlOverUGUI && EventSystem.current) {
			controlDisabled = EventSystem.current.IsPointerOverGameObject();
		}
	}

	IEnumerator UpdateKeyboardControl(){
		while (true) {
			if(!keyBoardControl) break;

			Move (Input.GetAxisRaw (horizontalKeyboardAxis) * selfT.right, desktopMoveSpeed * Time.deltaTime);
			Move (Input.GetAxisRaw (verticalKeyboardAxis) * selfT.forward, desktopMoveSpeed * Time.deltaTime);
			Rotate (Input.GetAxisRaw (rotateAxis) * desktopMoveSpeed * Time.deltaTime);

			yield return null;
		}
	}

	IEnumerator UpdateScreenEdgeControl(){
		while (true) {
			
			if(!screenEdgeMovementControl) break;

			if(Input.mousePosition.y >= Screen.height - 1f) Move(selfT.forward,desktopMoveSpeed * Time.deltaTime);
			if(Input.mousePosition.y <= 1) Move(-selfT.forward,desktopMoveSpeed * Time.deltaTime);
			if(Input.mousePosition.x <= 1) Move(-selfT.right,desktopMoveSpeed * Time.deltaTime);
			if(Input.mousePosition.x >= Screen.width -1f) Move(selfT.right,desktopMoveSpeed * Time.deltaTime);

			yield return null;
		}
	}

	IEnumerator UpdateMouseScrollControl(){
		while (true) {

			if(!mouseScrollControl) break;

			Scroll (Input.GetAxis ("Mouse ScrollWheel") * desktopScrollSpeed);

			yield return null;
		}
	}

	IEnumerator UpdateMouseDragControl(){
		while (true) {

			if(!mouseDragControl) break;

			if(Input.GetMouseButton(mouseRotateButton)){
				Rotate (Input.GetAxis ("Mouse X") * desktopRotateSpeed);
			}
				
			if (Input.GetMouseButton (mouseDragButton)) {
				float mouseX = Input.GetAxis ("Mouse X") / Screen.width * 10000f;
				float mouseY = Input.GetAxis ("Mouse Y") / Screen.height * 10000f;
				
				NormalizedMove (-selfT.right * mouseX, desktopMoveDragSpeed * Time.deltaTime);
				NormalizedMove (-selfT.forward * mouseY, desktopMoveDragSpeed * Time.deltaTime);
			}
			yield return null;
		}
	}

	IEnumerator UpdateTouchControl(){
		while (true) {
			Vector2 dV = ISTouchControlHandler.currentDragValue;
			if(dV != Vector2.zero){
				Quaternion fixedDir = Quaternion.Euler(new Vector3(0,selfT.eulerAngles.y,selfT.eulerAngles.z));
				Vector3 forwardDir = fixedDir*Vector3.forward;
				Move(dV.y*forwardDir,touchMoveSpeed);
				Move(dV.x*selfT.right,touchMoveSpeed);
			}
			if(ISTouchControlHandler.currentScaleValue != 0) Scroll(ISTouchControlHandler.currentScaleValue*touchScrollSpeed);
			if(ISTouchControlHandler.currentRotateValue != 0) Rotate(ISTouchControlHandler.currentRotateValue*touchRotateSpeed);
			yield return null;
		}
	}

	public void Move(Vector3 dir,float offset){
		if(controlDisabled) return;
		
		dir.y = 0;
		dir.Normalize ();
		dir*= offset;
		if(unlockWhenMove && dir != Vector3.zero){
			followingTarget = null;
			fixedPoint = null;
		}
		objectPos += dir;

		objectPos.x = Mathf.Clamp(objectPos.x,bound.xMin,bound.xMax);
		objectPos.z = Mathf.Clamp(objectPos.z,bound.yMin,bound.yMax);
	}

	public void NormalizedMove(Vector3 dir,float offset){
		if(controlDisabled) return;
		
		dir.y = 0;
		dir*= offset;
		if(unlockWhenMove && dir != Vector3.zero){
			followingTarget = null;
			fixedPoint = null;
		}
		objectPos += dir;

		objectPos.x = Mathf.Clamp(objectPos.x,bound.xMin,bound.xMax);
		objectPos.z = Mathf.Clamp(objectPos.z,bound.yMin,bound.yMax);
	}
	
	public void Rotate(float dir){
		if(controlDisabled) return;
		
		wantYAngle += dir;
		ISMath.WrapAngle(wantYAngle);
	}

	public void Scroll(float value){
		if(controlDisabled) return;
		
		scrollValue += value;
		scrollValue = Mathf.Clamp01(scrollValue);
		objectPos.y = scrollHigh.Evaluate(scrollValue);
		wantXAngle = scrollXAngle.Evaluate(scrollValue);
	}

	IEnumerator UpdateTransform(){
		while (true) {
			
			Vector3 cameraPosDir;
			Vector3 cameraPos;

			if(!fixedPoint){
				float currentGroundHigh = groundHigh;

				//Set wanted position to target's position if we are following something.
				if(followingTarget){
					objectPos.x = followingTarget.position.x;
					objectPos.z = followingTarget.position.z;
				}

				//Calculate vertical distance to ground to avoid intercepting ground.
				RaycastHit hit;
				Vector3 emitPos = objectPos;
				emitPos.y += 9999f;
				if (groundHighTest && Physics.Raycast (emitPos, -Vector3.up, out hit, Mathf.Infinity, groundMask)) {
					currentGroundHigh = hit.point.y;
				}

				emitPos = selfT.position;
				emitPos.y += 9999f;
				if (groundHighTest && Physics.Raycast (emitPos, -Vector3.up, out hit, Mathf.Infinity, groundMask)) {
					currentGroundHigh = Mathf.Max(currentGroundHigh,hit.point.y);
				}

				//Lerp actual rotation to wanted value.
				Quaternion targetRot = Quaternion.Euler(wantXAngle,wantYAngle,0f);
				selfT.rotation = Quaternion.Slerp (selfT.rotation, targetRot, rotationLerpSpeed * Time.deltaTime);

				//Calculate a world position refers to the center of screen.
				float dist = objectPos.y * Mathf.Tan((90f - wantXAngle) * Mathf.Deg2Rad);

				//Use this vector to move camera back and rotate.
				Quaternion targetYRot = Quaternion.Euler(0f, wantYAngle, 0f);
				cameraPosDir = targetYRot * (Vector3.forward * dist);

				//Calculate the actual world position to prepare to move our camera object.
				cameraPos = objectPos - cameraPosDir;
				cameraPos.y = (objectPos.y + (followingTarget ? followingTarget.position.y : currentGroundHigh));

				//Lerp to wanted position.
				selfT.position = Vector3.Lerp(selfT.position,cameraPos,movementLerpSpeed*Time.deltaTime);
			}
			else{
				//If we are positioning to a fixed point, we simply move to it.
				selfT.rotation = Quaternion.Slerp(selfT.rotation,fixedPoint.rotation,rotationLerpSpeed*Time.deltaTime);
				selfT.position = Vector3.Lerp(selfT.position,fixedPoint.position,movementLerpSpeed*Time.deltaTime);

				//We also keep objectPos to fixedPoint to make a stable feeling while leave fixed point mode.
				objectPos.x = fixedPoint.position.x;
				objectPos.z = fixedPoint.position.z;
			}
			yield return null;
		}
	}

	void OnDrawGizmosSelected(){
		
		//Draw debug lines.
		Vector3 mp = transform.position;
		Gizmos.DrawLine(new Vector3(bound.xMin,mp.y,bound.yMin),new Vector3(bound.xMin,mp.y,bound.yMax));
		Gizmos.DrawLine(new Vector3(bound.xMin,mp.y,bound.yMax),new Vector3(bound.xMax,mp.y,bound.yMax));
		Gizmos.DrawLine(new Vector3(bound.xMax,mp.y,bound.yMax),new Vector3(bound.xMax,mp.y,bound.yMin));
		Gizmos.DrawLine(new Vector3(bound.xMax,mp.y,bound.yMin),new Vector3(bound.xMin,mp.y,bound.yMin));
	}
}

public enum ISRTSCScrollAnimationType{
	Simple, Advanced
}