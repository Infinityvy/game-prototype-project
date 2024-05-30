using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 6f;
    public LayerMask mask;
    public float aliveTimeInSeconds = 6f;
    public float damage = 10f;

    private Vector3 lastPosition;
    private float timeAtSpawn;

    private bool destroyed = false;

    void Start()
    {
        timeAtSpawn = Time.time;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (destroyed) return;

        if (Time.time - timeAtSpawn > aliveTimeInSeconds)
        {
            initiateDestroy();
            return;
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Ray ray = new Ray(transform.position, lastPosition - transform.position);
        RaycastHit[] hits = Physics.RaycastAll(ray, Vector3.Distance(transform.position, lastPosition), mask);
        lastPosition = transform.position;

        if (hits.Length == 0) return;

        try
        {
            hits[0].transform.GetComponent<IEntity>().dealDamage(damage);
        }
        catch(ArgumentException e) 
        {
            Debug.LogError(e.Message);
        }

        initiateDestroy();
    }

    private void initiateDestroy()
    {
        destroyed = true;
        transform.Find("Model").gameObject.SetActive(false);
        transform.GetComponentInChildren<ParticleSystem>().Play();
        Invoke("destroy", 1);
    }

    private void destroy()
    {
        GameObject.Destroy(gameObject);
    }
}
