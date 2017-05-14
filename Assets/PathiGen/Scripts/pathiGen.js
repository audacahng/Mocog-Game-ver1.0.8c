// /-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\
//
// 							PathiGen 1.0, Copyright © 2012, RipCord Development
//
// \-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/

//ABOUT - This script generates a path of a defined or infinite length using an array of gameObjects that are either randomly chosen, or selected in the order they appear in the array.
//		- This script should be placed on an empty game object in your scene.


public enum PathStyle { useArrayOrder, usePathPieceFrequency, useRandomFromArray };
var pathStyle : PathStyle;

var pathPieces : GameObject[];			//This is an array of all the prefabs that will be used to create the generated path.

var pathPieceFrequency : float[];		//The size of this array must be equal to the size of the pathPieces array for the individual frequency values to line up properly.
private var totalFreq : float = 0;		//This value is used to calculate the total value of all the frequency numbers in the pathPieceFrequency array

var pathPieceLimit : int;				//Sets a limit to how many pathPieces can be in the scene at a time.
var newPathTime : float;				//The delay, in seconds, before a new path piece is generated.

private var selectedPath : GameObject;
private var y : int;					//This variable is used to determine the next pathPiece to select if the pathStyle is set to "useArrayOrder"


function Start () {

	if (pathStyle == pathStyle.usePathPieceFrequency) {

		//Add up all frequency values to determine TotalFrequency value
		for (var i = 0; i < pathPieceFrequency.Length; i++) {
			totalFreq += pathPieceFrequency[i];
		}
	}
	
	var x : int;	//This variable will be used in the following loop
	
	//If the pathPieceLimit is set to 0, the pathGenerator will treat it as no limit and will run infinitely
	if (pathPieceLimit != 0) {
	
		while (x <= pathPieceLimit){
			yield WaitForSeconds(newPathTime);
			GeneratePath();
   	  		x++;
		}
	}
	
	else {
	
		while (true){
			yield WaitForSeconds(newPathTime);
			GeneratePath();
		}
	}
}

function GeneratePath () {

	var pathSpawnPoint = GameObject.Find("POINT roadEnd");			//Find the end connector of the last path piece generated and set it as the spawn point for the next path piece.
//	Debug.Log("Spawn at:" +(pathSpawnPoint as Transform).name );
	
	Destroy(GameObject.Find("POINT roadEnd"));						//Destroy the end connector once it is no longer needed
	
	var index=-1;
	//PATH PIECE FREQUENCY --- Use pathPieceFrequency to determine next path piece
	if (pathStyle == pathStyle.usePathPieceFrequency) {
		var roll : float = Random.Range(0,totalFreq);				//Choose a number between 0 and the total frequency values

		//Set the index value.  This will be used to choose which pathPiece is generated
		for (var n = 0; n < pathPieceFrequency.Length; n++) {
			if (roll <= pathPieceFrequency[n]) { 
				index = n; break; 
			}
  			roll -= pathPieceFrequency[n];
		}

		if (index == -1) index = pathPieceFrequency.Length-1;	//In case the roll manages to be above the highest value, the roll is automatically set to the highest value
	
		selectedPath = pathPieces[index];							//Sets the selectedPath piece based on the index value
	
	}
	
	
	//RANDOM FROM ARRAY --- Randomly pick a pathPiece from the array
	else if (pathStyle == pathStyle.useRandomFromArray) {
	
		selectedPath = pathPieces[Random.Range(0, pathPieces.Length)];						//Picks a pathPiece from the pathPieces array
		
	}
	
	
	//ARRAY ORDER --- Generate path pieces in the order they appear in the pathPieces array.  The order will cycle back to the beginning once it reaches the end
	else if (pathStyle == pathStyle.useArrayOrder) {
		
		if (y >= pathPieces.Length) {
			y = 0;
		}
		else {
			selectedPath = pathPieces[y];
			y++;
		}	
	}
	
	var newRoad = Instantiate (selectedPath, pathSpawnPoint.transform.position, pathSpawnPoint.transform.rotation);

}