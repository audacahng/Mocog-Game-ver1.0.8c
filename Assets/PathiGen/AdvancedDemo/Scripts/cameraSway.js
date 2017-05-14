
var useX : boolean;
var xTarget : GameObject;
var swayMultiplier : float;

var useY : boolean;
var yTarget : GameObject;
var yOffset : float;

var useZ : boolean;
var zTarget : GameObject;
var cameraOffset : float;

function Update () {

	if (useX == true) {
		transform.position.x = xTarget.transform.position.x * swayMultiplier;
	}
	
	if (useY == true) {
		transform.position.y = yTarget.transform.position.y + yOffset;
	}
	
	if (useZ == true) {
		transform.position.z = zTarget.transform.position.z + cameraOffset;
	}
}