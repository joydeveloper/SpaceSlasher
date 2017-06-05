using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for load and store soundSFX
/// </summary>
public class SoundBox {
    public bool isSoundready;
    public Dictionary<string,AudioClip> Sfxclips;
    public SoundBox(string path, string[] key, string[] name)
    {
        SFXAppropriate(path, key, name);
    }
    /// <summary>
    /// Write fullpath with "/" for example: fullpath= Sound/Track/music1
    /// </summary>
    public static AudioClip LoadClip(string fullpath)//you must write fullpath with /----for example: fullpath= Sound/Track/music1
    {   
        AudioClip clip = Resources.Load<AudioClip>(fullpath);
        return clip;
    }
    public static AudioClip LoadClip(string path, string name)
    {
        AudioClip clip = Resources.Load<AudioClip>(path + name);
        return clip;
    }
    private void SFXAppropriate(string path,string[]key,string[] name)
    {
        Sfxclips = new Dictionary<string, AudioClip>();
        for (int i=0;i<name.Length;i++)
        {
           AudioClip sfx= LoadClip(path, name[i]);
            Sfxclips.Add(key[i], sfx);
        }
        isSoundready = true;
    }
    public AudioClip GetSFX(string key)
    {
        return Sfxclips[key];
    }
}
