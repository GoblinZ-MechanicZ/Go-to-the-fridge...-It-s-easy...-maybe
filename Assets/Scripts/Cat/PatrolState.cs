using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    [Header("CatSettings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _distanceTreshhold;

    private Vector3 _currentPoint;

    private bool _startMovement = false;
    private bool _startRotation = false;
    private void Update() 
    {
        Raycast(transform.forward);
        Raycast(transform.right);
        Raycast(-transform.right);

        if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), transform.forward * 4, 4f) && 
        Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), transform.right * 4, 4f) &&
        Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), -transform.right * 4, 4f) 
        && _startMovement == false && _startRotation == false)
            Rotate(transform.right);
    }

    private void Rotate(Vector3 direction)
    {
        _startRotation = true;

        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, angle, 0);

        _startRotation = false;
    }

    private IEnumerator Move(Vector3 point)
    {
        while(transform.position != point)
        {
            transform.position = Vector3.MoveTowards(transform.position, point, _speed * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        _startMovement = false;
    }

    private void Raycast(Vector3 direction)
    {
        if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), direction * 4, 4f) == false && _startMovement == false)
        {
            _currentPoint = transform.position + (direction * 4f);
            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Rotate(direction);

            _startMovement = true;
            StartCoroutine(Move(_currentPoint));

            Debug.Log(_currentPoint);
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), transform.forward * 4);
    }
}
