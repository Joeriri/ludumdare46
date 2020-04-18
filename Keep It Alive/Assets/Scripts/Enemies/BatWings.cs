using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatWings : MonoBehaviour
{
    [SerializeField] float flapFrequency;
    [SerializeField] float flapMaxAngle;
    [SerializeField] float angleOffset;
    [SerializeField] bool wingIsSnapped;

    private BatBehaviour parentBatBehaviour;
    public bool animCancel = false;

    // Update is called once per frame
    void Update()
    {
        parentBatBehaviour = GetComponentInParent<BatBehaviour>();
        if (!animCancel) { GetComponent<Transform>().localEulerAngles = new Vector3(0, 0, angleOffset + ((Mathf.Sin(Time.time * flapFrequency) + 1) / 2) * flapMaxAngle); }

        if (wingIsSnapped)
        {
            WingSnap();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Weapon") { WingSnap(); }
    }

    void WingSnap()
    {
        animCancel = true;
        transform.parent = null;
        gameObject.AddComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>().AddForce(Random.Range(200f, 500f) * Random.insideUnitCircle);
        parentBatBehaviour.WingSnapped();
        Destroy(this);
    }
}
