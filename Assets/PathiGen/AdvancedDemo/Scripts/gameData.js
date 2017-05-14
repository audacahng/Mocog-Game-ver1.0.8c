
static var levelSpeed : float;
var levelSpeedMin : float;  				// The minimum speed the level can move at
var levelSpeedMax : float;  				//The maximum speed the level can move at

static var levelSpeedMaxStatic : float;
static var playerVelocityMultiplier : float;

static var tileCount : int;  				//The cumulative number of tiles that have been generated
var totalTileCount : int;  					//The maximum number of tiles that will be generated in the scene
var maxActiveTiles : int;
static var maxActiveTilesStatic : int;
static var maxTiles : boolean;

static var distanceTravelled : float;
static var distanceRound : float;
static var distanceMeterText : String;

var textColour : Color;

static var finishLineCrossed : boolean = false;

var decelRate : float;
var decelTime : float;

function Awake () {

	maxTiles = false;
	
	levelSpeedMaxStatic = levelSpeedMax;
	maxActiveTilesStatic = maxActiveTiles;
	finishLineCrossed = false;
	
}

function Update () {
	
	//Distance Travelled
	distanceTravelled += (-1 * levelSpeed * Time.deltaTime);
	distanceRound = Mathf.Round(distanceTravelled * 100.00) / 10.00;
		
	if (finishLineCrossed != true) {
		//Set the level speed to the amount specified in the inspector
		levelSpeed = (-1 * acceleration.currentSpeed);
	
		if (totalTileCount > 0) {
			if (tileCount >= totalTileCount) {
				maxTiles = true;
			}
		}
	
		playerVelocityMultiplier = (-1 * (levelSpeed / levelSpeedMax));
		//Debug.Log("Player Velocity " +playerVelocityMultiplier);
	
		//DEBUG - Displays whether or not the player has crossed the finish line
		//Debug.Log("Finished Line Status = " +finishLineCrossed);
	}
	
	else if (finishLineCrossed == true) {
		SlowDownStop();
	}
}

function SlowDownStop () {

var decelRate = decelTime;
	
	while (decelRate > 0) {

		levelSpeed = Mathf.Lerp(levelSpeed, 0, (decelTime - decelRate) / decelTime);

		yield;
		decelRate -= Time.deltaTime;
	}
}

function OnGUI(){

	//Displays the speed the world is moving at
	var velocityMeter = gameObject.Find("GUI-playerVelocity");
	velocityMeter.GetComponent.<GUIText>().text = ("Level Speed: " +(-1 * levelSpeed));
	velocityMeter.GetComponent.<GUIText>().material.color = textColour;
	
	//Display the number of tiles generated compared to the maximum number allowed
	var tileMeter = gameObject.Find("GUI-tileCount");
	tileMeter.GetComponent.<GUIText>().text = ("Tile Count: " +(tileCount) +" / " +(totalTileCount));
	tileMeter.GetComponent.<GUIText>().material.color = textColour;
	
	//Display the distance (in meters) that the player has travelled
	var distanceMeter = gameObject.Find("GUI-distanceTravelled");
	//distanceMeter.guiText.text = distanceMeterText.Format("{0:0.00}", ("Distance Travelled: " +(distanceTravelled) +"m"));
	distanceMeter.GetComponent.<GUIText>().text = ("Distance Travelled: " +(distanceRound) +"m");
	distanceMeter.GetComponent.<GUIText>().material.color = textColour;

}