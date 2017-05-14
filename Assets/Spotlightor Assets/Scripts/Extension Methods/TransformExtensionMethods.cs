using UnityEngine;
using System.Collections;

public static class TransformExtensionMethods
{

	public static void SetPositionX (this Transform transform, float x)
	{
		transform.position = new Vector3 (x, transform.position.y, transform.position.z);
	}

	public static void SetPositionY (this Transform transform, float y)
	{
		transform.position = new Vector3 (transform.position.x, y, transform.position.z);
	}

	public static void SetPositionZ (this Transform transform, float z)
	{
		transform.position = new Vector3 (transform.position.x, transform.position.y, z);
	}
	
	public static void SetUniformLocalScale (this Transform transform, float uniformScale)
	{
		transform.localScale = Vector3.one * uniformScale;
	}
}
