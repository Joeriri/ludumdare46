using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyJointBehaviour : MonoBehaviour
{
    [SerializeField] float attachRadius = 4;

    public bool occupied = false;
    public Arm_2 attachedArm;
    [HideInInspector] public Foot attachedFoot;
    [HideInInspector] public Head attachedHead;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Limb" && !occupied)
        {
            Arm_2 arm = collision.gameObject.GetComponent<Arm_2>();
            Leg leg = collision.gameObject.GetComponent<Leg>();
            Head head = collision.gameObject.GetComponent<Head>();

            if (arm == null && head == null)
            { 
                Foot foot = leg.foot;
                //leg.AttachLeg(this);
                foot.AttachLeg(this);
                attachedFoot = foot;
                occupied = true;
            }
            else if(leg == null && arm == null)
            {
                Debug.LogWarning("Ayy");
                head.AttachHead(this);
                attachedHead = head;
                occupied = true;
            }
            else
            {
                arm.AttachArm(this);
                attachedArm = arm;
                occupied = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Limb" && !occupied)
        {
            Arm_2 arm = collision.gameObject.GetComponent<Arm_2>();
            Leg leg = collision.gameObject.GetComponent<Leg>();
            Head head = collision.gameObject.GetComponent<Head>();

            if (arm == null && head == null)
            {
                Foot foot = leg.foot;
                //leg.AttachLeg(this);
                foot.AttachLeg(this);
                attachedFoot = foot;
                occupied = true;
            }
            else if (leg == null && arm == null)
            {
                Debug.LogWarning("Ayy");
                head.AttachHead(this);
                attachedHead = head;
                occupied = true;
            }
            else
            {
                arm.AttachArm(this);
                attachedArm = arm;
                occupied = true;
            }
        }
    }

    private void Update()
    {
        // can't reset these inside Arm, so we check here if arm says it has detached, and then we do it here as well.
        if (attachedArm != null && attachedArm.attachmentPoint == null)
        {
            attachedArm = null;
            occupied = false;
        }
        if (attachedFoot != null && attachedFoot.attachmentPoint == null)
        {
            attachedFoot = null;
            occupied = false;
        }
        if (attachedHead != null && attachedHead.attachmentPoint == null)
        {
            attachedHead = null;
            occupied = false;
        }
    }
}
