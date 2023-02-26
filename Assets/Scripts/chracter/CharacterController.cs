using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private List<Vector3> _passedPoints = new();
    public List<Vector3> PassedPoints => _passedPoints;
    private bool _isCrouch = false;
    private bool _hasSmetana = false;   
    public bool IsCrouch { get {return _isCrouch; } }
    public bool HasSmetana { get { return _hasSmetana; } }

    private Vector3 endMovement;
    public Vector3 EndMovement {get { return endMovement; } }

    
}
