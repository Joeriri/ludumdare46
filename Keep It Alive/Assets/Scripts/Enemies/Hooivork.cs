using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hooivork : MonoBehaviour
{
    private bool startedDestroy = false;
    [SerializeField] private GameObject[] attachmentPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.tag = "Enemy";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //damage, mag jij doen Joeri
            for (int i = 0; i < attachmentPoints.Length; i++)
            {
                attachmentPoints[i].GetComponent<EnemyArm>().Weapon = null;
                attachmentPoints[i].GetComponent<EnemyArm>().attachPoint = null;
                attachmentPoints[i].GetComponent<FixedJoint2D>().connectedBody = null;
                attachmentPoints[i].GetComponent<FixedJoint2D>().enabled = false;
            }

            GetComponent<SpringJoint2D>().enabled = false;
            GetComponent<DistanceJoint2D>().enabled = false;
            GetComponent<BoxCollider2D>().offset = new Vector2(-0.42798f, -0.003289f);
            GetComponent<BoxCollider2D>().size = new Vector2(4.894349f, 0.9144681f);
            transform.SetParent(null);
            if (!startedDestroy)
            {
                GetComponent<DestroyGO>().StartDestroyThisSpritedGO();
                startedDestroy = true;
            }
        }
    }
}
