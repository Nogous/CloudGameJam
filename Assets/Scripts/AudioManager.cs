using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1;

    [HideInInspector] public AudioSource source;
    public bool loop;

    public AudioMixerGroup audioMixerGroup;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;

    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s==null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        s.source.Play();
    }
}
