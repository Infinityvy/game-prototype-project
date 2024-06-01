using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class VolumeManager
{
    public static float playerHurtBaseVolume = 0.8f;
    public static float stepBaseVolume = 0.8f;
    public static float swimBaseVolume = 0.4f;
    public static float splashBaseVolume = 0.3f;
    public static float arrowBaseVolume = 0.5f;
    public static float machinegunBaseVolume = 0.5f;
    public static float upgradeBaseVolume = 0.5f;



    public static float masterVolume = 1f;
    public static float musicVolume = 1f;
    public static float effectsVolume = 1f;

    public static Sound[] music = null;
    public static Sound[] effects = null;

    public static void flush()
    {
        music = null;
        effects = null;
    }

    public static void addMusic(Sound[] newMusic)
    {
        if(music == null) 
        {
            music = newMusic;
            return;
        }

        music.Concat(newMusic);
    }

    public static void addEffects(Sound[] newEffects) 
    {
        if(effects == null) 
        {
            effects = newEffects;
            return;
        }

        effects.Concat(newEffects);
    }
}
