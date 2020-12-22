using UnityEngine;

public class GameTile : MonoBehaviour
{
	[SerializeField]
	private Transform Arrow;

	private GameTile _north, _east, _south, _west, _nextOnPath;
	private int _distance;

	public bool IsAlternative { get; set; }

	public Direction PathDirection { get; set; }

	public Vector3 ExitPoint { get; set; }

	public bool HasPath => _distance != int.MaxValue;

	private static Quaternion
		northRotation = Quaternion.Euler(90, 0, 0),
		eastRotation = Quaternion.Euler(90, 90, 0),
		southRotation = Quaternion.Euler(90, 180, 0),
		westRotation = Quaternion.Euler(90, 270, 0);

	private GameTileContent _content;

	public GameTileContent Content
	{
		get => _content;
		set
		{
			Debug.Assert(value != null, "Null assigned to content!");
			if (_content != null)
			{
				_content.Recycle();
			}

			_content = value;
			_content.transform.localPosition = transform.localPosition;
		}
	}

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

	private GameTile GrowPathTo(GameTile neighbor, Direction direction)
	{
		Debug.Assert(this.HasPath, "No Path!");
		if (!HasPath || neighbor == null || neighbor.HasPath)
		{
			return null;
		}

		neighbor._distance = _distance + 1;
		neighbor._nextOnPath = this;

		neighbor.ExitPoint = neighbor.transform.localPosition + direction.GetHalfVector();
		neighbor.PathDirection = direction;
		return neighbor.Content.BlocksPath ? null : neighbor;
	}

	public void ShowPath()
	{
		if (_distance == 0)
		{
			Arrow.gameObject.SetActive(false);
			return;
		}

		Arrow.gameObject.SetActive(true);
		Arrow.localRotation =
			_nextOnPath == _north ? northRotation :
			_nextOnPath == _east ? eastRotation :
			_nextOnPath == _south ? southRotation :
			westRotation;
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
		ExitPoint = transform.localPosition;
	}

	public void HidePath()
	{
		Arrow.gameObject.SetActive(false);
	}

	public GameTile NextTileOnPath => _nextOnPath;

	public GameTile GrowPathNorth() => GrowPathTo(_north, Direction.South);
	public GameTile GrowPathEast() => GrowPathTo(_east, Direction.West);
	public GameTile GrowPathSouth() => GrowPathTo(_south, Direction.North);
	public GameTile GrowPathWest() => GrowPathTo(_west, Direction.East);
}