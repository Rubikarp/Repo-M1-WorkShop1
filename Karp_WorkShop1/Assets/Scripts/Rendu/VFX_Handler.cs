using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Handler : MonoBehaviour
{
    public InputHandler input;
    public ParticleSystem left, right;

    void Update()
    {
        if (input.isPlanning)
        {
            left.Play();
            right.Play();
        }
        else
        {
            if (left.isEmitting)
            {
                left.Stop();
            }
            if (right.isEmitting)
            {
                right.Stop();
            }
        }
    }
}
