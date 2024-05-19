using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaceable
{
    public PlaceableType type { get; protected set; }

    public Transform transform { get; }
}
