using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LayeredMusicPlayer : MonoBehaviour
{
    [Range(0, 1)] public float musicPercent;
    [SerializeField] private AudioClip[] layers;
    private List<AudioSource> _sources = new List<AudioSource>();
    [SerializeField] private AudioMixerGroup mixerGroup;

    public static LayeredMusicPlayer Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AddComponents();
    }

    void AddComponents()
    {
        foreach (var audioClip in layers)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();

            source.clip = audioClip;
            source.outputAudioMixerGroup = mixerGroup;
            source.loop = true;
            source.Play();
            _sources.Add(source);
        }
    }

    private void Update()
    {
        UpdateVolumes(musicPercent * layers.Length);
    }

    void UpdateVolumes(float volume)
    {
        for (var i = 0; i < _sources.Count; i++)
        {
            float diff = volume - i;
            _sources[i].volume = diff;
        }
    }
}
