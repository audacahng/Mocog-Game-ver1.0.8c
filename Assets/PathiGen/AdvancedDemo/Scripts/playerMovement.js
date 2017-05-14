
var horizontalSpeed : float;

var verticalSpeed : float;
var tilt : float;
var theCamera : GameObject;

function Update () {

	//Rigidbody Movement
	if(Input.GetButton("Horizontal")) {		
		var moveX : float = Input.GetAxis("Horizontal");
		GetComponent.<Rigidbody>().MovePosition(GetComponent.<Rigidbody>().position + Vector3((moveX * horizontalSpeed * gameData.playerVelocityMultiplier * Time.deltaTime), 0, 0));
	}
	
	if(Input.GetButton("Vertical")) {
		var moveZ : float = Input.GetAxis("Vertical");
		GetComponent.<Rigidbody>().MovePosition(GetComponent.<Rigidbody>().position + Vector3(0, 0,(moveZ * verticalSpeed * gameData.playerVelocityMultiplier * Time.deltaTime)));
	}
	
	//Player Tilting
	transform.eulerAngles.z = moveX * gameData.playerVelocityMultiplier * -1 * tilt;
	
	//Camera FOV Change
	//theCamera.camera.fieldOfView = (90 + (gameData.playerVelocityMultiplier * 20)); 
}