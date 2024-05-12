using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEnemy
{
    public Transform transform { get; }
    public Vector3 position {  get; }
    public Vector3 velocity { get; }

    public EnemyState getEnemyState();
    public UnityEvent deathEvent { get; }
    public static Transform getPrefab()
    {
        throw new NotImplementedException();
    }
}
