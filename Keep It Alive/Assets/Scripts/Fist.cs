using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour
{
    public bool fistCooldown = false;
    [SerializeField] Arm_2 parentArm;

    // Update is called once per frame
    void Update()
    {
        if (!fistCooldown && parentArm.attached == true)
        {
            GetComponent<CircleCollider2D>().enabled = true;
        }
        else
        {
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    public IEnumerator FistCooldownIE(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        fistCooldown = false;
    }
}
