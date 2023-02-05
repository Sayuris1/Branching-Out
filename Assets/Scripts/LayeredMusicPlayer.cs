using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class LayeredMusicPlayer : MonoBehaviour
{
    [System.Serializable]
    public class MusicData
    {
        public AudioClip Clip;
        public AudioMixerGroup MixerGroup = null;
    }
    
    [Range(0, 1)] public float musicPercent;
    [Range(0, 1)]public float _currMusicPercent;
    [SerializeField] private MusicData[] layers;
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
        foreach (var data in layers)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();

            source.clip = data.Clip;
            if (data.MixerGroup)
                source.outputAudioMixerGroup = data.MixerGroup;
            else
                source.outputAudioMixerGroup = mixerGroup;
            source.loop = true;
            source.Play();
            _sources.Add(source);
        }
    }

    private void Update()
    {
        _currMusicPercent = Mathf.Lerp(_currMusicPercent, musicPercent, Time.deltaTime * 0.5f);
        UpdateVolumes(_currMusicPercent * layers.Length);
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
