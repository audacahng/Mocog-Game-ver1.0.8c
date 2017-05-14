
function OnTriggerEnter (other : Collider) {

	if (other.gameObject.tag == ("collider")) {
		pathiGenDemo.activeTileCount--;
	}
	Debug.Log(other.name);
	Destroy (other.transform.parent.gameObject);

}