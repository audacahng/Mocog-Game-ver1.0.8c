using UnityEngine;
using System.Collections;
using System;

public static class TransformHelper{

	//usage var foo = transform.Search("Foo");

	public static Transform Search(this Transform target, string name)
	{
		if (target.name == name) return target;

		for (int i = 0; i < target.childCount; ++i)
		{
			var result = Search(target.GetChild(i), name);

			if (result != null) return result;
		}

		return null;
	}






}
