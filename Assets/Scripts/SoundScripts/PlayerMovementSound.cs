using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSound : MonoBehaviour
{
    private Sound[] stepSounds;
    private Sound[] swimSounds;

    void Start()
    {
        stepSounds = GameUtility.loadSounds("Step", VolumeManager.stepVolume, 2);
        gameObject.createAudioSources(stepSounds);
        VolumeManager.stepSounds = stepSounds;

        swimSounds = GameUtility.loadSounds("Swimming", VolumeManager.swimVolume, 1);
        gameObject.createAudioSources(swimSounds);
        VolumeManager.swimSounds = swimSounds;
    }

    void playStepSound()
    {
        if(!PlayerEntity.instance.movement.isGrounded) return;
        if (PlayerEntity.instance.transform.position.y > 0) stepSounds.playRandom();
        else swimSounds.playRandom();
    }
}
