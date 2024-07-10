using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    AudioSource SFXSource;

    [SerializeField]
    AudioSource MusicSource;

    [SerializeField]
    AudioClip[] AudioClips;

    [SerializeField]
    AudioClip MusicClip;

    Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
    private void Awake()
    {
        Instance = this;

        SFXSource = GetComponent<AudioSource>();

        for (int i = 0; i < AudioClips.Length; i++)
        {
            clips.Add(AudioClips[i].name, AudioClips[i]);
        }
    }
    private void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        MusicSource.clip = MusicClip;
        MusicSource.Play();
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }

    public void PlaySound(string name)
    {
        SFXSource.PlayOneShot(clips[name]);
    }

    public void PlaySoundPitchVariant(string name, float minPitch, float maxPitch)
    {
        SFXSource.pitch = Random.Range(minPitch, maxPitch);
        PlaySound(name);
    }
}
