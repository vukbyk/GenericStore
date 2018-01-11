using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControl : MonoBehaviour
{
    public PoliParticlesController[] effect;

    int playing = 0;

    void Start ()
    {
        effect[playing].play();
    }
	
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            nextEffect();
        }
        
    }

    public void nextEffect()
    {
        effect[playing].stop();
        playing++;
        playing %= effect.Length;
        effect[playing].play();
        
    }
}
