using UnityEngine;

public class GameTileContent : MonoBehaviour
{
	[SerializeField]
	public GameTileContentType Type;

	private GameTileContentFactory _originFactory;

	public GameTileContentFactory OriginFactory
	{
		get => _originFactory;
		set
		{
			Debug.Assert(_originFactory == null, "Redefined origin factory!");
			_originFactory = value;
		}
	}

	public void Recycle()
	{
		_originFactory.Reclaim(this);
	}
}