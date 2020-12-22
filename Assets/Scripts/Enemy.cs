//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-12-21 16:57:56
//Description: 
//=========================================

using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private GameTile _tileFrom, _tileTo;
	private Vector3 _positionFrom, _positionTo;
	private float _progress, _progressFactor;
	private EnemyFactory _originFactory;
	private Direction _direction;
	private DirectionChange _directionChange;
	private float _directionAngleFrom, _directionAngleTo;

	[SerializeField]
	private Transform Model;

	private float _pathOffset;
	private float _speed;

	public float Scale { get; private set; }

	public EnemyFactory OriginFactory
	{
		get => _originFactory;
		set
		{
			Debug.Assert(_originFactory == null, "Redefined origin factory!");
			_originFactory = value;
		}
	}

	public void Initialize(float scale, float speed, float pathOffset)
	{
		Scale = scale;
		Model.localScale = new Vector3(scale, scale, scale);
		this._speed = speed;
		this._pathOffset = pathOffset;
	}

	public void SpawnOn(GameTile tile)
	{
		Debug.Assert(tile.NextTileOnPath != null, "Nowhere to go!", this);
		_tileFrom = tile;
		_tileTo = tile.NextTileOnPath;
//		_positionFrom = _tileFrom.transform.localPosition;
//		_positionTo = _tileFrom.ExitPoint;
//		transform.localRotation = _tileFrom.PathDirection.GetRotation();
		PrepareIntro();
		_progress = 0;
	}

	private void PrepareIntro()
	{
		_positionFrom = _tileFrom.transform.localPosition;
		_positionTo = _tileFrom.ExitPoint;
		_direction = _tileFrom.PathDirection;
		_directionChange = DirectionChange.None;
		_directionAngleFrom = _directionAngleTo = _direction.GetAngle();
		Model.localPosition = new Vector3(_pathOffset, 0);
		transform.localRotation = _tileFrom.PathDirection.GetRotation();
		_progressFactor = 2f * _speed;
	}


	private void PrepareOutro()
	{
		_positionTo = _tileFrom.transform.localPosition;
		_directionChange = DirectionChange.None;
		_directionAngleTo = _direction.GetAngle();
		Model.localPosition = new Vector3(_pathOffset, 0);
		transform.localRotation = _direction.GetRotation();
		_progressFactor = 2f * _speed;
	}

	private void PrepareNextState()
	{
		_tileFrom = _tileTo;
		_tileTo = _tileTo.NextTileOnPath;
		_positionFrom = _positionTo;
		if (_tileTo == null)
		{
			PrepareOutro();
			return;
		}

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
		Model.localPosition = new Vector3(_pathOffset, 0);
		_progressFactor = _speed;
	}

	private void PrepareTurnRight()
	{
		_directionAngleTo = _directionAngleFrom + 90f;
		Model.localPosition = new Vector3(_pathOffset - 0.5f, 0);
		transform.localPosition = _positionFrom + _direction.GetHalfVector();
		_progressFactor = _speed / (Mathf.PI * 0.5f * (0.5f - _pathOffset));
	}

	private void PrepareTurnLeft()
	{
		_directionAngleTo = _directionAngleFrom - 90f;
		Model.localPosition = new Vector3(_pathOffset + 0.5f, 0);
		transform.localPosition = _positionFrom + _direction.GetHalfVector();
		_progressFactor = _speed / (Mathf.PI * 0.5f * (0.5f + _pathOffset));
	}

	private void PrepareTurnAround()
	{
		_directionAngleTo = _directionAngleFrom + (_pathOffset < 0f ? 180f : -180f);
		Model.localPosition = new Vector3(_pathOffset, 0);
		transform.localPosition = _positionFrom;
		_progressFactor = _speed / (Mathf.PI * Mathf.Max(Mathf.Abs(_pathOffset), 0.2f));
	}

	public bool GameUpdate()
	{
		_progress += Time.deltaTime * _progressFactor;
		while (_progress >= 1f)
		{
//			_tileFrom = _tileTo;
//			_tileTo = _tileTo.NextTileOnPath;

			if (_tileTo == null)
			{
				OriginFactory.Reclaim(this);
				return false;
			}

//			_progress -= 1f;
			_progress = (_progress - 1f) / _progressFactor;
			PrepareNextState();
			_progress *= _progressFactor;
		}

		if (_directionChange == DirectionChange.None)
		{
			transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);
		}
		else
		{
			float angle = Mathf.LerpUnclamped(
				_directionAngleFrom, _directionAngleTo, _progress);

			transform.localRotation = Quaternion.Euler(0, angle, 0);
		}

		return true;
	}
}