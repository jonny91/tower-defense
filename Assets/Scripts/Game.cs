using System;
using UnityEngine;

public class Game : MonoBehaviour
{
	[SerializeField]
	private Vector2Int BoardSize = new Vector2Int(11, 11);

	[SerializeField]
	private GameBoard Board = default;

	private void Awake()
	{
		Board.Initialize(BoardSize);
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