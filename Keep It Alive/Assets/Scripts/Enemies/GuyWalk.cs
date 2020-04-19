using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuyWalk : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private GameObject body;
    [SerializeField] private float stepForce;
    [SerializeField] private float stepWait;

    [SerializeField] private GameObject closedLeg;
    [SerializeField] private GameObject openLeg;

    private bool left = true;
    private bool newStep = true;
    private bool move = true;

    //private DistanceJoint2D distanceJoint;


    // Start is called before the first frame update
    void Start()
    {
        //distanceJoint = GetComponent<DistanceJoint2D>();
        //if (distanceJoint == null) { distanceJoint = gameObject.AddComponent<DistanceJoint2D>(); }
    }

    // Update is called once per frame
    void Update()
    {
        //distanceJoint.connectedAnchor = new Vector2(transform.position.x, transform.position.y);

        if (left && move == true)
        {
            transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;
        }
        else if(move == true)
        {
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
        }
        
        if(Mathf.RoundToInt(Mathf.Sin(transform.position.x*6)*100)/100 >= 0.97f && newStep == true)
        {
            StartCoroutine(stepCooldown());
            newStep = false;
        }

        if (Mathf.RoundToInt(Mathf.Sin(transform.position.x * 6) * 100) / 100 <= 0.97f && newStep == false)
        {
            newStep = true;
        }

    }

    IEnumerator stepCooldown()
    {
        move = false;
        closedLeg.SetActive(true);
        openLeg.SetActive(false);
        Debug.LogWarning("Step");
        if (body != null) { body.GetComponent<Rigidbody2D>().AddForce(stepForce * new Vector2(-1, -1)); }
        yield return new WaitForSeconds(stepWait);
        closedLeg.SetActive(false);
        openLeg.SetActive(true);
        move = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 10)
        {
            Destroy(this);
        }

    }
}
