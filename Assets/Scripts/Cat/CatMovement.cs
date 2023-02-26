using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMovement : MonoBehaviour
{
    [SerializeField] private Transform _front;
    private bool _startRotation = false;
    private bool _startMovement = false;

    public Transform Front => _front;

    public bool StartMovement => _startMovement;
    public bool StartRotation => _startRotation;
    public void Rotate(Vector3 direction)
    {
        _startRotation = true;

        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, angle, 0);

        _startRotation = false;
    }

    public IEnumerator Move(Vector3 point, float speed)
    {
        _startMovement = true;

        while(transform.position != point)
        {
            transform.position = Vector3.MoveTowards(transform.position, point, speed * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }

        _startMovement = false;
    }
}
