using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private List<EntityAction> entityActions = new List<EntityAction>();


    private void Update() {
        foreach(var action in entityActions) {
            action.Update(this);
        }
    }
}
