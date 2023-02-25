using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFearTransition : Transition
{
    private void Update() 
    {
        if(GetComponent<FearState>().NumberOfCurrentPoint == 0)
            NeedTransit = true;
    }
}
