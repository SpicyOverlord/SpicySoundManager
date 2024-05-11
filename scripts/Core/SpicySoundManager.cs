using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

//VERSION: 1.1
[RequireComponent(typeof(AudioSource))]
public class SpicySoundManager : MonoBehaviour
{
    private static SpicySoundManager _instance;
    private static AudioSource _audioSource;
    public static bool EnableSoundEffects = true;

    [Label("Sound Manager Preset")] [Expandable]
    public SpicySoundManagerPreset smp;

    [Header("Test Sound")] public string soundName;

    [Button("Test Sound")]
    private void TestSound()
    {
        if (_instance == null)
            _instance = this;
        _audioSource = GetComponent<AudioSource>();

        PlaySound(soundName);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() => _instance = null;

    private void Awake()
    {
        //set static instance
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //set audio source, and play music if selected
        _audioSource = GetComponent<AudioSource>();

        _audioSource.playOnAwake = smp.playMusicOnAwake;
        if (smp.playMusicOnAwake && smp.music != null) PlayMusic();
        _audioSource.loop = smp.loopingMusic;

        //find all sound presets in project
        smp.sounds = new List<SpicySound>();
        foreach (SpicySound sound in (SpicySound[])Resources.FindObjectsOfTypeAll(typeof(SpicySound)))
        {
            smp.sounds.Add(sound);
        }
    }

    /// <summary>
    /// Play a sound preset
    /// </summary>
    /// <param name="soundName">The name of the sound preset file</param>
    public static void PlaySound(string soundName)
    {
        if (CheckForMissingInstance()) return;
        if (!EnableSoundEffects)
        {
            Debug.Log("Sound Effects not enabled");
            return;
        }
        
        List<SpicySound> matchedSounds = _instance.smp.sounds.Where(sound => sound.name == soundName).ToList();
        if (matchedSounds.Count == 0)
        {
            Debug.LogError("Sound: '" + soundName + "' not Found");
            return;
        }
        if (matchedSounds.Count > 1)
        {
            Debug.Log("Multiple sounds with the same name found. Playing the first one.");
        }

        var sound = matchedSounds[0];
        float volume;
        switch (sound.soundType)
        {
            case SoundType.SingleSound:
                volume = sound.randomizeVolume
                    ? RandomVolume(sound.minMaxVolume)
                    : sound.volume;
                _audioSource.PlayOneShot(sound.singleSound, volume);
                break;
            case SoundType.MultiSound:
                switch (sound.multiSoundType)
                {
                    case MultiSoundType.RandomSound:
                        volume = sound.randomizeVolume
                            ? RandomVolume(sound.minMaxVolume)
                            : sound.volume;
                        _audioSource.PlayOneShot(sound.GetRandomSound(), volume);
                        break;
                    case MultiSoundType.GeneratedSound:
                        foreach (var subSound in sound.sounds)
                        {
                            volume = sound.randomizeVolume
                                ? RandomVolume(sound.minMaxVolume)
                                : sound.volume;
                            _audioSource.PlayOneShot(subSound, volume);
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static float RandomVolume(Vector2 vol)
    {
        return Random.Range(vol.x * 100, vol.y * 100) / 100f;
    }


    /// <summary>
    /// Plays music
    /// </summary>
    /// <param name="looping">If the music should loop</param>
    public static void PlayMusic(bool looping = true)
    {
        if (_instance == null)
        {
            Debug.Log("Sound Manager not found");
            return;
        }

        _audioSource.loop = looping;

        _audioSource.clip = _instance.smp.music;
        _audioSource.Play();
    }

    /// <summary>
    /// Stops the music
    /// </summary>
    public static void StopMusic()
    {
        if (CheckForMissingInstance()) return;

        _audioSource.Stop();
    }

    /// <summary>
    /// Mute all sounds
    /// </summary>
    public static void Mute()
    {
        if (CheckForMissingInstance()) return;

        _audioSource.mute = true;
    }

    /// <summary>
    /// UnMute all sounds
    /// </summary>
    public static void UnMute()
    {
        if (CheckForMissingInstance()) return;

        _audioSource.mute = false;
    }

    /// <summary>
    /// Toggle Mute on all sounds
    /// </summary>
    public static void ToggleMute()
    {
        if (CheckForMissingInstance()) return;

        _audioSource.mute = !_audioSource.mute;
    }

    /// <summary>
    /// Pause the music
    /// </summary>
    public static void Pause()
    {
        if (CheckForMissingInstance()) return;
        _audioSource.Pause();
    }

    /// <summary>
    /// UnPause the music
    /// </summary>
    public static void UnPause()
    {
        if (CheckForMissingInstance()) return;
        _audioSource.UnPause();
    }

    /// <summary>
    /// Toggle pause on the music
    /// </summary>
    public static void TogglePause()
    {
        if (CheckForMissingInstance()) return;

        if (_audioSource.isPlaying)
        {
            _audioSource.Pause();
        }
        else
        {
            _audioSource.UnPause();
        }
    }


    /// <summary>
    /// Check if the Sound Manager is active
    /// </summary>
    /// <returns></returns>
    private static bool CheckForMissingInstance()
    {
        bool instanceIsMissing = _instance == null;
        if (instanceIsMissing) Debug.Log("Sound Manager not found");

        return instanceIsMissing;
    }
}