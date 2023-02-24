using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 followLocation;
    [SerializeField] private float followDistance;

    [SerializeField] private float turnSpeed;
    [SerializeField] private float smoothTime;

    private Vector3 currentVelocity;
    private Vector3 targetPosition;

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, 
                target.position + target.right * followDistance + followLocation, ref currentVelocity, smoothTime);
        targetPosition.Set(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(targetPosition, Vector3.up);
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget.transform;
    }
}