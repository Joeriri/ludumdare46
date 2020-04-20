using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] UnityEvent triggerEvent = new UnityEvent();

    private void OnTriggerEnter2D(Collider2D collider)
    {
        BodyPart tempPart = collider.GetComponent<BodyPart>();
        if (tempPart != null)
        {
            if (tempPart.hasHeart)
            {
                triggerEvent.Invoke();
            }
        }
    }
}
