using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
	[SerializeField]
	private Vector2Int BoardSize = new Vector2Int(11, 11);

	[SerializeField]
	private GameBoard Board = default;

	[SerializeField]
	private GameTileContentFactory TileContentFactory;

	[SerializeField]
	private EnemyFactory EnemyFactory;

	[SerializeField, Range(0.1F, 10F)]
	private float SpawnSpeed = 1f;

	private EnemyCollection _enemies = new EnemyCollection();

	public Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

	private float _spawnProgress;

	private void Awake()
	{
		Board.Initialize(BoardSize, TileContentFactory);
		Board.ShowGrid = true;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			HandleTouch();
		}
		else if (Input.GetMouseButtonDown(1))
		{
			HandleAlternativeTouch();
		}
		else if (Input.GetKeyDown(KeyCode.V))
		{
			Board.ShowPaths = !Board.ShowPaths;
		}
		else if (Input.GetKeyDown(KeyCode.G))
		{
			Board.ShowGrid = !Board.ShowGrid;
		}

		_spawnProgress += SpawnSpeed * Time.deltaTime;
		while (_spawnProgress >= 1f)
		{
			_spawnProgress -= 1f;
			SpawnEnemy();
		}

		_enemies.GameUpdate();
		Physics.SyncTransforms();
		Board.GameUpdate();
	}

	private void SpawnEnemy()
	{
		var spawnPoint = Board.GetSpawnPoint(Random.Range(0, Board.SpawnPointCount));
		var enemy = EnemyFactory.Get();
		enemy.SpawnOn(spawnPoint);

		_enemies.Add(enemy);
	}

	private void HandleAlternativeTouch()
	{
		GameTile tile = Board.GetTile(TouchRay);
		if (tile != null)
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				Board.ToggleDestination(tile);
			}
			else
			{
				Board.ToggleSpawnPoint(tile);
			}
		}
	}

	private void HandleTouch()
	{
		var tile = Board.GetTile(TouchRay);
		if (tile != null)
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				Board.ToggleTower(tile);
			}
			else
			{
				Board.ToggleWall(tile);
			}
		}
	}

	private void OnValidate()
	{
		if (BoardSize.x < 2)
		{
			BoardSize.x = 2;
		}

		if (BoardSize.y < 2)
		{
			BoardSize.y = 2;
		}
	}
}