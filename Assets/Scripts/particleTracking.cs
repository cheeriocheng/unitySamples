using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class particleTracking : MonoBehaviour
{
    private ParticleSystem _parentParticleSystem;
    private IDictionary<uint, ParticleSystem.Particle> _trackedParticles = new Dictionary<uint, ParticleSystem.Particle>();
    int _count;

    // Start is called before the first frame update
    void Start()
    {
        _parentParticleSystem = this.GetComponent<ParticleSystem>();
        if (_parentParticleSystem == null)
            Debug.LogError("Missing ParticleSystem!", this);

        _count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        var liveParticles = new ParticleSystem.Particle[_parentParticleSystem.particleCount];
        _parentParticleSystem.GetParticles(liveParticles);

        var particleDelta = GetParticleDelta(liveParticles);

        foreach (var particleAdded in particleDelta.Added)
        {
            Debug.Log("whee" + _count +" " + particleAdded.position );
            _count++;
        }

        foreach (var particleRemoved in particleDelta.Removed)
        {
            Debug.Log("pop" + _count);
            _count--;

        }

    }

    private ParticleDelta GetParticleDelta(ParticleSystem.Particle[] liveParticles)
    {
        var deltaResult = new ParticleDelta();

        foreach (var activeParticle in liveParticles)
        {
            ParticleSystem.Particle foundParticle;
            if (_trackedParticles.TryGetValue(activeParticle.randomSeed, out foundParticle))
            {
                _trackedParticles[activeParticle.randomSeed] = activeParticle;
            }
            else
            {
                deltaResult.Added.Add(activeParticle);
                _trackedParticles.Add(activeParticle.randomSeed, activeParticle);
            }
        }

        var updatedParticleAsDictionary = liveParticles.ToDictionary(x => x.randomSeed, x => x);
        var dictionaryKeysAsList = _trackedParticles.Keys.ToList();

        foreach (var dictionaryKey in dictionaryKeysAsList)
        {
            if (updatedParticleAsDictionary.ContainsKey(dictionaryKey) == false)
            {
                deltaResult.Removed.Add(_trackedParticles[dictionaryKey]);
                _trackedParticles.Remove(dictionaryKey);
            }
        }

        return deltaResult;
    }

    private class ParticleDelta
    {
        public IList<ParticleSystem.Particle> Added { get; set; } = new List<ParticleSystem.Particle>();
        public IList<ParticleSystem.Particle> Removed { get; set; } = new List<ParticleSystem.Particle>();
    }
}
