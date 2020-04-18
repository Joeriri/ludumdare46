using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    [SerializeField] private Transform footSelector;
    [SerializeField] private float selectorRadius = 1f;
    [SerializeField] private Leg leg;

    [SerializeField] private float detachCooldownDuration = 1f;
    [SerializeField] private float limbBreakForce = Mathf.Infinity;

    [SerializeField] private float springFreq = 1;
    [SerializeField] private float springDistance = 1;

    private bool attachedToMouse = false;
    private bool attachable = true;
    private bool attached = false;
    private bool selected;
    private Rigidbody2D attachmentPointRb;
    private SpringJoint2D attachmentPointJoint;

    public BodyJointBehaviour attachmentPoint;
    private MainBody body;

    private HingeJoint2D hingeJointFoot;

    // Start is called before the first frame update
    void Start()
    {
        hingeJointFoot = gameObject.GetComponent<HingeJoint2D>();
        body = FindObjectOfType<MainBody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(mousePos, footSelector.position) < selectorRadius)
            {
                SelectFoot();
            }
        }

        if (selected)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;

            // arm break
            if (leg.hingeJointLeg.reactionForce.magnitude > limbBreakForce)
            {
                DetachFoot();
                Debug.Log("Arm broke off!");
            }


            // Deselect the arm
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                DeselectFoot();
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            DeselectFoot();
        }

        if (attachedToMouse)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }

        //if(attachmentPoint != null)
        //{
        //    if (Vector3.Distance(attachmentPoint.gameObject.transform.position, transform.position) >= springDistance - 0.2f)
        //    {
        //        attachmentPointJoint.frequency = springFreq;
        //        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        //        Debug.Log("Ayy");
        //    }
        //}
    }

    void SelectFoot()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) { rb = gameObject.AddComponent<Rigidbody2D>(); }

        rb.isKinematic = true;

        selected = true;
        if (attached)
        {
            // make arm independent from body
            //attachmentPointJoint.enabled = false;
            //attachmentPointJoint.connectedBody = null;
        }

        Debug.Log("Foot selected");
    }

    void DeselectFoot()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        selected = false;
        if (attached)
        {
            attachmentPointJoint.enabled = true;
            attachmentPointJoint.connectedBody = body.GetComponent<Rigidbody2D>();

            // make joint from attachmentpoint to target
            //attachmentPointJoint.enabled = true;
            //attachmentPointJoint.connectedBody = rb;
            //rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (rb == null) { rb = gameObject.AddComponent<Rigidbody2D>(); }
        rb.isKinematic = false;
        attachedToMouse = false;

        Debug.Log("Leg deselected");
    }

    public void AttachLeg(BodyJointBehaviour bodyPoint)
    {
        if (attachable && !attached)
        {
            leg.AttachLeg(bodyPoint);
            attached = true;
            attachable = false;

            attachmentPoint = bodyPoint;
            attachmentPointRb = attachmentPoint.GetComponent<Rigidbody2D>();
            attachmentPointJoint = GetComponent<SpringJoint2D>();
            //attachmentPointJoint = attachmentPoint.GetComponent<SpringJoint2D>();

            attachmentPointJoint.autoConfigureDistance = false;
            attachmentPointJoint.connectedAnchor = new Vector2 (body.transform.InverseTransformPoint(bodyPoint.transform.position).x, body.transform.InverseTransformPoint(bodyPoint.transform.position).y);
            //attachmentPointJoint.autoConfigureConnectedAnchor = true;
            attachmentPointJoint.distance = springDistance;
            attachmentPointJoint.frequency = springFreq;

            //attachmentPointJoint.autoConfigureDistance = false;
            //attachmentPointJoint.distance = springDistance;
            //attachmentPointJoint.frequency = springFreq*6;

            //transform.position = attachmentPoint.transform.position;
            //hingeJoint.enabled = true;
            //hingeJoint.connectedBody = body.GetComponent<Rigidbody2D>();
            // Move layers to avoid nasty collisions
            Transform[] _children = GetComponentsInChildren<Transform>();
            foreach (Transform child in _children)
            {
                child.gameObject.layer = 8;
            }

            Debug.Log("Foot Attached");
        }
    }

    public void DetachFoot()
    {
        leg.DetachLeg();

        attached = false;
        attachable = false;

        //attachmentPointJoint.autoConfigureDistance = true;
        //attachmentPointJoint.frequency = 1;

        attachmentPoint = null;
        if (attachmentPointJoint.connectedBody != null) { attachmentPointJoint.connectedBody = null; }
        attachmentPointJoint = null;



        //fixedJoint.enabled = false;
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

    IEnumerator DetachCooldown()
    {
        yield return new WaitForSeconds(detachCooldownDuration);
        attachable = true;
        //Debug.Log("Yes");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(footSelector.position, selectorRadius);
    }
}
