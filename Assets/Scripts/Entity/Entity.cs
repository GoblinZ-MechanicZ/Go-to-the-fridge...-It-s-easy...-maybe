using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private string entityName = "Unnamed";
    [SerializeField] private EntityType entityType = EntityType.None;
    [SerializeField] private List<EntityAction> entityActions = new List<EntityAction>();
    // [SerializeReference] private EntityAction entityActions;


    private void Update()
    {
        foreach (var action in entityActions)
        {
            action?.UpdateAction(this);
        }
    }
}
