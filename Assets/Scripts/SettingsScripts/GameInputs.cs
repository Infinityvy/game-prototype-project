using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInputs
{
    public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public static void initialize()
    {
        keys.Add("Forward", KeyCode.W);
        keys.Add("Left", KeyCode.A);
        keys.Add("Back", KeyCode.S);
        keys.Add("Right", KeyCode.D);
        keys.Add("Jump", KeyCode.Space);

        keys.Add("Reset Camera", KeyCode.Mouse2);
    }
}
