using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ResourceBlock
{
    public ResourceBlock(int wood, int metal)
    {
        this.wood = wood;
        this.metal = metal;
    }

    public int wood;
    public int metal;

    public bool isEmpty()
    {
        return wood == 0 && metal == 0;
    }

    public static ResourceBlock operator +(ResourceBlock a, ResourceBlock b)
    {
        return new ResourceBlock(a.wood + b.wood, a.metal + b.metal);
    }

    public static ResourceBlock operator -(ResourceBlock a, ResourceBlock b)
    {
        return new ResourceBlock(a.wood - b.wood, a.metal - b.metal);
    }

    public static ResourceBlock operator *(ResourceBlock a, int b)
    {
        return new ResourceBlock(a.wood * b, a.metal * b);
    }

    public static ResourceBlock operator /(ResourceBlock a, int b) 
    {
        return new ResourceBlock(a.wood / b, a.metal / b);
    }
}
