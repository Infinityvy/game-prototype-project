using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEntity : MonoBehaviour, IEntity
{
    public static PlayerEntity instance;

    [HideInInspector]
    public bool isDead = false;
    [HideInInspector]
    public PlayerMovement movement;

    private float maxHealth = 100;
    private float currentHealth;

    public GameObject GameOverPanel;


    void Start()
    {
        instance = this;
        movement = GetComponent<PlayerMovement>();

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
        isDead = true;
        transform.GetComponentInChildren<MeshRenderer>().enabled = false;
        transform.GetComponentInChildren<ParticleSystem>().Play();
        GameOverPanel.SetActive(true);
    }
}
