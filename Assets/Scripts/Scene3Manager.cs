using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3Manager : MonoBehaviour
{
    public ParticleSystem[] fireParticles;
    public Material foamMaterial;

    public GameObject waterSystem;

     public GameObject foamSystem;


    /// <summary>
    /// WATER SYSTEM CONTROL 
    /// </summary>

    public void waterStepCounter(int step)
    {
        if (step == 2)
        {
            waterSystem.SetActive(true);
        }
        else
        {
            waterSystem.SetActive(false);
        }
    }
    
     public void FoamStepCounter(int step)
    {
        if (step == 2)
        {
            foamSystem.SetActive(true);
        }
        else
        {
            foamSystem.SetActive(false);
        }
    }



    /// <summary>
    /// WATER SYSTEM CONTROL 
    /// </summary>
    /// 
    /// 
    /// 



}
