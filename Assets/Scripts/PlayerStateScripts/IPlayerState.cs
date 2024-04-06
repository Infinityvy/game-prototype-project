using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public abstract void initialize();
    public abstract void finalize();
    public abstract void update();
}
