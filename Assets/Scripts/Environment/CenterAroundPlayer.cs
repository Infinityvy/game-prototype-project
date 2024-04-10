using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterAroundPlayer : MonoBehaviour
{
    // public:
    public Transform player;

    void Update()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
    }
}
