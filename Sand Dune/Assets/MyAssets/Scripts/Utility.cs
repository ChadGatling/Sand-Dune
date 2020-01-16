using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Custom utility functions.
/// </summary>
public class Utility {
	/// <summary>Computes and returns the value of a given ratio on a given curve.</summary>
	/// <param name ="curve">The curve on which the ratio will be computed.</param>
	/// <param name="ratio">The value to compute on the curve. Usually a float between 0 and 1.</param>
	/// <returns>Returns the Y-value of the curve at the given X-value as a float.</returns>
	public static float EvaluateCurve(AnimationCurve curve, float ratio) {
		return curve.Evaluate(ratio);
	}
	
	/// <summary>Computes and returns the value of a given ratio on a given curve applied to a given maximum.</summary>
	/// <param name ="curve">The curve on which the ratio will be computed.</param>
	/// <param name="ratio">The value to compute on the curve. Usually a float between 0 and 1.</param>
	/// <param name="maximum">The maximum value that should be returned. Return value will be relative to maximum.</param>
	/// <returns>Returns the Y-value of the curve at the given X-value as a float multiplied by the constant maximum.</returns>
	public static float EvaluateCurve(AnimationCurve curve, float ratio, float maximum) {
		return curve.Evaluate(ratio) * maximum;
	}
}
