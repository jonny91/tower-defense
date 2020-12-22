//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-12-22 16:50:47
//Description: 
//=========================================

using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TargetPoint : MonoBehaviour
{
	public Enemy Enemy { get; private set; }

	public Vector3 Position => transform.position;

	private void Awake()
	{
		Debug.Assert(gameObject.layer == 9, "Target point on wrong layer!", this);

		Enemy = transform.root.GetComponent<Enemy>();
		Debug.Assert(Enemy != null, "Target point without Enemy root!", this);
		Debug.Assert(GetComponent<SphereCollider>() != null, "Target point without SphereCollider!", this);
	}
}