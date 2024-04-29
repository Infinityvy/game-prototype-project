using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 6f;
    public LayerMask mask;
    public float aliveTimeInSeconds = 6f;

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

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if(Physics.Raycast(transform.position, lastPosition-transform.position, Vector3.Distance(transform.position, lastPosition), mask))
        {
            initiateDestroy();
        }

        if(Time.time - timeAtSpawn > aliveTimeInSeconds)
        {
            initiateDestroy();
        }

        lastPosition = transform.position;
    }

    private void initiateDestroy()
    {
        destroyed = true;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        Invoke("destroy", 1);
    }

    private void destroy()
    {
        GameObject.Destroy(gameObject);
    }
}
