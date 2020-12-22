//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-12-22 15:28:33
//Description: 
//=========================================

using UnityEngine;

public class FloatRangeSliderAttribute : PropertyAttribute
{
	public float Min { get; private set; }

	public float Max { get; private set; }

	public FloatRangeSliderAttribute(float min, float max)
	{
		Min = min;
		Max = max < min ? min : max;
	}
}