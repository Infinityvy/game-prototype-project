using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEntity : MonoBehaviour, IEntity
{
    private float maxHealth = 100;
    private float currentHealth;

    public CanvasGroup GameOverPanel;


    void Start()
    {
        currentHealth = maxHealth;
    }

    public void dealDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) die();
    }

    public float getHealth()
    {
        return currentHealth;
    }

    private void die()
    {
        Debug.Log("Player died.");
        Time.timeScale = 0f;
        GameOverPanel.alpha = 1;
        GameOverPanel.blocksRaycasts = true;
    }
}
