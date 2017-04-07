using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour {
	private Transform lookAt;
	private Vector3 startOffSet;
	private Vector3 moveVector;
	private Vector3 animationOffSet = new Vector3 (0, 5, 5);
	private float animationDuration=3;
	private float transition=0;
	// Use this for initialization
	void Start () {
		lookAt = GameObject.FindGameObjectWithTag ("Player").transform;
		startOffSet = transform.position - lookAt.position;
	}
	
	// Update is called once per frame
	void Update () {
		moveVector = lookAt.position + startOffSet;
		//X 
		moveVector.x=0;
		//Y 
		moveVector.y=Mathf.Clamp(moveVector.y,3,5);
		if (transition > 1) {
			transform.position = moveVector;
		} else {
			//Animation at the start game
			transform.position=Vector3.Lerp(moveVector+animationOffSet,moveVector,transition);
			transition += Time.deltaTime * 1 / animationDuration;
			transform.LookAt (lookAt.position + Vector3.up);
		}
	}
}
