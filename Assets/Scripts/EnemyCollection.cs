//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-12-21 17:15:00
//Description: 
//=========================================

using System;
using System.Collections.Generic;

[Serializable]
public class EnemyCollection
{
	private List<Enemy> _enemies = new List<Enemy>();

	public void Add(Enemy enemy)
	{
		_enemies.Add(enemy);
	}

	public void GameUpdate()
	{
		for (int i = 0; i < _enemies.Count; i++)
		{
			if (!_enemies[i].GameUpdate())
			{
				var lastIndex = _enemies.Count - 1;
				_enemies[i] = _enemies[lastIndex];
				_enemies.RemoveAt(lastIndex);
				i--;
			}
		}
	}
}