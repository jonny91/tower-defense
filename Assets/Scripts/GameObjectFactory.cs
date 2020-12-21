//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-12-21 16:52:22
//Description: 
//=========================================

using UnityEngine;
using UnityEngine.SceneManagement;
using Application = UnityEngine.Application;

public abstract class GameObjectFactory : ScriptableObject
{
	private Scene _scene;

	protected T CreateGameObjectInstance<T>(T prefab) where T : MonoBehaviour
	{
		if (!_scene.isLoaded)
		{
			if (Application.isEditor)
			{
				_scene = SceneManager.GetSceneByName(name);
				if (!_scene.isLoaded)
				{
					_scene = SceneManager.CreateScene(name);
				}
			}
			else
			{
				_scene = SceneManager.CreateScene(name);
			}
		}

		T instance = Instantiate(prefab);
		SceneManager.MoveGameObjectToScene(instance.gameObject, _scene);
		return instance;
	}
}