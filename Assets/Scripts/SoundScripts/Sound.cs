using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name = "";

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(.1f, 3f)]
    public float pitch = 1f;

    [HideInInspector]
    public AudioSource source;

    public Sound(AudioClip clip, float volume, float pitch)
    {
        this.clip = clip;
        this.volume = volume;
        this.pitch = pitch;

        name = clip.name;
    }

    public void play() { source.Play(); }
}
