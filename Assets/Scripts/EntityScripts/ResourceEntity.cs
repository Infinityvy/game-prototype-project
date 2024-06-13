using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ResourceEntity : MonoBehaviour, IEntity
{
    public ResourceBlock contents;

    bool prefabsFetched = false;
    private Transform plankPrefab;
    private Transform ingotPrefab;

    private bool atRest = false;

    public static ResourceEntity create(ResourceBlock contents, Vector3 pos)
    {
        GameObject gameObject = new GameObject("ResourceEntity");
        gameObject.transform.position = pos;

        ResourceEntity entity = gameObject.AddComponent<ResourceEntity>();
        entity.contents = contents;
        entity.updateModel();

        gameObject.layer = LayerMask.NameToLayer("Resources");
        gameObject.AddComponent<SphereCollider>().radius = 0.5f;
        gameObject.GetComponent<SphereCollider>().isTrigger = true;

        return entity;
    }

    private void Update()
    {
        if (atRest) return;

        if (transform.position.y > 0.35f)
        {
            transform.Translate(0, -9.81f * Time.deltaTime, 0);
        }
        else 
        {
            atRest = true;
            mergeNearbyResources();
        }
    }

    public void updateModel()
    {
        if(!prefabsFetched) fetchPrefabs();

        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        float woodRotationOffset = Random.Range(0f, 360f);
        for (int i = 0; i < contents.wood && i < 4; i++) 
        {
            Instantiate(plankPrefab, transform.position, Quaternion.Euler(0, woodRotationOffset + 45 * i, 0), transform);
        }

        float metalRotationOffset = Random.Range(0f, 360f);
        for (int i = 0; i < contents.metal && i < 4; i++)
        {
            Instantiate(ingotPrefab, transform.position, Quaternion.Euler(0, metalRotationOffset + 45 * i, 0), transform);
        }
    }

    public void dealDamage(float damage)
    {
        // do nothing since invulnerable
    }

    public float getHealth()
    {
        // no actual health
        return 0;
    }

    private void fetchPrefabs()
    {
        plankPrefab = Resources.Load<Transform>("Plank");
        ingotPrefab = Resources.Load<Transform>("Ingot");
    }

    private void mergeNearbyResources()
    {
        GetComponent<SphereCollider>().enabled = false;

        Collider[] hits = Physics.OverlapSphere(transform.position, 2f, LayerMask.GetMask("Resources"));
        GetComponent<SphereCollider>().enabled = true;

        if (hits.Length == 0) return;

        Vector3 meanPosition = transform.position;
        int hitCount = hits.Length;

        for (int i = 0;i < hitCount;i++)
        {
            meanPosition += hits[i].transform.position;

            contents += hits[i].transform.GetComponent<ResourceEntity>().contents;

            GameObject.Destroy(hits[i].transform.gameObject);
        }

        meanPosition /= hitCount + 1;

        transform.position = meanPosition;
        updateModel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        AkSoundEngine.PostEvent("player_pickup", other.gameObject);

        PlayerBuildModeState.resourceInventory.addResources(contents);
        GetComponent<SphereCollider>().enabled = false;
        Destroy(gameObject);
    }
}
