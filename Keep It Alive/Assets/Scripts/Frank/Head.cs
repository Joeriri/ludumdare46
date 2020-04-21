using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [Header("Needed")]
    [SerializeField] private Transform headSelector;
    [SerializeField] private float selectorRadius = 1f;
    [SerializeField] private float hingeBreakForce = Mathf.Infinity;

    [HideInInspector] public BodyJointBehaviour attachmentPoint = null;

    private bool attached = false;
    private bool attachable = true;
    private bool selected;
    private Vector2 mouseOffset;
    private Vector2 selectorOffset;
    private HingeJoint2D attachmentPointJoint;
    private MainBody body;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        attachmentPointJoint = GetComponent<HingeJoint2D>();
        body = FindObjectOfType<MainBody>();
        selectorOffset = new Vector2(headSelector.localPosition.x, headSelector.localPosition.y);
        attachmentPointJoint.breakForce = hingeBreakForce;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(mousePos, headSelector.position) < selectorRadius && selected == false)
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
            transform.position = mousePos - selectorOffset + mouseOffset;

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                gm.bodyPartClicked = null;
                DeselectHead();
            }

            GetComponent<Collider2D>().isTrigger = true;
        }
        else
        {
            GetComponent<Collider2D>().isTrigger = false;
        }

        if (attachmentPointJoint != null)
        {
            if (attachmentPointJoint.enabled == true && attachmentPoint != null)
            {
                attachmentPointJoint.connectedAnchor = new Vector2(body.transform.InverseTransformPoint(attachmentPoint.transform.position).x, body.transform.InverseTransformPoint(attachmentPoint.transform.position).y);
            }
        }

    }

    private void LateUpdate()
    {
        if (gm.bodyPartClicked == this.gameObject && selected == false)
        {
            SelectHead();
        }
    }

    private void SelectHead()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selected = true;
        mouseOffset = new Vector2(headSelector.position.x, headSelector.position.y) - mousePos;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) { rb = gameObject.AddComponent<Rigidbody2D>(); }

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        GameManager.Instance.stopCamera();
    }

    private void DeselectHead()
    {
        selected = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (attached)
        {
            attachmentPointJoint.enabled = true;
            attachmentPointJoint.connectedBody = body.GetComponent<Rigidbody2D>();
        }

        if (rb == null) { rb = gameObject.AddComponent<Rigidbody2D>(); }
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.None;

        GameManager.Instance.ResumeCamera();
    }

    public void AttachHead(BodyJointBehaviour bodyPoint)
    {
        gm.EndGame();
        if (attachable && !attached)
        {
            attached = true;
            attachable = false;

            attachmentPoint = bodyPoint;

            //attachmentPointJoint fix
            transform.position = new Vector3(attachmentPoint.transform.position.x - 0.5f * transform.up.x, attachmentPoint.transform.position.y - 0.5f * transform.up.y, attachmentPoint.transform.position.z);
            
            attachmentPointJoint.connectedBody = body.GetComponent<Rigidbody2D>();
            attachmentPointJoint.connectedAnchor = new Vector2(body.transform.InverseTransformPoint(attachmentPoint.transform.position).x, body.transform.InverseTransformPoint(attachmentPoint.transform.position).y);
            transform.position = new Vector3(attachmentPoint.transform.position.x - 0.5f * transform.up.x, attachmentPoint.transform.position.y - 0.5f * transform.up.y, attachmentPoint.transform.position.z);
            attachmentPointJoint.enabled = true;

            Transform[] _children = GetComponentsInChildren<Transform>();
            foreach (Transform child in _children)
            {
                child.gameObject.layer = 8;
            }
        }
    }

    public void DetachHead()
    {
        attached = false;
        attachable = false;

        attachmentPointJoint.enabled = false;

        attachmentPoint = null;

        if (attachmentPointJoint.connectedBody != null) { attachmentPointJoint.connectedBody = null; }

        Transform[] _children = GetComponentsInChildren<Transform>();
        foreach (Transform child in _children)
        {
            child.gameObject.layer = 9;
        }

        string randomRipSound = "Rip_" + Random.Range(1, 3).ToString();
        AudioManager.instance.Play(randomRipSound);
        AudioManager.instance.ChangePitch(randomRipSound, Random.Range(0.7f, 1.3f));
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        if (joint == attachmentPointJoint)
        {
            DetachHead();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(headSelector.position, selectorRadius);
    }
}
