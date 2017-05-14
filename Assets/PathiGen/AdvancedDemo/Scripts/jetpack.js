
var boostHeight : float;
var gravity : float;

function Update () {

	if (Input.GetButton ("Jump")) {
		gameObject.transform.position.y += boostHeight;
	}
	else {
		if (gameObject.transform.position.y > 0.105) {
			gameObject.transform.position.y -= gravity;
		}
	}
	
	if (gameObject.transform.position.y < 0.105) {
		gameObject.transform.position.y = 0.105;
	}
	
	if (gameObject.transform.position.y > 0.66) {
		gameObject.transform.position.y = 0.66;
	}
}