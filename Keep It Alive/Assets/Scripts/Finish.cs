using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        BodyPart tempPart = collider.GetComponent<BodyPart>();
        if (tempPart != null)
        {
            if (tempPart.hasHeart)
            {
                GameManager.Instance.GoToNextLevel();
            }
        }
    }
}
