using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    public Text text;

    private int enemyCount = 0;

    void Update()
    {
        if(enemyCount != EnemyDirector.instance.enemies.Count)
        {
            enemyCount = EnemyDirector.instance.enemies.Count;
            text.text = enemyCount.ToString();
        }
    }
}
