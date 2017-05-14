var speedMin = 0.0;
var speedMax = 1.0;
var accelerateTime : float;
static var currentSpeed : float = 0.0;
private var accelTimePassed : float = 0.0;
private var decelTimePassed : float = 0.0;

function FixedUpdate() {

	if (gameData.finishLineCrossed != true) {
		Accelerate();
	}

	if (Input.GetAxis("Fire3")) {
		Decelerate();
	}
   
	if (gameData.finishLineCrossed == true) {
		if (gameData.levelSpeed >= 0.01) {
			var decelerateTime : float;
			gameData.levelSpeed -= Time.deltaTime;
			currentSpeed = decelerateTime / 5;
		}
	}
}

function Accelerate() {

	accelTimePassed += Time.deltaTime;
	currentSpeed = Mathf.Lerp(speedMin, gameData.levelSpeedMaxStatic, accelTimePassed/accelerateTime);
}

function Decelerate() {

	decelTimePassed += Time.deltaTime;
	currentSpeed = Mathf.Lerp(gameData.levelSpeedMaxStatic, speedMin, decelTimePassed/accelerateTime);
}