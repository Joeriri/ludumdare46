using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] UnityEvent triggerEvent = new UnityEvent();

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.LogWarning("a thing collided");
        BodyPart tempPart = collider.GetComponent<BodyPart>();
        if (tempPart != null)
        {
            Debug.LogWarning("bodypart collided");
            if (tempPart.hasHeart)
            {
                Debug.LogWarning("it worked");
                triggerEvent.Invoke();
            }
        }
    }
}
