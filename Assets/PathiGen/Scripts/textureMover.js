// /-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\
//
// 							TextureMover 1.0, Copyright © 2012, RipCord Development
//
// \-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/

//ABOUT - This script animates the texture Y offset of the specified sub material.  It is used to animate the water texture in the RiverBank prefabs included in PathiGen.


var movementSpeed : float = 1;

function Update () {

	GetComponent.<Renderer>().materials[1].mainTextureOffset.y = Time.time * movementSpeed;

}