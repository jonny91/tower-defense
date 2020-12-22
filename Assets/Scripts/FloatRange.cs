//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-12-22 15:24:40
//Description: 
//=========================================

using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct FloatRange
{
	[SerializeField]
	private float _min, _max;

	public float Max => _max;

	public float Min => _min;

	public float RandomValueInRange => Random.Range(_min, _max);

	public FloatRange(float value)
	{
		_max = _min = value;
	}

	public FloatRange(float min, float max)
	{
		this._min = min;
		this._max = max < min ? min : max;
	}
}