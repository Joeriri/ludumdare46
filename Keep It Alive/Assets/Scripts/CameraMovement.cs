using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [HideInInspector] public Transform target;

    [SerializeField] public float smoothSpeed = 0.125f;
    [SerializeField] public Vector3 offset;

    private Vector3 velocity;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
