using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm_2 : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Target target;
    private SpringJoint2D targetJoint;
    private Rigidbody2D targetRb;
    private bool selected = false;
    [SerializeField] private Transform handSelector;
    [SerializeField] private float selectorRadius = 1f;

    [Header("Attachment")]
    [SerializeField] private float detachCooldownDuration = 1f;
    private bool attachable = true;
    private bool attached = false;
    [SerializeField] float limbBreakForce = Mathf.Infinity;
    private BodyJointBehaviour attachmentPoint;

    private MainBody body;
    private FixedJoint2D fixedJoint;
    private Rigidbody2D rb;
    private SpringJoint2D springJoint;

    // Start is called before the first frame update
    void Start()
    {
        fixedJoint = GetComponent<FixedJoint2D>();
        springJoint = GetComponent<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        body = FindObjectOfType<MainBody>();
        targetJoint = target.GetComponent<SpringJoint2D>();
        targetRb = target.GetComponent<Rigidbody2D>();

        DetachArm();
        //DeselectArm();
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.transform.position = mousePos;

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

            // Stick to body
            attachmentPoint = bodyPoint;
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

        // make lose of body
        attachmentPoint = null;
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
        // make target a non-physcics object
        targetRb.velocity = Vector2.zero;
        targetRb.bodyType = RigidbodyType2D.Kinematic;
        // enable target joint to arm to make arm depend on target
        targetJoint.enabled = true;
        targetJoint.connectedBody = rb;
        // disable arm joint to target
        springJoint.enabled = false;

        Debug.Log("Arm selected");
    }

    void DeselectArm()
    {
        selected = false;
        // make target a physics object
        target.transform.position = handSelector.position;
        targetRb.bodyType = RigidbodyType2D.Dynamic;
        // disable target joint to arm
        targetJoint.enabled = false;
        // enable arm joint to target to make target dependable on arm
        springJoint.enabled = true;
        springJoint.connectedBody = targetRb;

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
}
