using System.Collections;
using System.Collections.Generic;
using Assets.Utils;
using UnityEngine;

namespace Assets.Managers
{
    /// <summary>
    /// Class Sound and Music controls:Singleton
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        public JukeBox JB;
        public SoundBox SB;
        private AudioSource _loopedsource;
        public static SoundManager GetInstance()//if we use sm for load directly string sounds
        {
            if (!Instance)
            {
                GameObject soundManager = new GameObject("SoundManager(empty)");
                Instance = soundManager.AddComponent<SoundManager>();
                Instance.Initialize();
            }
            return Instance;
        }
        public static SoundManager GetInstance(JukeBox jb)//if we use sm with loaded music
        {
            if (!Instance)
            {
                GameObject soundManager = new GameObject("SoundManager");
                Instance = soundManager.AddComponent<SoundManager>();
                Instance.JB = jb;
                Instance.Initialize();
            }
            return Instance;
        }
        public static SoundManager GetInstance(JukeBox jb, SoundBox sb)//if we use complete sm with music&&sounds
        {
            if (!Instance)
            {
                GameObject soundManager = new GameObject("SoundManager");
                Instance = soundManager.AddComponent<SoundManager>();
                Instance.JB = jb;
                Instance.SB = sb;
                Instance.Initialize();
            }
            return Instance;
        }

        private const float MaxVolume_BGM = 0.8f;
        private const float MaxVolume_SFX = 1f;
        private static float CurrentVolumeNormalized_BGM = 1f;
        private static float CurrentVolumeNormalized_SFX = 1f;
        private static float CurrentVolumeModifier_BGM = 0.8f;
        private static bool isMuted;
        private List<AudioSource> sfxSources;
        private AudioSource bgmSource;

        private void Initialize()
        {
            // add our bgm sound source
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
            bgmSource.volume = GetBGMVolume();
            DontDestroyOnLoad(gameObject);
        }

        private static float GetBGMVolume()
        {
            return isMuted ? 0f : MaxVolume_BGM * CurrentVolumeNormalized_BGM;
        }
        public static float GetSFXVolume()
        {
            return isMuted ? 0f : MaxVolume_SFX * CurrentVolumeNormalized_SFX;
        }

        private void FadeBGMOut(float fadeDuration)
        {
            float delay = 0f;
            float toVolume = 0f;
            StartCoroutine(FadeBGM(toVolume, delay, fadeDuration));
        }

        private void FadeBGMIn(AudioClip bgmClip, float delay, float fadeDuration)
        {
            Instance.bgmSource.clip = bgmClip;
            Instance.bgmSource.Play();
            float toVolume = GetBGMVolume();
            StartCoroutine(FadeBGM(toVolume, delay, fadeDuration));
        }

        private IEnumerator FadeBGM(float fadeToVolume, float delay, float duration)
        {
            yield return new WaitForSeconds(delay);
            float elapsed = 0f;
            while (duration > 0)
            {
                float t = (elapsed / duration);
                float volume = Mathf.Lerp(0f, fadeToVolume * CurrentVolumeModifier_BGM, t);
                Instance.bgmSource.volume = volume;
                elapsed += Time.deltaTime;
                yield return 0;
            }
        }
        public static void PlayBGM(AudioClip bgmClip, bool fade, float fadeDuration)
        {
            if (fade)
            {
                if (Instance.bgmSource.isPlaying)
                {
                    // fade out, then switch and fade in
                    Instance.FadeBGMOut(fadeDuration / 2);
                    Instance.FadeBGMIn(bgmClip, fadeDuration / 2, fadeDuration / 2);
                }
                else
                {
                    // just fade in
                    float delay = 0f;
                    Instance.FadeBGMIn(bgmClip, delay, fadeDuration);
                }
            }
            else
            {
                // play immediately
                Instance.bgmSource.volume = GetBGMVolume();
                Instance.bgmSource.clip = bgmClip;
                Instance.bgmSource.Play();
            }
        }
        public static void PlayBGM(int number, bool fade, float fadeDuration)
        {
            if (fade)
            {
                if (Instance.bgmSource.isPlaying)
                {
                    // fade out, then switch and fade in
                    Instance.FadeBGMOut(fadeDuration / 2);
                    Instance.FadeBGMIn(Instance.JB.GetTrack(number), fadeDuration / 2, fadeDuration / 2);

                }
                else
                {
                    // just fade in
                    float delay = 0f;
                    Instance.FadeBGMIn(Instance.JB.GetTrack(number), delay, fadeDuration);
                }
            }
            else
            {
                // play immediately
                Instance.bgmSource.volume = GetBGMVolume();
                Instance.bgmSource.clip = Instance.JB.GetTrack(number);
                Instance.bgmSource.Play();
            }
        }

        private IEnumerator PlayNext(int number)
        {
            yield return new WaitForSeconds(Instance.JB.GetTrack(number).length);
            if (JB.GetTrack(number + 1) != null)
            {
                PlayPlaylist(number + 1, true, 1);
            }
            else
            {
                PlayPlaylist(0, true, 1);
            }
        }
        public static void PlayPlaylist(int number, bool fade, float fadeDuration)
        {
            if (fade)
            {
                if (Instance.bgmSource.isPlaying)
                {
                    // fade out, then switch and fade in
                    Instance.FadeBGMOut(fadeDuration / 2);
                    Instance.FadeBGMIn(Instance.JB.GetTrack(number), fadeDuration / 2, fadeDuration / 2);
                }
                else
                {
                    // just fade in
                    float delay = 0f;
                    Instance.FadeBGMIn(Instance.JB.GetTrack(number), delay, fadeDuration);
                }
            }
            else
            {
                // play immediately
                Instance.bgmSource.volume = GetBGMVolume();
                Instance.bgmSource.clip = Instance.JB.GetTrack(number);
                Instance.bgmSource.Play();
            }
            Instance.StartCoroutine(Instance.PlayNext(number));
        }
        public static void StopBGM(bool fade, float fadeDuration)
        {
            if (Instance.bgmSource.isPlaying)
            {
                // fade out, then switch and fade in
                if (fade)
                {
                    Instance.FadeBGMOut(fadeDuration);
                }
                else
                {
                    Instance.bgmSource.Stop();
                }
            }
        }

        private AudioSource GetSFXSource()
        {
            // set up a new sfx sound source for each new sfx clip
            AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
            sfxSource.volume = GetSFXVolume();
            if (sfxSources == null)
            {
                sfxSources = new List<AudioSource>();
            }
            else
                sfxSources.Add(sfxSource);
            return sfxSource;
        }

        private IEnumerator RemoveSFXSource(AudioSource sfxSource)
        {
            yield return new WaitForSeconds(sfxSource.clip.length);
            sfxSources.Remove(sfxSource);
            Destroy(sfxSource);
        }

        private IEnumerator RemoveSFXSourceFixedLength(AudioSource sfxSource, float length)
        {
            yield return new WaitForSeconds(length);
            sfxSources.Remove(sfxSource);
            Destroy(sfxSource);
        }
        public static void RemoveSFXSourceLooped()
        {
            Instance.StartCoroutine(Instance.RemoveSFXSource(Instance._loopedsource));
        }
        public static void PlaySFX(AudioClip sfxClip)
        {
            AudioSource source = Instance.GetSFXSource();
            source.volume = GetSFXVolume();
            source.clip = sfxClip;
            source.Play();
            Instance.StartCoroutine(Instance.RemoveSFXSource(source));
        }
        public static void PlaySFX(string key)
        {
            AudioSource source = Instance.GetSFXSource();
            source.volume = GetSFXVolume();
            source.clip = Instance.SB.GetSFX(key);
            source.Play();
            Instance.StartCoroutine(Instance.RemoveSFXSource(source));
        }
        public static void PlaySFX(string key, float volume)
        {
            AudioSource source = Instance.GetSFXSource();
            source.volume = GetSFXVolume() * volume;
            source.clip = Instance.SB.GetSFX(key);
            source.Play();
            Instance.StartCoroutine(Instance.RemoveSFXSource(source));
        }
        public static void PlayLoopSFX(string key)
        {
            AudioSource source = Instance.GetSFXSource();
            source.volume = GetSFXVolume();
            source.clip = Instance.SB.GetSFX(key);
            source.loop = true;
            Instance._loopedsource = source;
            source.Play();     
        }
        public static void PlayLoopSFX(string key, float volume)
        {
            AudioSource source = Instance.GetSFXSource();
            source.volume = GetSFXVolume() * volume;
            source.clip = Instance.SB.GetSFX(key);
            source.loop = true;
            Instance._loopedsource = source;
            source.Play();
        }
        public static void PlaySFXRandomized(AudioClip sfxClip)
        {
            AudioSource source = Instance.GetSFXSource();
            source.volume = GetSFXVolume();
            source.clip = sfxClip;
            source.pitch = Random.Range(0.85f, 1.2f);
            source.Play();
            Instance.StartCoroutine(Instance.RemoveSFXSource(source));
        }
        public static void PlaySFXFixedDuration(AudioClip sfxClip, float duration, float volumeMultiplier = 1.0f)
        {
            AudioSource source = Instance.GetSFXSource();
            source.volume = GetSFXVolume() * volumeMultiplier;
            source.clip = sfxClip;
            source.loop = true;
            source.Play();
            Instance.StartCoroutine(Instance.RemoveSFXSourceFixedLength(source, duration));
        }
        public static void DisableSoundImmediate()
        {
            Instance.StopAllCoroutines();
            if (Instance.sfxSources != null)
            {
                foreach (AudioSource source in Instance.sfxSources)
                {
                    source.volume = 0;
                }
            }
            Instance.bgmSource.volume = 0f;
            isMuted = true;
        }

        public static void EnableSoundImmediate()
        {
            if (Instance.sfxSources != null)
            {
                foreach (AudioSource source in Instance.sfxSources)
                {
                    source.volume = GetSFXVolume();
                }
            }
            Instance.bgmSource.volume = GetBGMVolume();
            isMuted = false;
        }
        public static void SetGlobalVolume(float newVolume)
        {
            CurrentVolumeNormalized_BGM = newVolume;
            CurrentVolumeNormalized_SFX = newVolume;
            AdjustSoundImmediate();
        }
        public static void SetSFXVolume(float newVolume)
        {
            CurrentVolumeNormalized_SFX = newVolume;
            AdjustSoundImmediate();
        }
        public static void SetBGMVolume(float newVolume)
        {
            CurrentVolumeNormalized_BGM = newVolume;
            AdjustSoundImmediate();
        }
        public static void AdjustSoundImmediate()
        {
            if (Instance.sfxSources != null)
            {
                foreach (AudioSource source in Instance.sfxSources)
                {
                    source.volume = GetSFXVolume();
                }
            }
            Instance.bgmSource.volume = GetBGMVolume();
        }
    }
}