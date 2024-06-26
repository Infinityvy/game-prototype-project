using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEntity : MonoBehaviour, IEntity
{
    public static PlayerEntity instance;

    public GameObject GameOverPanel;

    [HideInInspector]
    public bool isDead = false;
    [HideInInspector]
    public PlayerMovement movement;

    private float maxHealth = 100;
    private float currentHealth;
    private float healthRegenPerSecond = 0.5f;
    private bool isInvincible = false;

    private Material modelMaterial;


    private Sound[] hurtSounds;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        movement = GetComponent<PlayerMovement>();

        modelMaterial = transform.Find("Model").GetComponent<MeshRenderer>().material;

        hurtSounds = GameUtility.loadSounds("Playerhurt", VolumeManager.playerHurtBaseVolume, 1.5f);
        gameObject.createAudioSources(hurtSounds);
        VolumeManager.addEffects(hurtSounds);

        currentHealth = maxHealth;
    }

    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth + healthRegenPerSecond * Time.deltaTime, 0, maxHealth);
    }

    public void dealDamage(float damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        //hurtSounds.playRandom();
        AkSoundEngine.PostEvent("player_hurt", gameObject);

        modelMaterial.color = Color.red;
        Invoke(nameof(resetModelColor), 0.2f);

        isInvincible = true;
        Invoke(nameof(resetInvincibility), 0.2f);

        if (currentHealth <= 0) die();
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public float getHealth()
    {
        return currentHealth;
    }

    private void die()
    {
        isDead = true;

        movement.GetComponent<Rigidbody>().velocity = Vector3.zero;

        Time.timeScale = 0.2f;

        AkSoundEngine.PostEvent("player_death", gameObject);

        transform.GetComponent<CapsuleCollider>().enabled = false;
        transform.GetComponentInChildren<MeshRenderer>().enabled = false;
        transform.GetComponentInChildren<ParticleSystem>().Play();
        GameOverPanel.SetActive(true);
        ScoreController.updateHighscore();
    }

    private void resetModelColor()
    {
        modelMaterial.color = Color.white;
    }

    private void resetInvincibility()
    {
        isInvincible = false;
    }
}
