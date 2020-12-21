//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-12-21 16:57:07
//Description: 
//=========================================

using UnityEngine;

[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{
	[SerializeField]
	private Enemy Prefab;

	public Enemy Get()
	{
		var instance = CreateGameObjectInstance(Prefab);
		instance.OriginFactory = this;
		return instance;
	}

	public void Reclaim(Enemy enemy)
	{
		Debug.Assert(enemy.OriginFactory == this, "Wrong factory reclaimed!");
		Destroy(enemy.gameObject);
	}
}