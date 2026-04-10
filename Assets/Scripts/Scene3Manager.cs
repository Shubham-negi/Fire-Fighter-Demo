using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3Manager : MonoBehaviour
{
    public ParticleSystem[] particleSystems;
    public Material foamMaterial;

    public GameObject waterSystem;


    /// <summary>
    /// WATER SYSTEM CONTROL 
    /// </summary>

    public void StepCounter(int step)
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




    /// <summary>
    /// WATER SYSTEM CONTROL 
    /// </summary>
    /// 
    /// 
    /// 



}
