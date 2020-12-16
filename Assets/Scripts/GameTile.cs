using UnityEngine;

public class GameTile : MonoBehaviour
{
	[SerializeField]
	private Transform Arrow;

	private GameTile _north, _east, _south, _west, _nextOnPath;
	private int _distance;

	public bool HasPath => _distance != int.MaxValue;

	/// <summary>
	/// 如果一个瓦片是第二个瓦片的东邻，则第二个瓦片是第一个瓦片的西邻
	/// </summary>
	/// <param name="east"></param>
	/// <param name="west"></param>
	public static void MakeEastWestNeighbors(GameTile east, GameTile west)
	{
		Debug.Assert(west._east == null && east._west == null, "Redefined neighbors!");
		west._east = east;
		east._west = west;
	}

	public static void MakeNorthSouthNeighbors(GameTile north, GameTile south)
	{
		Debug.Assert(north._south == null && south._north == null, "Redefined neighbors!");
		north._south = south;
		south._north = north;
	}

	private GameTile GrowPathTo(GameTile neighbor)
	{
		Debug.Assert(this.HasPath, "No Path !");
		if (!HasPath || neighbor == null || neighbor.HasPath)
		{
			return null;
		}

		neighbor._distance = _distance + 1;
		neighbor._nextOnPath = this;

		return neighbor;
	}

	public void ClearPath()
	{
		_distance = int.MaxValue;
		_nextOnPath = null;
	}

	public void BecomeDestination()
	{
		_distance = 0;
		_nextOnPath = null;
	}

	public GameTile GrowPathNorth() => GrowPathTo(_north);
	public GameTile GrowPathEast() => GrowPathTo(_east);
	public GameTile GrowPathSouth() => GrowPathTo(_south);
	public GameTile GrowPathWest() => GrowPathTo(_west);
}