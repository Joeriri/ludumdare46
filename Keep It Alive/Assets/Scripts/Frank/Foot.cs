﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    [Header("Needed")]
    public Leg leg;
    [SerializeField] private Transform footSelector;
    [SerializeField] private float selectorRadius = 1f;

    [Header("Spring")]
    [SerializeField] private float springFreq = 1;
    [SerializeField] private float springDistance = 1;

    [Header("Cooldowns")]
    [SerializeField] private float attachCooldownTimer = 0;
    [SerializeField] private float detachCooldownDuration = 1f;

    [Header("Attach on start")]
    [SerializeField] private bool attachOnStart = false;
    [SerializeField] private BodyJointBehaviour startJoint;

    [HideInInspector] public BodyJointBehaviour attachmentPoint = null;


    private bool attachable = true;
    private bool attached = false;
    private bool attachCooldown = false;
    private bool selected;
    private Vector2 mouseOffset;
    private Vector2 selectorOffset;
    private Rigidbody2D attachmentPointRb;
    private SpringJoint2D attachmentPointJoint;
    private MainBody body;
    private HingeJoint2D hingeJointFoot;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        //attachmentPointJoint = GetComponent<SpringJoint2D>();
        gm = FindObjectOfType<GameManager>();
        attachmentPointJoint = GetComponent<SpringJoint2D>();
        hingeJointFoot = gameObject.GetComponent<HingeJoint2D>();
        body = FindObjectOfType<MainBody>();
        selectorOffset = new Vector2(footSelector.localPosition.x, footSelector.localPosition.y);
        DeselectFoot();

        if (attachOnStart)
        {
            if (startJoint != null)
            {
                AttachLeg(startJoint);
                startJoint.attachedFoot = this;
                startJoint.occupied = true;
                DeselectFoot();
            }
            else
            {
                Debug.LogError("Trying to attach a Foot on start, but no startJoint given!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpringJoint2D>().breakForce = body.footBreak;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(mousePos, footSelector.position) < selectorRadius && selected == false)
            {
                if (gm.bodyPartClicked == null)
                {
                    gm.bodyPartClicked = this.gameObject;
                }
            }
        }

        if (selected)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos - selectorOffset + mouseOffset ;

            // arm break
            if (attachmentPointJoint.enabled && attachmentPointJoint.connectedBody == body.GetComponent<Rigidbody2D>())
            {
                //Debug.LogWarning("Yes");
                if (leg.hingeJointLeg.reactionForce.magnitude > body.legBreak && attachCooldown == false)
                {
                    DetachFoot();
                }
            }
            else
            {
                //Debug.LogWarning("No");
                if (leg.hingeJointLeg.reactionForce.magnitude > body.legBreakWhileSpringNotActive && attachCooldown == false)
                {
                    DetachFoot();
                }
            }            

            // Deselect the arm
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                gm.bodyPartClicked = null;
                DeselectFoot();
            }

            //make leg collider trigger
            leg.GetComponent<Collider2D>().isTrigger = true;
        }
        else
        {
            leg.GetComponent<Collider2D>().isTrigger = false;
        }

        if(attachmentPointJoint != null)
        {
            if (attachmentPointJoint.enabled == true && attachmentPoint != null)
            {
                attachmentPointJoint.connectedAnchor = new Vector2(body.transform.InverseTransformPoint(attachmentPoint.transform.position).x, body.transform.InverseTransformPoint(attachmentPoint.transform.position).y);
            }
        }

        if(GetComponent<SpringJoint2D>() == null)
        {
            AddSpringJoint2D();
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

    private void LateUpdate()
    {
        if (gm.bodyPartClicked == this.gameObject && selected == false)
        {
            SelectFoot();
        }
    }

    void SelectFoot()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selected = true;
        mouseOffset = new Vector2(footSelector.position.x, footSelector.position.y) - mousePos;

        // Add a rigidbody if there is none here
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) { rb = gameObject.AddComponent<Rigidbody2D>(); }
        // make foot non-physics
        rb.isKinematic = true;

        // stop camera movement
        GameManager.Instance.stopCamera();

        Debug.Log("Foot selected");
    }

    void DeselectFoot()
    {
        selected = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

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
        //attachedToMouse = false;

        // stop camera movement
        GameManager.Instance.ResumeCamera();

        Debug.Log("Foot deselected");
    }

    public void AttachLeg(BodyJointBehaviour bodyPoint)
    {
        if (attachable && !attached)
        {
            leg.AttachLeg(bodyPoint);
            attached = true;
            attachable = false;

            body.attachedLegs.Add(this);
            attachmentPoint = bodyPoint;
            attachmentPointRb = attachmentPoint.GetComponent<Rigidbody2D>();
            //attachmentPointJoint = GetComponent<SpringJoint2D>();
            //attachmentPointJoint = attachmentPoint.GetComponent<SpringJoint2D>();

            attachmentPointJoint.autoConfigureDistance = false;
            //attachmentPointJoint.connectedAnchor = new Vector2 (body.transform.InverseTransformPoint(bodyPoint.transform.position).x, body.transform.InverseTransformPoint(bodyPoint.transform.position).y);
            //attachmentPointJoint.autoConfigureConnectedAnchor = true;
            attachmentPointJoint.distance = springDistance;
            attachmentPointJoint.frequency = springFreq;

            attachCooldown = true;
            StartCoroutine(AttachCooldown());
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

            // play sound
            if (selected)
            {
                string randomAttachSound = "Attach_" + Random.Range(1, 4).ToString();
                AudioManager.instance.Play(randomAttachSound);
                AudioManager.instance.ChangePitch(randomAttachSound, Random.Range(0.7f, 1.3f));
            }

            // if first limb to attach, cue the music
            if (!GameManager.Instance.hasAttachedFirstLimb && selected)
            {
                GameManager.Instance.AttachedFirstLimb();
            }

            Debug.Log("Foot Attached");
        }
    }

    public void DetachFoot()
    {
        leg.DetachLeg();

        attached = false;
        attachable = false;

        body.attachedLegs.Remove(this);
        attachmentPointJoint.enabled = false;

        //attachmentPointJoint.autoConfigureDistance = true;
        //attachmentPointJoint.frequency = 1;

        attachmentPoint = null;
        if (attachmentPointJoint.connectedBody != null) { attachmentPointJoint.connectedBody = null; }
        //attachmentPointJoint = null;

        //fixedJoint.enabled = false;
        // Put arm back on default layer
        Transform[] _children = GetComponentsInChildren<Transform>();
        foreach (Transform child in _children)
        {
            child.gameObject.layer = 9;
        }
        // start attach cooldown to prevent instant re attachment.
        StartCoroutine(DetachCooldown());

        // play sound
        string randomRipSound = "Rip_" + Random.Range(1, 3).ToString();
        AudioManager.instance.Play(randomRipSound);
        AudioManager.instance.ChangePitch(randomRipSound, Random.Range(0.7f, 1.3f));

        Debug.Log("Foot detached");
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        if( joint == attachmentPointJoint)
        {
            //AddSpringJoint2D();
            DetachFoot();
        }
    }

    private void AddSpringJoint2D()
    {
        SpringJoint2D dikkeJoint = GetComponent<SpringJoint2D>();
        if (dikkeJoint == null) { dikkeJoint = gameObject.AddComponent<SpringJoint2D>(); }
        dikkeJoint.enabled = false;
        dikkeJoint.breakForce = body.footBreak;
        attachmentPointJoint = dikkeJoint;
    }

    IEnumerator DetachCooldown()
    {
        yield return new WaitForSeconds(detachCooldownDuration);
        attachable = true;
        //Debug.Log("Yes");
    }

    IEnumerator AttachCooldown()
    {
        yield return new WaitForSeconds(attachCooldownTimer);
        attachCooldown = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(footSelector.position, selectorRadius);
    }
}
