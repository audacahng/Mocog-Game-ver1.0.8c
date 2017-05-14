// /-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\
//
// 							PathiGen 1.0, Copyright © 2012, RipCord Development
//
// \-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/

//ABOUT - This script generates a path of a defined or infinite length using an array of gameObjects that are either randomly chosen, or selected in the order they appear in the array.
//		- This script should be placed on an empty game object in your scene.


var pathPieces : GameObject[];			//This is an array of all the prefabs that will be used to create the generated path.
var endPath: GameObject;  				//The last path piece that will be generated at the end of the level


private var selectedPath : GameObject;
private var y : int;					//This variable is used to determine the next pathPiece to select if the pathStyle is set to "useArrayOrder"

static var activeTileCount : int;  		//Keeps track of how many tiles are currently active in the scene
private var generateTiles : boolean;

function Start () {
	
	generateTiles = true;  //Activates the tile generator
	activeTileCount = 1;  //Sets the number of active tiles in the scene to 1
	gameData.tileCount = 1;  //Sets the accumulated number of tiles in the scene to 1
	
	var x : int;	//This variable will be used in the following loop
	
}

function LateUpdate () {

	if (generateTiles == true) {
		if (gameData.maxTiles != true) {
			if (activeTileCount <= gameData.maxActiveTilesStatic) {
				GeneratePath();
			}
		}
	
		if (gameData.maxTiles == true) {
			GeneratePathEnd();
			generateTiles = false;
		}
	}
}

function GeneratePath () {

	var pathSpawnPoint = GameObject.Find("POINT roadEnd");			//Find the end connector of the last path piece generated and set it as the spawn point for the next path piece.
	Destroy(GameObject.Find("POINT roadEnd"));						//Destroy the end connector once it is no longer needed
	
	selectedPath = pathPieces[Random.Range(0, pathPieces.Length)];						//Picks a pathPiece from the pathPieces array
	
	var newPath = Instantiate (selectedPath, pathSpawnPoint.transform.position, pathSpawnPoint.transform.rotation);
	activeTileCount++;
	gameData.tileCount++;
	
}

function GeneratePathEnd() {

	var pathSpawnPoint = GameObject.Find("POINT roadEnd");
	Destroy(GameObject.Find("POINT roadEnd"));	
		
	var newPath = Instantiate (endPath, pathSpawnPoint.transform.position, pathSpawnPoint.transform.rotation);

}