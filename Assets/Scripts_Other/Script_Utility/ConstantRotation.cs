using UnityEngine;
using System.Collections;

public class ConstantRotation : MonoBehaviour
{
	public float rotationSpeed = 30;
	public Vector3 axis = Vector3.up;

	// Update is called once per frame
	void Update ()
	{
		transform.Rotate (axis, rotationSpeed * Time.deltaTime);
	}
}

