/*
 * Ball.js
 * Copyright (C) 2011-2012 Zeemote. All rights reserved.
 *   This is a sample script.
 */
 
var stayStageFlag : boolean = false;

function FixedUpdate () {

	/* Get Zeemote Controller Input and move the ball. */
	GetComponent.<Rigidbody>().AddForce(ZeemoteInput.GetAxis("Horizontal") * 20.0f, 0, ZeemoteInput.GetAxis("Vertical") * 20.0f);

	if(ZeemoteInput.GetAxis("Fire1") && this.stayStageFlag) {
		/* If the Fire1 button is pushed, the ball is jumped. */
		GetComponent.<Rigidbody>().velocity.y = 15.0f;
	}

	/* If the ball falls from the stage, the ball will be returned to the original position. */ 
	if(GetComponent.<Rigidbody>().position.y < -30.0f) {
		GetComponent.<Rigidbody>().position.y = 10.0f;
		GetComponent.<Rigidbody>().position.x = GetComponent.<Rigidbody>().position.z = 0.0f;
		GetComponent.<Rigidbody>().velocity = Vector3(0, 0, 0);
	}
}

function OnCollisionStay(collisionInfo : Collision) {
	/* Stay in the stage. */
	stayStageFlag = true;
}

function OnCollisionExit(collisionInfo : Collision) {
	/* Exit from the stage. */
	stayStageFlag = false;
}