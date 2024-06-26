using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target; 

    private Vector3 offset = Vector3.up;

    private float speed = 5;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed * (target.position - transform.position).magnitude);
        transform.position = smoothedPosition;

    }
}
