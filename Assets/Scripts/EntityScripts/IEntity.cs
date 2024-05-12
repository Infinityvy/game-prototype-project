using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEntity
{
    public float getHealth();
    public void dealDamage(float damage);
}
