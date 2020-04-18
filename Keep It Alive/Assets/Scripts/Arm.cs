using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform target;
    private SpringJoint2D targetJoint;
    private bool selected = false;
    [SerializeField] private Transform handSelector;
    [SerializeField] private float selectorRadius = 1f;

    [Header("Attachment")]
    [SerializeField] private float detachCooldownDuration = 1f;
    private bool attachable = true;
    private bool attached = false;
    [SerializeField] float limbBreakForce = Mathf.Infinity;

    private MainBody body;
    private FixedJoint2D fixedJoint;
    private Rigidbody2D rb;

    private BodyJointBehaviour attachmentPoint;
    private SpringJoint2D attachmentPointJoint;

    // Start is called before the first frame update
    void Start()
    {
        fixedJoint = GetComponent<FixedJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        //handSelector = transform.Find("HandSelector");
        body = FindObjectOfType<MainBody>();
        targetJoint = target.GetComponent<SpringJoint2D>();

        DetachArm();
        //DeselectArm();
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

        if (selected)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.position = mousePos;

            // arm break
            if (fixedJoint.reactionForce.magnitude > limbBreakForce)
            {
                DetachArm();
                Debug.Log("Arm broke off!");
            }


            // Deselect the arm
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                DeselectArm();
            }
        }
        else
        {
            // Select the arm
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Vector2.Distance(mousePos, handSelector.position) < selectorRadius)
                {
                    SelectArm();
                }
            }
        }
    }

    public void AttachArm(BodyJointBehaviour bodyPoint)
    {
        if (attachable && !attached)
        {
            attached = true;
            attachable = false;

            attachmentPoint = bodyPoint;
            attachmentPointJoint = attachmentPoint.GetComponent<SpringJoint2D>();

            transform.position = attachmentPoint.transform.position;
            fixedJoint.enabled = true;
            fixedJoint.connectedBody = body.GetComponent<Rigidbody2D>();
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
        attached = false;
        attachable = false;

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
        StartCoroutine(DetachCooldown());

        Debug.Log("Arm detached");
    }

    void SelectArm()
    {
        selected = true;
        // find some components
        Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
        if (attached)
        {
            // make arm independent from body
            attachmentPointJoint.enabled = false;
            attachmentPointJoint.connectedBody = null;
        }
        // make joint from target to arm
        targetRb.bodyType = RigidbodyType2D.Kinematic;
        targetRb.velocity = Vector2.zero;
        targetJoint.enabled = true;

        Debug.Log("Arm selected");
    }

    void DeselectArm()
    {
        selected = false;
        // find some components
        Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
        if (attached)
        {
            // make joint from attachmentpoint to target
            attachmentPointJoint.enabled = true;
            attachmentPointJoint.connectedBody = targetRb;
        }
        // let target floooow
        target.position = handSelector.position;
        targetRb.bodyType = RigidbodyType2D.Dynamic;
        targetJoint.enabled = false;

        Debug.Log("Arm deselected");
    }

    IEnumerator DetachCooldown()
    {
        yield return new WaitForSeconds(detachCooldownDuration);
        attachable = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(handSelector.position, selectorRadius);
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        //DetachArm();
        //gameObject.AddComponent<FixedJoint2D>();
        // doe goeie waardes
    }
}
