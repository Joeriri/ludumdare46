using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBody : MonoBehaviour
{

    public List<Arm_2> attachedArms = new List<Arm_2>();
    public List<Foot> attachedLegs = new List<Foot>();

    [Header("Initial")]
    [SerializeField] float initialArmBreak = 1;
    [SerializeField] float initialLegBreak = 1;
    //[SerializeField] float initialFootBreak = Mathf.Infinity;

    [Header("Added")]
    [SerializeField] float addedArmBreak = 1;
    [SerializeField] float addedLegBreak = 1;
    //[SerializeField] float addedFootBreak = 1;

    [Header("Multiplier")]
    [SerializeField] float armMultiplier = 1;
    [SerializeField] float legMultiplier = 1;

    [HideInInspector] public float armBreak = 1;
    [HideInInspector] public float legBreak = 1;
    [HideInInspector] public float footBreak = 1;

    // Update is called once per frame
    void Update()
    {
        armBreak = initialArmBreak + (addedArmBreak * attachedArms.Count * armMultiplier) + (addedArmBreak * attachedLegs.Count * legMultiplier);
        legBreak = initialLegBreak + (addedLegBreak * attachedArms.Count * armMultiplier) + (addedLegBreak * attachedLegs.Count * legMultiplier);
        footBreak = Mathf.Infinity;
    }
}
