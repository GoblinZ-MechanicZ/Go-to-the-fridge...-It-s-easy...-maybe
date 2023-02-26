using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFearTransition : Transition
{
    private void OnEnable() 
    {
        
    }

    private void OnDisable() 
    {
        
    }

    private void OnPlayerAttack()
    {
        NeedTransit = true;
    }
}
