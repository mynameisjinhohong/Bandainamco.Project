using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusParticle_LJH : Particles_LJH
{
    [SerializeField] private List<ParticleSystem> extraParticles;

    protected override void Init()
    {
        base.Init();

        foreach (var p in extraParticles)
        {
            if (particles.Contains(p)) continue;
            particles.Add(p);
        }
    }

    public override void Play()
    {
        gameObject.SetActive(true);
        base.Play();
    }

    public override void Stop()
    {
        gameObject.SetActive(false);
        base.Stop();
    }
}
