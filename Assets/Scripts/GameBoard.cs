using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
	[SerializeField]
	private Transform Ground = default;

	[SerializeField]
	private GameTile TilePrefab;

	[SerializeField]
	private Texture2D GrideTexture;

	private Vector2Int _size;

	private GameTile[] _tiles;

	private Queue<GameTile> _searchFrontier = new Queue<GameTile>();
	private GameTileContentFactory _gameTileContentFactory;

	private bool _showPaths, _showGrid;

	public void Initialize(Vector2Int size, GameTileContentFactory contentFactory)
	{
		this._size = size;
		this._gameTileContentFactory = contentFactory;
		Ground.localScale = new Vector3(size.x, size.y, 1f);
		_tiles = new GameTile[size.x * size.y];
		Vector2 offset = new Vector2(
			(size.x - 1) * 0.5f, (size.y - 1) * 0.5f
		);

		for (int i = 0, y = 0; y < size.y; y++)
		{
			for (int x = 0; x < size.x; x++, i++)
			{
				var tile = _tiles[i] = Instantiate(TilePrefab);

				tile.IsAlternative = (x & 1) == 0;
				if ((y & 1) == 0)
				{
					tile.IsAlternative = !tile.IsAlternative;
				}

				tile.transform.SetParent(transform, false);
				tile.transform.localPosition = new Vector3(
					x - offset.x, 0, y - offset.y);

				if (x > 0)
				{
					GameTile.MakeEastWestNeighbors(tile, _tiles[i - 1]);
				}

				if (y > 0)
				{
					GameTile.MakeNorthSouthNeighbors(tile, _tiles[i - size.x]);
				}

				tile.Content = contentFactory.Get(GameTileContentType.Empty);
			}
		}

//		FindPaths();
		ToggleDestination(_tiles[_tiles.Length / 2]);
	}

	private bool FindPaths()
	{
		foreach (var tile in _tiles)
		{
			if (tile.Content.Type == GameTileContentType.Destination)
			{
				tile.BecomeDestination();
				_searchFrontier.Enqueue(tile);
			}
			else
			{
				tile.ClearPath();
			}
		}


//		_tiles[0].BecomeDestination();
//		_searchFrontier.Enqueue(_tiles[0]);

//		_tiles[_tiles.Length / 2].BecomeDestination();
//		_searchFrontier.Enqueue(_tiles[_tiles.Length / 2]);

		if (_searchFrontier.Count == 0)
		{
			return false;
		}

		while (_searchFrontier.Count > 0)
		{
			var t = _searchFrontier.Dequeue();
			if (t != null)
			{
				if (t.IsAlternative)
				{
					_searchFrontier.Enqueue(t.GrowPathNorth());
					_searchFrontier.Enqueue(t.GrowPathSouth());
					_searchFrontier.Enqueue(t.GrowPathEast());
					_searchFrontier.Enqueue(t.GrowPathWest());
				}
				else
				{
					_searchFrontier.Enqueue(t.GrowPathWest());
					_searchFrontier.Enqueue(t.GrowPathEast());
					_searchFrontier.Enqueue(t.GrowPathSouth());
					_searchFrontier.Enqueue(t.GrowPathNorth());
				}
			}
		}

		foreach (var tile in _tiles)
		{
			if (!tile.HasPath)
			{
				return false;
			}
		}

		if (_showPaths)
		{
			foreach (var t in _tiles)
			{
				t.ShowPath();
			}
		}

		return true;
	}

	public void ToggleDestination(GameTile tile)
	{
		if (tile.Content.Type == GameTileContentType.Destination)
		{
			tile.Content = _gameTileContentFactory.Get(GameTileContentType.Empty);
			if (!FindPaths())
			{
				tile.Content = _gameTileContentFactory.Get(GameTileContentType.Destination);
				FindPaths();
			}
		}
		else
		{
			tile.Content = _gameTileContentFactory.Get(GameTileContentType.Destination);
			FindPaths();
		}
	}

	public void ToggleWall(GameTile tile)
	{
		if (tile.Content.Type == GameTileContentType.Wall)
		{
			tile.Content = _gameTileContentFactory.Get(GameTileContentType.Empty);
			FindPaths();
		}
		else
		{
			tile.Content = _gameTileContentFactory.Get(GameTileContentType.Wall);
			if (!FindPaths())
			{
				tile.Content = _gameTileContentFactory.Get(GameTileContentType.Empty);
				FindPaths();
			}
		}
	}

	public GameTile GetTile(Ray ray)
	{
		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			var x = (int) (hit.point.x + _size.x * 0.5f);
			var y = (int) (hit.point.z + _size.y * 0.5f);
			if (x >= 0 && x < _size.x && y >= 0 && y < _size.y)
			{
				return _tiles[x + y * _size.x];
			}
		}

		return null;
	}

	public bool ShowPaths
	{
		get => _showPaths;
		set
		{
			_showPaths = value;

			foreach (var tile in _tiles)
			{
				if (_showPaths)
				{
					tile.ShowPath();
				}
				else
				{
					tile.HidePath();
				}
			}
		}
	}

	public bool ShowGrid
	{
		get => _showGrid;
		set
		{
			_showGrid = value;
			var m = Ground.GetComponent<MeshRenderer>().material;
			if (ShowGrid)
			{
				m.mainTexture = GrideTexture;
				m.SetTextureScale("_MainTex", _size);
			}
			else
			{
				m.mainTexture = null;
			}
		}
	}
}