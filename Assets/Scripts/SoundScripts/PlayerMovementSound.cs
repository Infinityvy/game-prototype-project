using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSound : MonoBehaviour
{
    private Sound[] stepSounds;
    private Sound[] swimSounds;

    void Start()
    {
        stepSounds = GameUtility.loadSounds("Step", VolumeManager.stepBaseVolume, 2);
        gameObject.createAudioSources(stepSounds);
        VolumeManager.addEffects(stepSounds);

        swimSounds = GameUtility.loadSounds("Swimming", VolumeManager.swimBaseVolume, 1);
        gameObject.createAudioSources(swimSounds);
        VolumeManager.addEffects(swimSounds);
    }

    void playStepSound()
    {
        if(!PlayerEntity.instance.movement.isGrounded) return;
        //if (PlayerEntity.instance.transform.position.y > 0) stepSounds.playRandom();
        //else swimSounds.playRandom();

        if (PlayerEntity.instance.transform.position.y > 0) AkSoundEngine.PostEvent("footsteps_wood", gameObject);
        else AkSoundEngine.PostEvent("footsteps_water", gameObject);
    }
}
