using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameUtility
{
    public static Vector2Int toTilePosition(this Vector3 position)
    {
        return new Vector2Int(position.x.toTileCoordinate(), position.z.toTileCoordinate());    
    }

    public static int toTileCoordinate(this float coordinate)
    {
        return Mathf.RoundToInt(coordinate / TileBuilder.tileSize);
    }

    public static bool isMouseOnScreen()
    { 
        return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); 
    }

    public static void createAudioSources(this GameObject gameObject, Sound[] sounds)
    {
        foreach (Sound s in sounds) 
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public static void playRandom(this Sound[] sounds)
    {
        sounds[Random.Range(0, sounds.Length)].play();
    }

    public static Sound[] loadSounds(string path, float volume, float pitch)
    {
        AudioClip[] splashClips = Resources.LoadAll<AudioClip>(path);
        Sound[] sounds = new Sound[splashClips.Length];
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i] = new Sound(splashClips[i], volume, pitch);
        }

        return sounds;
    }

    /// <summary>
    /// Runs the given probability (0-1). Returns true when probability is hit.
    /// </summary>
    /// <param name="probability"></param>
    /// <returns></returns>
    public static bool runProbability(float  probability)
    {
        return Random.Range(0f, 1f) <= probability;
    }

    public static void takeScreenshot()
    {
        string exeDirectory = Directory.GetParent(Application.dataPath).FullName;

        string screenshotsDirectory = Path.Combine(exeDirectory, "Screenshots");

        if (!Directory.Exists(screenshotsDirectory))
        {
            Directory.CreateDirectory(screenshotsDirectory);
        }

        string screenshotPath = Path.Combine(screenshotsDirectory, "Screenshot_" + getFormattedDateTime() + ".png");
        ScreenCapture.CaptureScreenshot(screenshotPath);

        Debug.Log("Screenshot saved to: " + screenshotPath);
    }

    public static string getFormattedDateTime()
    {
        System.DateTime now = System.DateTime.Now;

        string formattedDateTime = now.ToString("yyyyMMddHHmmss");

        return formattedDateTime;
    }
}
