using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Session : MonoBehaviour
{
    public static Session instance { get; private set; }

    public Transform player;
    public IPlayerState playerState { get; private set; }

    public void Start()
    {
        GameInputs.initialize();
        instance = this;

        // initializing the start platform
        PlayerBuildModeState playerBuildModeState = new PlayerBuildModeState();
        playerBuildModeState.initialize();
        for (int x = -1; x < 2; x++)
        {
            for (int z = -1; z < 2; z++)
            {
                playerBuildModeState.placeTile(x, z);
            }
        }
        playerBuildModeState.finalize();

        playerState = new PlayerBuildModeState();
        playerState.initialize();
    }

    private void Update()
    {
        playerState.update();
    }
}
