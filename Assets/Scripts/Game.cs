using System;
using UnityEngine;

public class Game : MonoBehaviour
{
	[SerializeField]
	private Vector2Int BoardSize = new Vector2Int(11, 11);

	[SerializeField]
	private GameBoard Board = default;

	[SerializeField]
	private GameTileContentFactory TileContentFactory;

	public Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

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
	}

	private void HandleAlternativeTouch()
	{
		GameTile tile = Board.GetTile(TouchRay);
		if (tile != null)
		{
//			tile.Content = TileContentFactory.Get(GameTileContentType.Destination);
			Board.ToggleDestination(tile);
		}
	}

	private void HandleTouch()
	{
		var tile = Board.GetTile(TouchRay);
		if (tile != null)
		{
			Board.ToggleWall(tile);
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