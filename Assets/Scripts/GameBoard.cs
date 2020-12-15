using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
	[SerializeField]
	private Transform Ground = default;

	[SerializeField]
	private GameTile TilePrefab;

	private Vector2Int _size;

	private GameTile[] _tiles;

	public void Initialize(Vector2Int size)
	{
		this._size = size;
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
				tile.transform.SetParent(transform, false);
				tile.transform.localPosition = new Vector3(
					x - offset.x, 0, y - offset.y);
			}
		}
	}
}