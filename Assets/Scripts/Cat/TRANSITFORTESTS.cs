using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRANSITFORTESTS : Transition
{
    [SerializeField] private bool _yes = false;

    private void Update() {
        if(_yes)
            NeedTransit = true;
    }
}
