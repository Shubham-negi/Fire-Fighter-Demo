using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IsolationTankActivator : MonoBehaviour
{

    public UnityEvent taskEndEvent;
    public WaterSequentialActivatorManager tankActivator1;
    public WaterSequentialActivatorManager tankActivator2;

    public WaterSequentialActivatorManager tankActivator3;


    public void onstepComplete(int stepIndex)
    {

        if (stepIndex == 1)
        {
            tankActivator1.WaterSequentialActivator();

            tankActivator2.WaterSequentialActivator();

            tankActivator3.WaterSequentialActivator();

            taskEndEvent?.Invoke();
        }

    }
}
