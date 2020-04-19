﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] private bool hasHeart = false;
    private BodyJointBehaviour attachmentPointChild;

    // Start is called before the first frame update
    void Start()
    {
        attachmentPointChild = GetComponentInChildren<BodyJointBehaviour>();
    }

    public void DestroyBodyPart()
    {
        if (attachmentPointChild != null)
        {
            // If attachmentPoint has attached arm or leg, de-attach them
            if (attachmentPointChild.attachedArm != null)
            {
                attachmentPointChild.attachedArm.DetachArm();
            }
            if (attachmentPointChild.attachedFoot != null)
            {
                attachmentPointChild.attachedFoot.DetachFoot();
            }
        }

        if (hasHeart)
        {
            GameManager.Instance.FrankDie();
        }

        Debug.Log("Body part " + gameObject.name + " was destroyed!");
        Destroy(gameObject);
    }
}
