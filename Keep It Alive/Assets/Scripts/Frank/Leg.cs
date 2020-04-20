using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    public Foot foot;
    public SpringJoint2D hingeJointLeg;
    private MainBody body;

    public BodyJointBehaviour attachmentPoint;

    private void Awake()
    {
        hingeJointLeg = gameObject.GetComponent<SpringJoint2D>();
        body = FindObjectOfType<MainBody>();
    }

    public void AttachLeg(BodyJointBehaviour bodyPoint)
    {
        attachmentPoint = bodyPoint;
        //attachmentPointJoint = attachmentPoint.GetComponent<SpringJoint2D>();

        transform.position = new Vector3(attachmentPoint.transform.position.x - 0.5f * transform.up.x, attachmentPoint.transform.position.y - 0.5f*transform.up.y, attachmentPoint.transform.position.z);

        hingeJointLeg.connectedBody = body.GetComponent<Rigidbody2D>();
        hingeJointLeg.connectedAnchor = new Vector2(body.transform.InverseTransformPoint(attachmentPoint.transform.position).x, body.transform.InverseTransformPoint(attachmentPoint.transform.position).y);
        transform.position = new Vector3(attachmentPoint.transform.position.x - 0.5f * transform.up.x, attachmentPoint.transform.position.y - 0.5f*transform.up.y, attachmentPoint.transform.position.z);
        hingeJointLeg.enabled = true;

        Transform[] _children = GetComponentsInChildren<Transform>();
        foreach (Transform child in _children)
        {
            child.gameObject.layer = 8;
        }
    }

    public void DetachLeg()
    {
        hingeJointLeg.enabled = false;
        hingeJointLeg.connectedBody = null;

        Transform[] _children = GetComponentsInChildren<Transform>();
        foreach (Transform child in _children)
        {
            child.gameObject.layer = 0;
        }
    }
}
