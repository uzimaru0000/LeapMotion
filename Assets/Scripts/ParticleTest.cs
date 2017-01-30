using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour {

    public ParticleSystem particle;
    public bool isPlay;

    void Start() { 
        particle.Pause();
    }

    void Update() {
        if (isPlay) particle.Play(true);
        else particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

}
