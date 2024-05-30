using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Session : MonoBehaviour
{
    public static Session instance { get; private set; }

    public bool isPaused = false;

    public Transform player;
    public IPlayerState playerState { get; private set; }

    public void Start()
    {
        GameInputs.initialize();
        instance = this;

        TileBuilder.flush();
        PlaceableBuilder.flush();

        // initializing the start platform
        PlayerBuildModeState playerBuildModeState = new();
        playerBuildModeState.initialize();
        playerBuildModeState.createStartPlatform();
        playerBuildModeState.finalize();

        playerState = new PlayerBuildModeState();
        playerState.initialize();
    }

    private void Update()
    {
        playerState.update();
    }

    public void setPaused(bool state)
    {
        isPaused = state;
        Time.timeScale = (state ? 0 : 1);
    }
}
