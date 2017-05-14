using UnityEngine;
using System.Collections;

/// <summary>
/// Addons of math functions
/// </summary>
public static class MathAddons
{
	public const float TWO_PI = Mathf.PI * 2;
	public const float HALF_PI = Mathf.PI * 0.5f;
	
    /// <summary>
    /// Is value in range min to max
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static bool IsInRange(float value, float min, float max) { return (value >= min) && (value <= max); }
    /// <summary>
    /// Is value in range min to max
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static bool IsInRange(int value, int min, int max) { return (value >= min) && (value <= max); }
}
