using UnityEngine;
using System.Collections;

/// <summary>
/// ISMath - The SPINACH Mathematical Library
/// 
/// This is a subset of The SPINACH Mathematical Library provided by SPINACH for the usage of ISRTS Camera 
/// 
/// 
/// © 2013 - 2016 SPINACH All rights reserved.
/// </summary>

public class ISMath {

	static public float Clamp(float v, ISRange range){
		return Mathf.Clamp (v, range.min, range.max);
	}

	static public float MapToZeroAndOne(float min, float max, float value){
		return value / (max - min);
	}

	static public float Map(float v, ISRange valueRange, ISRange targetedRange){
		v = ISMath.Clamp (v, valueRange);
		float f = targetedRange.length / valueRange.length;
		return targetedRange.min + f * v;
	}

	static public float Random(ISRange range){
		return UnityEngine.Random.Range (range.min, range.max);
	}

	static public float WrapAngle (float a){
		while (a < -180.0f) a += 360.0f;
		while (a > 180.0f) a -= 360.0f;
		return a;
	}

	public enum ISMath_BoundCompareResult{Contains, Larger, Lower}
	static public ISMath_BoundCompareResult BoundCompare(float value, float a, float b){
		if(value > b) return ISMath_BoundCompareResult.Larger;
		else if(value < a) return ISMath_BoundCompareResult.Lower;

		return ISMath_BoundCompareResult.Contains;
	}
}

[System.Serializable]
public class ISRange{
	public float min,max;
	public float sum {get{ return min + max; }}
	public float length {get{ return max - min; }}

	public ISRange(float min, float max){
		this.min = min;
		this.max = max;
	}
}
