using System.Collections.Generic;
using UnityEngine;

namespace Assets.Utils
{
    /// <summary>
    /// Class for load and store soundTracks
    /// </summary>
    public class JukeBox
    {
        public List<AudioClip> Tracks;
        public bool isJukeready;
        public JukeBox(string path, string[] clips)
        {
            LoadClips(path, clips);
        }
        public static AudioClip LoadClip(string name)
        {
            string path = "Sound/" + name;
            AudioClip clip = Resources.Load<AudioClip>(path);
            return clip;
        }
        public static AudioClip LoadClip(string path, string name)
        {
            AudioClip clip = Resources.Load<AudioClip>(path + name);
            return clip;
        }
        public void LoadClips(string path, string[] name)
        {
            Tracks = new List<AudioClip>();
            foreach (string t in name)
            {
                Tracks.Add(Resources.Load<AudioClip>(path + t));
            }
            isJukeready = true;
        }
        public AudioClip GetTrack(int i)
        {
            return Tracks[i];
        }
    }
}