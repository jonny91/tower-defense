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

	[SerializeField, FloatRangeSlider(0.5f, 2f)]
	private FloatRange Scale = new FloatRange(1f);

	[SerializeField, FloatRangeSlider(-0.4f, 0.4f)]
	private FloatRange PathOffset = new FloatRange(0f);

	[SerializeField, FloatRangeSlider(0.2f, 5f)]
	private FloatRange Speed = new FloatRange(1f);

	public Enemy Get()
	{
		var instance = CreateGameObjectInstance(Prefab);
		instance.OriginFactory = this;
		instance.Initialize(Scale.RandomValueInRange, Speed.RandomValueInRange, PathOffset.RandomValueInRange);
		return instance;
	}

	public void Reclaim(Enemy enemy)
	{
		Debug.Assert(enemy.OriginFactory == this, "Wrong factory reclaimed!");
		Destroy(enemy.gameObject);
	}
}