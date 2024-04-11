using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public float getHealth();
    public void dealDamage(float damage);
}
