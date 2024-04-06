using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowTurret : MonoBehaviour, ITurret, IPlaceable
{
    public Transform crossbow;

    void Start()
    {
        
    }

    void Update()
    {
        crossbow.Rotate(0, Time.deltaTime * 10, 0);
    }
}
