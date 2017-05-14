
enum Direction { translateX, translateY, translateZ };
var direction : Direction;

var negativeDirection : boolean;

function Update () {

	switch(direction){
		case Direction.translateX:
			if (negativeDirection == true) {
				transform.position.x -= gameData.levelSpeed;
			}
		else {
			transform.position.x += gameData.levelSpeed;
		}			
		break;
		
		case Direction.translateY:
			if (negativeDirection == true) {
				transform.position.y -= gameData.levelSpeed;
			}
		else {
			transform.position.y += gameData.levelSpeed;
		}			
		break;
		
		case Direction.translateZ:
			if (negativeDirection == true) {
				transform.position.z -= gameData.levelSpeed;
			}
		else {
			transform.position.z += gameData.levelSpeed;
		}			
		break;
	}
		

}