using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New SoundManager", menuName = "SpicySoundManager/Create New SoundManager Preset", order = 1)]
public class SpicySoundManagerPreset : ScriptableObject {
    public bool playMusicOnAwake = true;
    public bool loopingMusic = true;
    public AudioClip music;

    [Space] [ReadOnly] public List<SpicySound> sounds = new List<SpicySound>();
}