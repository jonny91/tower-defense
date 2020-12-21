//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-12-21 16:57:56
//Description: 
//=========================================

using System;
using UnityEngine;

public class
	Enemy : MonoBehaviour
{
	private GameTile _tileFrom, _tileTo;
	private Vector3 _positionFrom, _positionTo;
	private float _progress;
	private EnemyFactory _originFactory;
	private Direction _direction;
	private DirectionChange _directionChange;
	private float _directionAngleFrom, _directionAngleTo;

	public EnemyFactory OriginFactory
	{
		get => _originFactory;
		set
		{
			Debug.Assert(_originFactory == null, "Redefined origin factory!");
			_originFactory = value;
		}
	}

	public void SpawnOn(GameTile tile)
	{
		Debug.Assert(tile.NextTileOnPath != null, "Nowhere to go!", this);
		_tileFrom = tile;
		_tileTo = tile.NextTileOnPath;
//		_positionFrom = _tileFrom.transform.localPosition;
//		_positionTo = _tileFrom.ExitPoint;
//		transform.localRotation = _tileFrom.PathDirection.GetRotation();
		PrepareInfo();
		_progress = 0;
	}

	private void PrepareInfo()
	{
		_positionFrom = _tileFrom.transform.localPosition;
		_positionTo = _tileFrom.ExitPoint;
		_direction = _tileFrom.PathDirection;
		_directionChange = DirectionChange.None;
		_directionAngleFrom = _directionAngleTo = _direction.GetAngle();
		transform.localRotation = _tileFrom.PathDirection.GetRotation();
	}

	private void PrepareNextState()
	{
		_positionFrom = _positionTo;
		_positionTo = _tileFrom.ExitPoint;
		_directionChange = _direction.GetDirectionChangeTo(_tileFrom.PathDirection);
		_direction = _tileFrom.PathDirection;
		transform.localRotation = _tileFrom.PathDirection.GetRotation();
		_directionAngleFrom = _directionAngleTo;

		switch (_directionChange)
		{
			case DirectionChange.None:
				PrepareForward();
				break;
			case DirectionChange.TurnRight:
				PrepareTurnRight();
				break;
			case DirectionChange.TurnLeft:
				PrepareTurnLeft();
				break;
			default:
				PrepareTurnAround();
				break;
		}
	}

	private void PrepareForward()
	{
		transform.localRotation = _direction.GetRotation();
		_directionAngleTo = _direction.GetAngle();
	}

	private void PrepareTurnRight()
	{
		_directionAngleTo = _directionAngleFrom + 90f;
	}

	private void PrepareTurnLeft()
	{
		_directionAngleTo = _directionAngleFrom - 90f;
	}

	private void PrepareTurnAround()
	{
		_directionAngleTo = _directionAngleFrom + 180f;
	}

	public bool GameUpdate()
	{
		_progress += Time.deltaTime;
		while (_progress >= 1f)
		{
			_tileFrom = _tileTo;
			_tileTo = _tileTo.NextTileOnPath;
			if (_tileTo == null)
			{
				OriginFactory.Reclaim(this);
				return false;
			}

			_progress -= 1f;
			PrepareNextState();
		}

		transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);
		if (_directionChange != DirectionChange.None)
		{
			float angle = Mathf.LerpUnclamped(
				_directionAngleFrom, _directionAngleTo, _progress);
			transform.localRotation = Quaternion.Euler(0, angle, 0);
		}

		return true;
	}
}