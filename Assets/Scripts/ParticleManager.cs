using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{

    public ParticleSystem[] particles;

    public static ParticleManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void PlayParticleSystemAndChild(ParticleSystem p)
    {

        p.Play();

        if (p.transform.childCount != 0)
        {
            p.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
    }



    public void Play(int particleId)
    {
        ParticleSystem p = particles[particleId];
        PlayParticleSystemAndChild(p);
    }

    public void Play(int particleId, Vector3 pos)
    {
        ParticleSystem p = particles[particleId];

        p.transform.position = pos;

        PlayParticleSystemAndChild(p);

    }

    public void Play(int particleId, Vector3 pos, Vector3 rot)
    {
        ParticleSystem p = particles[particleId];

        p.transform.position = pos;
        p.transform.eulerAngles = rot;

        PlayParticleSystemAndChild(p);

    }

    public void Play(int particleId, Vector3 pos, Color col)
    {
        ParticleSystem p = particles[particleId];

        p.transform.position = pos;

        var main = p.main;
        main.startColor = new ParticleSystem.MinMaxGradient(col);

        PlayParticleSystemAndChild(p);
    }

    public void Play(int particleId, Vector3 pos, Vector3 rot, Color col)
    {
        ParticleSystem p = particles[particleId];

        p.transform.position = pos;
        p.transform.eulerAngles = rot;

        var main = p.main;
        main.startColor = new ParticleSystem.MinMaxGradient(col);

        PlayParticleSystemAndChild(p);
    }
}
