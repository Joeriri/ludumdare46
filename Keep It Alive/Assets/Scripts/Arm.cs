using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform target;

    [Header("Attachment")]
    [SerializeField] private float detachCooldownDuration = 1f;
    private bool attachable = true;


    private MainBody body;
    private FixedJoint2D fixedJoint;

    private GameObject attachmentPoint;
    private SpringJoint2D attachmentPointJoint;

    // Start is called before the first frame update
    void Start()
    {
        fixedJoint = GetComponent<FixedJoint2D>();

        body = FindObjectOfType<MainBody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SelectArm();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            DeselectArm();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            DetachArm();
        }
    }

    public void AttachArm(GameObject attachmentObject)
    {
        if (attachable)
        {
            attachmentPoint = attachmentObject;
            attachmentPointJoint = attachmentPoint.GetComponent<SpringJoint2D>();

            transform.position = attachmentPoint.transform.position;
            fixedJoint.enabled = true;
            fixedJoint.connectedBody = body.GetComponent<Rigidbody2D>();
            attachable = false;
            // Move layers to avoid nasty collisions
            Transform[] _children = GetComponentsInChildren<Transform>();
            foreach (Transform child in _children)
            {
                child.gameObject.layer = 8;
            }
            
            Debug.Log("Arm attached");
        }
    }

    public void DetachArm()
    {
        attachmentPoint = null;
        attachmentPointJoint = null;

        fixedJoint.enabled = false;
        // Put arm back on default layer
        Transform[] _children = GetComponentsInChildren<Transform>();
        foreach (Transform child in _children)
        {
            child.gameObject.layer = 0;
        }
        // start attach cooldown to prevent instant re attachment.
        attachable = false;
        StartCoroutine(DetachCooldown());

        Debug.Log("Arm detached");
    }

    void SelectArm()
    {
        // find some components
        Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
        SpringJoint2D targetJoint = target.GetComponent<SpringJoint2D>();
        // make arm independent from body
        attachmentPointJoint.enabled = false;
        attachmentPointJoint.connectedBody = null;
        // make joint from target to arm
        targetRb.bodyType = RigidbodyType2D.Kinematic;
        targetRb.velocity = Vector2.zero;
        targetJoint.enabled = true;

        Debug.Log("Arm deselected");
    }

    void DeselectArm()
    {
        // find some components
        Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
        SpringJoint2D targetJoint = target.GetComponent<SpringJoint2D>();
        // make joint from attachmentpoint to target
        attachmentPointJoint.enabled = true;
        attachmentPointJoint.connectedBody = targetRb;
        // let target floooow
        targetRb.bodyType = RigidbodyType2D.Dynamic;
        targetJoint.enabled = false;

        Debug.Log("Arm deselected");
    }

    IEnumerator DetachCooldown()
    {
        yield return new WaitForSeconds(detachCooldownDuration);
        attachable = true;
    }
}
