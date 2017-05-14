using UnityEngine;
using System.Collections;
using System;

public static class EnumHelper
{

	public static string EnumToString (Enum enumValue)
	{
		return System.Enum.GetName (enumValue.GetType (), enumValue);
	}
}
