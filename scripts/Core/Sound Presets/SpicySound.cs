using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

//[Serializable]
[CreateAssetMenu(fileName = "New Sound", menuName = "SpicySoundManager/Create New Sound Preset", order = 1)]
public class SpicySound : ScriptableObject {
    [Space] [BoxGroup("Sound Settings")] public SoundType soundType;

    [ShowIf("MultiSound")] [BoxGroup("Sound Settings")]
    public MultiSoundType multiSoundType;


    [BoxGroup("Sound Settings")] public bool randomizeVolume;

    [MinMaxSlider(0f, 1f)] [ShowIf("randomizeVolume")] [BoxGroup("Sound Settings")]
    public Vector2 minMaxVolume = new Vector2(0, 1);

    [HideIf("randomizeVolume")] [BoxGroup("Sound Settings")]
    public float volume = 1;


    [ShowIf("SingleSound")] [BoxGroup("Sound Settings")]
    public AudioClip singleSound;

    [ShowIf("MultiSound")] [BoxGroup("Sound Settings")]
    public List<AudioClip> sounds;

    [ResizableTextArea] public string notes;


    private bool SingleSound() => soundType == SoundType.SingleSound;
    private bool MultiSound() => soundType == SoundType.MultiSound;

    private bool HideVolume() => multiSoundType == MultiSoundType.RandomSound || soundType != SoundType.SingleSound;

    private bool GeneratedSound() =>
        multiSoundType == MultiSoundType.GeneratedSound && soundType != SoundType.SingleSound;

    public AudioClip GetRandomSound() {
        return sounds[Random.Range(0, sounds.Count)];
    }
}

public enum SoundType {
    SingleSound,
    MultiSound
}

public enum MultiSoundType {
    RandomSound,
    GeneratedSound
}