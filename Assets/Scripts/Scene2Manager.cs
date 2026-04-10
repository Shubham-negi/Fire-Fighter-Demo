using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2Manager : MonoBehaviour
{


    public GameObject waterSystem;
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
}
