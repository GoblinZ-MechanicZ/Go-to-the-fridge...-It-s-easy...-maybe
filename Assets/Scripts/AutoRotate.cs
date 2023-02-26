using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] private Vector3 rotateAxis;
    [SerializeField] private float rotateSpeed;

    private void FixedUpdate()
    {
        transform.Rotate(rotateAxis * rotateSpeed);
    }
}