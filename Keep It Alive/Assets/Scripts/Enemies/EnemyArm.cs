using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArm : MonoBehaviour
{
    public GameObject Weapon;
    public GameObject attachPoint;
    private FixedJoint2D fixedJoint;
    // Start is called before the first frame update
    void Start()
    {
        fixedJoint = GetComponent<FixedJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Weapon != null)
        {
            fixedJoint.enabled = true;
            fixedJoint.autoConfigureConnectedAnchor = false;
            //springJoint.autoConfigureDistance = false;
            fixedJoint.connectedBody = Weapon.GetComponent<Rigidbody2D>();
            //springJoint.distance = springDistance;
            fixedJoint.connectedAnchor = new Vector2(Weapon.transform.InverseTransformPoint(attachPoint.transform.position).x, Weapon.transform.InverseTransformPoint(attachPoint.transform.position).y);
        }
    }
}
