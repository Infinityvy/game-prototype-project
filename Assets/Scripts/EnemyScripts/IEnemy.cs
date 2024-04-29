using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public Vector3 position {  get; }
    public EnemyState getEnemyState();
    public float getHealth();
    public void dealDamage(float damage);

    public static Transform getPrefab()
    {
        throw new NotImplementedException();
    }
}
