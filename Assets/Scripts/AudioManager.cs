using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    AudioClip[] AudioClips;

    AudioSource source;

    Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
    private void Awake()
    {
        Instance = this;

        source = GetComponent<AudioSource>();

        for (int i = 0; i < AudioClips.Length; i++)
        {
            clips.Add(AudioClips[i].name, AudioClips[i]);
        }
    }

    public void PlaySound(string name)
    {
        source.PlayOneShot(clips[name]);
    }

    public void PlaySoundPitchVariant(string name, float minPitch, float maxPitch)
    {
        source.pitch = Random.Range(minPitch, maxPitch);
        PlaySound(name);
    }
}
