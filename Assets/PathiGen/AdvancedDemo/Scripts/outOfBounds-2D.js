
function FixedUpdate () {

	if (transform.position.x > 3.0) {
		pathiGenDemo.activeTileCount--;
		Destroy (gameObject);
	}
}