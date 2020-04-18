using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyJointBehaviour : MonoBehaviour
{
    [SerializeField] float attachRadius = 4;

    // Start is called before the first frame update
    void Start()
    {
        //Can't be bothered to mess with the radius of collider
        CircleCollider2D initCol = GetComponent<CircleCollider2D>();
        if (initCol != null) Destroy(initCol);
        //It's parented, so this just makes it into a nice scale
        attachRadius = attachRadius * 0.01f;

        //automatically set the collider radius to the right radius
        GetComponent<Transform>().localScale = new Vector3(attachRadius, attachRadius, 1);
        this.gameObject.AddComponent<CircleCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Limb")
        {
            Arm arm = collision.gameObject.GetComponent<Arm>();
            arm.AttachArm(gameObject);
            
            //Rigidbody2D rb = GetComponent<Rigidbody2D>();
            //if (rb == null) { rb = this.gameObject.AddComponent<Rigidbody2D>(); }
            //rb.isKinematic = true;
            //rb.constraints = RigidbodyConstraints2D.FreezePosition;

            //FixedJoint2D cj = this.gameObject.AddComponent<FixedJoint2D>();
            //Rigidbody2D CBrb = collision.gameObject.GetComponent<Rigidbody2D>();
            //if (CBrb == null) { CBrb = collision.gameObject.AddComponent<Rigidbody2D>(); }

            //CBrb.velocity = Vector3.zero;
            ////CBrb.constraints = RigidbodyConstraints2D.FreezePosition;

            //cj.connectedBody = CBrb;
            //collision.gameObject.layer = 8;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
