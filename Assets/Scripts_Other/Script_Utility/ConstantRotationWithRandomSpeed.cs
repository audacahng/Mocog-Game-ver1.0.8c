using UnityEngine;
using System.Collections;

public class ConstantRotationWithRandomSpeed : ConstantRotation
{
	public float rotationSpeedVaration = 30;
	public bool inverseSpeedRandomly = false;
	// Use this for initialization
	void Start ()
	{
		rotationSpeed += Random.Range (-rotationSpeedVaration, rotationSpeedVaration);
		if (inverseSpeedRandomly) {
			if (Random.value >= 0.5f)
				rotationSpeed = -rotationSpeed;
		}
	}
}
