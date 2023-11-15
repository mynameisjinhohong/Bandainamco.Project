using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles_LJH : MonoBehaviour
{
    protected List<ParticleSystem> particles;
    protected List<float> originEmission;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        particles = new List<ParticleSystem>();
        particles.AddRange(GetComponentsInChildren<ParticleSystem>());

        originEmission = new List<float>();

        foreach(var p in particles)
        {
            originEmission.Add(p.emission.rateOverTimeMultiplier);
        }
    }

    public virtual void Play() {
        for(int i=0; i<particles.Count; i++)
        {
            var p = particles[i];
            var emission = p.emission;
            emission.rateOverTimeMultiplier = originEmission[i];
            p.Clear();
            p.Play();
        }
    }
    public virtual void Stop() {
        foreach(var p in particles)
        {
            var emission = p.emission;
            emission.rateOverTimeMultiplier = 0;
            p.Stop();
        }
    }
}
