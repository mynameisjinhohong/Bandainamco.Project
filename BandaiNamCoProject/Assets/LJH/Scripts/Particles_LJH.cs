using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles_LJH : MonoBehaviour
{
    protected List<ParticleSystem> particles;
    protected List<float> originEmission;

    private bool isInit = false;

    private void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        if (isInit) return;

        particles = new List<ParticleSystem>();
        particles.AddRange(GetComponentsInChildren<ParticleSystem>(true));

        originEmission = new List<float>();

        foreach(var p in particles)
        {
            originEmission.Add(p.emission.rateOverTimeMultiplier);
        }

        isInit = true;
    }

    public virtual void Play() {
        for(int i=0; i<particles.Count; i++)
        {
            var p = particles[i];
            p.gameObject.SetActive(true);
            var emission = p.emission;
            emission.enabled = true;
            emission.rateOverTimeMultiplier = originEmission[i];
            //p.Clear();
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
