using UnityEngine;
using System.Collections;

public class MoveWithWaypoint : MonoBehaviour {

	public float rotateSpeed;
	public float moveSpeed;
	public float findNextDist;
	public Transform[] waypoints;

	Transform selfT;
	Transform curWaypoint;
	int waypointIndex = 0;

	void Awake(){
		selfT = transform;
		curWaypoint = waypoints[0];
	}

	void Update () {
		selfT.Translate(selfT.forward*moveSpeed*Time.deltaTime,Space.World);
		Quaternion wantRot = Quaternion.LookRotation(curWaypoint.position-selfT.position);
		selfT.rotation = Quaternion.Slerp(selfT.rotation,wantRot,rotateSpeed*Time.deltaTime);
		if(Vector3.Distance(selfT.position,curWaypoint.position) < findNextDist) FindNextWaypoint();
	}

	void FindNextWaypoint(){
		waypointIndex++;
		if(waypointIndex >= waypoints.Length) waypointIndex = 0;
		curWaypoint = waypoints[waypointIndex];
	}
}
