//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-12-22 16:23:32
//Description: 
//=========================================

using System;
using UnityEngine;

public class Tower : GameTileContent
{
	[SerializeField, Range(1.5f, 10.5f)]
	private float TargetingRange = 1.5f;

	private TargetPoint _target;

	private const int _enemyLayerMask = 1 << 9;

	public override void GameUpdate()
	{
		if (TrackTarget() || AcquireTarget())
		{
			Debug.Log("Locked on target!");
		}
	}

	private bool AcquireTarget()
	{
		Collider[] targets = Physics.OverlapSphere(
			transform.localPosition, TargetingRange, _enemyLayerMask);

		if (targets.Length > 0)
		{
			_target = targets[0].GetComponent<TargetPoint>();
			Debug.Assert(_target != null, "Targeted none-target!", targets[0]);
			return true;
		}

		_target = null;
		return false;
	}

	private bool TrackTarget()
	{
		if (_target == null)
		{
			return false;
		}

		var a = transform.position;
		var b = _target.Position;
		//0.125 碰撞器的半径
		if (Vector3.Distance(a, b) > TargetingRange + 0.125f * _target.Enemy.Scale)
		{
			_target = null;
			return false;
		}

		return true;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		var position = transform.localPosition;
		position.y += 0.01f;
		Gizmos.DrawWireSphere(position, TargetingRange);

		Gizmos.color = Color.red;
		if (_target != null)
		{
			Gizmos.DrawLine(position, _target.Position);
		}
	}
}