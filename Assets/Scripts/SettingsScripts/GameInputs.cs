using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInputs
{
    public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public static void initialize()
    {
        keys.TryAdd("Forward", KeyCode.W);
        keys.TryAdd("Left", KeyCode.A);
        keys.TryAdd("Back", KeyCode.S);
        keys.TryAdd("Right", KeyCode.D);
        keys.TryAdd("Jump", KeyCode.Space);

        keys.TryAdd("Reset Camera", KeyCode.Mouse2);
    }
}
