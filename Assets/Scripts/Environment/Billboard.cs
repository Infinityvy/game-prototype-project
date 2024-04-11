using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Billboard : MonoBehaviour
{
    public bool3 freezeRotation;
    private Vector3 billboardNormal = new Vector3(0, 0, -1);
    public Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        //cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newRotation = Vector3.zero;
        Vector3 vectorToCamera = cam.position - transform.position;

        if (!freezeRotation.x)
        {
            newRotation.x = Vector3.Angle(billboardNormal, new Vector3(vectorToCamera.x, 0, 0));
        }

        if (!freezeRotation.y)
        {
            newRotation.y = Vector3.Angle(billboardNormal, new Vector3(0, vectorToCamera.y, 0));
        }

        if (!freezeRotation.z)
        {
            newRotation.z = Vector3.Angle(billboardNormal, new Vector3(0, 0, vectorToCamera.z));
        }

        transform.rotation = Quaternion.Euler(newRotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, cam.position);
    }
}
