// /-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\
//
// 							PathiGen 1.0, Copyright © 2012, RipCord Development
//											demo-loadScene.js
//
// \-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/

//ABOUT - This script is meant to demonstrate the random nature of PathiGen in the demo scene.  This is not the PathiGen script.


private var offState : Color;  //Used to store the initial material colour
var overState : Color;  //Defines the colour the button changes to when the cursor hovers over it
var clickState : Color;  //Defines the colour the button changes to when it is clicked on

private var buttonState : String;  //Stores the current state of the button
var fadeTime : float = 0.25;  //The amount of time it takes for the button colour to fade from one state to the next

var sceneNumber : int;

function Awake() {

	//Defines the offState colour of the button as the colour it is when the scene starts
	offState = GetComponent.<Renderer>().material.color;
	
}

// BUTTON STATES --------------------

function OnMouseEnter() {

	buttonState = "Over";
	ColourChange();
	
}

function OnMouseExit() {

	buttonState = "Off";
	ColourChange();
	
}

function OnMouseDown() {

	buttonState = "Down";
	GetComponent.<Renderer>().material.color = clickState;

}

// BUTTON ACTION --------------------

function OnMouseUp() {

	buttonState = "Clicked";
	GetComponent.<Renderer>().material.color = overState;
	
	Application.LoadLevel (sceneNumber);
	
}

function ColourChange()	{

	var currentColor = GetComponent.<Renderer>().material.color;
	var timeLeft = fadeTime;
	
	while (timeLeft > 0) {

		if (buttonState == "Over") {
			GetComponent.<Renderer>().material.color = Color.Lerp(currentColor, overState, (fadeTime - timeLeft) / fadeTime);
		}

		if (buttonState == "Off") {
			GetComponent.<Renderer>().material.color = Color.Lerp(currentColor, offState, (fadeTime - timeLeft) / fadeTime);
		}

		yield;
		timeLeft -= Time.deltaTime;
	}
}
	