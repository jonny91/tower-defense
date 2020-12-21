//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-12-21 23:27:36
//Description: 
//=========================================

using UnityEngine;

public enum Direction
{
	North,
	East,
	South,
	West,
}

public enum DirectionChange
{
	None,
	TurnRight,
	TurnLeft,
	TurnAround,
}

public static class DirectionExtensions
{
	private static Quaternion[] _rotations =
	{
		Quaternion.identity,
		Quaternion.Euler(0, 90, 0),
		Quaternion.Euler(0, 180, 0),
		Quaternion.Euler(0, 270, 0),
	};

	public static Quaternion GetRotation(this Direction dir)
	{
		return _rotations[(int) dir];
	}

	public static DirectionChange GetDirectionChangeTo(this Direction current, Direction next)
	{
		if (current == next)
		{
			return DirectionChange.None;
		}
		else if (current + 1 == next || current - 3 == next)
		{
			return DirectionChange.TurnRight;
		}
		else if (current - 1 == next || current + 3 == next)
		{
			return DirectionChange.TurnLeft;
		}

		return DirectionChange.TurnAround;
	}

	public static float GetAngle(this Direction dir)
	{
		return (float) dir * 90f;
	}
}