using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* generate bubbles and find the moment for the 'birth'
 * based on examples from 
 * https://docs.unity3d.com/ScriptReference/ParticleSystem.Emit.html
 * https://www.youtube.com/watch?v=jKSz8JJnL4E 
*/
public class particleControl : MonoBehaviour
{
    // In this example, we have a Particle System emitting green particles; we then emit and override some properties every 2 seconds.
    private ParticleSystem system;
    public Material particleMaterial;
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.Particle[] particleArray;
    private int previousCount = -1; 
    // Start is called before the first frame update
    void Start()
    {
        // A simple particle material with no texture.
        //Material particleMaterial = new Material(Shader.Find("Particles/Unlit"));

        // Create a green Particle System.
        var go = new GameObject("Particle System");
        go.transform.Rotate(-90, 0, 0); // Rotate so the system emits upwards.
        system = go.AddComponent<ParticleSystem>();
        go.GetComponent<ParticleSystemRenderer>().material = particleMaterial;

        mainModule = system.main;
        //mainModule.useUnscaledTime = true; 
        mainModule.maxParticles = 1; //maximum particles allowed 
        mainModule.startColor = Color.yellow;
        mainModule.startSize = 0.5f;

        // Every 2 secs we will emit.
        InvokeRepeating("DoEmit", 2.0f, 2.0f);

        particleArray = new ParticleSystem.Particle[system.main.maxParticles]; 
    }


    // Update is called once per frame
    void Update()
    {
        var currentCount = system.particleCount; 

        //this will only log the first N particles;
        //if (currentCount> previousCount)
        //{
        //    Debug.Log("Squeee " + (currentCount- previousCount) + "/"+ currentCount);
        //}
        //previousCount = system.particleCount;

        var numParticlesAlive=  system.GetParticles(particleArray);
        if (numParticlesAlive > 0)
        {
            Debug.Log(particleArray[0].position);
        }

    }


    void DoEmit()
    {
        // Any parameters we assign in emitParams will override the current system's when we call Emit.
        // Here we will override the start color and size.
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.startColor = Color.white;
        emitParams.startSize = 0.2f;
        system.Emit(emitParams, 10);
        system.Play(); // Continue normal emissions
    }


}

