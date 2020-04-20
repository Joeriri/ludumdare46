using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatWings : MonoBehaviour
{
    [SerializeField] float flapFrequency;
    [SerializeField] float flapMaxAngle;
    [SerializeField] float angleOffset;
    [SerializeField] bool wingIsSnapped;
    [SerializeField] int wingHp = 2;
    [SerializeField] float hitCooldownTime = 1;
    [SerializeField] float flyAwayTimer = 3;

    private BatBehaviour parentBatBehaviour;
    public bool animCancel = false;
    private bool hitCooldown = false;


    // Update is called once per frame
    void Update()
    {
        parentBatBehaviour = GetComponentInParent<BatBehaviour>();
        if (!animCancel) { GetComponent<Transform>().localEulerAngles = new Vector3(0, 0, angleOffset + ((Mathf.Sin(Time.time * flapFrequency) + 1) / 2) * flapMaxAngle); }

        if (wingHp <= 0)
        {
            WingSnap();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fist" && hitCooldown == false)
        {
            collision.GetComponent<Fist>().fistCooldown = true;
            collision.GetComponent<Fist>().StartCoroutine(collision.GetComponent<Fist>().FistCooldownIE(hitCooldownTime));
            hitCooldown = true;
            wingHp -= 1;
            parentBatBehaviour.StartCoroutine(parentBatBehaviour.GoAway(flyAwayTimer));
            StartCoroutine(OnHitCooldown(hitCooldownTime));
        }
    }

    void WingSnap()
    {
        animCancel = true;
        transform.parent = null;
        //gameObject.AddComponent<Rigidbody2D>();
        GetComponentInChildren<PolygonCollider2D>().isTrigger = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().AddForce(Random.Range(200f, 500f) * Random.insideUnitCircle);
        parentBatBehaviour.WingSnapped();
        Destroy(this);
    }

    private IEnumerator OnHitCooldown(float timer)
    {
        yield return new WaitForSeconds(timer);
        hitCooldown = false;
    }
}
