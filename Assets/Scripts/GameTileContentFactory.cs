using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class GameTileContentFactory : ScriptableObject
{
	private Scene _contentScene;

	[SerializeField]
	private GameTileContent DestinationPrefab;

	[SerializeField]
	private GameTileContent EmptyPrefab;

	[SerializeField]
	private GameTileContent WallPrefab;

	public void Reclaim(GameTileContent content)
	{
		Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
		Destroy(content.gameObject);
	}

	public GameTileContent Get(GameTileContent prefab)
	{
		GameTileContent instance = Instantiate(prefab);
		instance.OriginFactory = this;
		MoveToFactoryScene(instance.gameObject);
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
		}

		Debug.Assert(false, "Unsupported type: " + type);
		return null;
	}

	private void MoveToFactoryScene(GameObject o)
	{
		if (!_contentScene.isLoaded)
		{
			if (Application.isEditor)
			{
				_contentScene = SceneManager.GetSceneByName(name);
				if (!_contentScene.isLoaded)
				{
					_contentScene = SceneManager.CreateScene(name);
				}
			}
			else
			{
				_contentScene = SceneManager.CreateScene(name);
			}
		}

		SceneManager.MoveGameObjectToScene(o, _contentScene);
	}
}