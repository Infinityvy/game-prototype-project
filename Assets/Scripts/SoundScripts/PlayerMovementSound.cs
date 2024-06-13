using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSound : MonoBehaviour
{
    void Start()
    {

    }

    void playStepSound()
    {
        if(!PlayerEntity.instance.movement.isGrounded) return;

        if (PlayerEntity.instance.transform.position.y > 0) AkSoundEngine.PostEvent("footsteps_wood", gameObject);
        else AkSoundEngine.PostEvent("footsteps_water", gameObject);
    }
}
