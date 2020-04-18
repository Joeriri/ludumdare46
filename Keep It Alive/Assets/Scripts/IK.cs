using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK : MonoBehaviour
{
    // Source for all IK stuff: https://www.alanzucconi.com/2018/05/02/ik-2d-2/

    [SerializeField] private Transform jointA;
    [SerializeField] private Transform jointB;
    [SerializeField] private Transform hand;
    [SerializeField] private Transform target;

    private float lengthA;
    private float lengthB;

    // Start is called before the first frame update
    void Start()
    {
        lengthA = Vector2.Distance(jointA.position, jointB.position);
        lengthB = Vector2.Distance(jointB.position, hand.position);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateIK();
    }

    void UpdateIK()
    {
        float jointAngleA;
        float jointAngleB;

        float lengthTar = Vector2.Distance(jointA.position, target.position);

        Vector2 diff = target.position - jointA.position;
        float atan = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        if (lengthA + lengthB < lengthTar)
        {
            jointAngleA = atan;
            jointAngleB = 0f;
        }
        else
        {
            float cosAngle0 = ((lengthTar * lengthTar) + (lengthA * lengthA) - (lengthB * lengthB)) / (2 * lengthTar * lengthA);
            float angle0 = Mathf.Acos(cosAngle0) * Mathf.Rad2Deg;

            float cosAngle1 = ((lengthB * lengthB) + (lengthA * lengthA) - (lengthTar * lengthTar)) / (2 * lengthB * lengthA);
            float angle1 = Mathf.Acos(cosAngle1) * Mathf.Rad2Deg;

            jointAngleA = atan - angle0;
            jointAngleB = 180f - angle1;
        }

        jointA.transform.rotation = Quaternion.Euler(0, 0, jointAngleA);
        jointB.transform.localRotation = Quaternion.Euler(0, 0, jointAngleB);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(jointA.position, jointB.position);
        Gizmos.DrawLine(jointB.position, hand.position);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(target.position, 0.1f);

    }
}
