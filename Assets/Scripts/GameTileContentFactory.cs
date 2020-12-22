using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class GameTileContentFactory : GameObjectFactory
{
	[SerializeField]
	private GameTileContent DestinationPrefab;

	[SerializeField]
	private GameTileContent EmptyPrefab;

	[SerializeField]
	private GameTileContent WallPrefab;

	[SerializeField]
	private GameTileContent SpawnPointPrefab;

	[SerializeField]
	private Tower TowerPrefab;

	public void Reclaim(GameTileContent content)
	{
		Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
		Destroy(content.gameObject);
	}

	public GameTileContent Get(GameTileContent prefab)
	{
		GameTileContent instance = CreateGameObjectInstance(prefab);
		instance.OriginFactory = this;
		return instance;
	}

	public GameTileContent Get(GameTileContentType type)
	{
		switch (type)
		{
			case GameTileContentType.Empty:
				return Get(EmptyPrefab);
			case GameTileContentType.Destination:
				return Get(DestinationPrefab);
			case GameTileContentType.Wall:
				return Get(WallPrefab);
			case GameTileContentType.SpawnPoint:
				return Get(SpawnPointPrefab);
			case GameTileContentType.Tower:
				return Get(TowerPrefab);
		}

		Debug.Assert(false, "Unsupported type: " + type);
		return null;
	}
}