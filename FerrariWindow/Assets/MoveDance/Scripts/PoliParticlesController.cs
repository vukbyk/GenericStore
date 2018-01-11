using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoliParticlesController : MonoBehaviour
{
    [System.Serializable]
    public class Subparticle
    {
        public ParticleSystem p;
        public float startDelay = 0;
        public float duration = 0;
        public float endDelay = 0;
    }

    public Subparticle[] poliParticles;

    bool playing = false;
    float timeFromStart=-1;
    float timeFromEnd=-1;
 
    void Start ()
    {
        foreach (Subparticle sp in poliParticles)
        {
            ParticleSystem.EmissionModule em = sp.p.emission;
            em.enabled = false;
        }
    }

	public void play()
    {
        playing = true;
        timeFromStart = 0;
        timeFromEnd = -1;
    }

    public void stop()
    {
        timeFromEnd = 0;
        timeFromStart = -1;
    }
 
    void Update ()
    {
        if (playing)
        {
            if (timeFromStart >= 0)
            {
                timeFromStart += Time.deltaTime;
                foreach (Subparticle sp in poliParticles)
                {
                    if (timeFromStart >= sp.startDelay)
                    {
                        ParticleSystem.EmissionModule em = sp.p.emission;
                        em.enabled = true;
                    }
                    if ( sp.duration > 0 && timeFromStart >= sp.duration)
                    {
                        ParticleSystem.EmissionModule em = sp.p.emission;
                        em.enabled = false;
                    }
                }
            }
            if (timeFromEnd >= 0)
            {
                timeFromEnd += Time.deltaTime;
                foreach (Subparticle sp in poliParticles)
                {
                    if (timeFromEnd >= sp.endDelay)
                    {
                        ParticleSystem.EmissionModule em = sp.p.emission;
                        em.enabled = false;
                    }                    
                    else //playing flag stayes false only after all emiters are false
                    {
                        playing = true;
                    }
                }
            }
        }
    }
}
