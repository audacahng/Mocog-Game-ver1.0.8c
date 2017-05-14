
private var decelTimePassed : float = 0.0;

function OnTriggerEnter (thing : Collider) {

	if(thing.tag == "finishLine") {
		gameData.finishLineCrossed = true;
	}
}