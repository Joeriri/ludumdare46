using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [HideInInspector] public Transform target;

    [SerializeField] public float smoothSpeed = 0.125f;
    [SerializeField] public Vector3 offset;

    [SerializeField] private bool lockY = false;

    private Vector3 velocity;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        // position with possible lock
        transform.position = new Vector3(smoothedPosition.x, lockY ? transform.position.y : smoothedPosition.y, smoothedPosition.z);
    }
}
