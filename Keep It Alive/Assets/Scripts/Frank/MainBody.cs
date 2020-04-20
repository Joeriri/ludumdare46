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

    [Header("Spring Not Active")]
    [SerializeField] float springNotActiveSubtraction = 1;
    [SerializeField] float springNotActiveDivision = 1;

    [HideInInspector] public float armBreak = 1;
    [HideInInspector] public float legBreak = 1;
    [HideInInspector] public float legBreakWhileSpringNotActive = 1;
    [HideInInspector] public float footBreak = 1;

    //[Header("Debug")]
    //[SerializeField] float[] armStrain = new float[2];
    //[SerializeField] float[] legStrain = new float[2];


    //private void Start()
    //{
    //    armStrain = new float[2];
    //    legStrain = new float[2];
    //}
    // Update is called once per frame
    void Update()
    {
        armBreak = initialArmBreak + (addedArmBreak * attachedArms.Count * armMultiplier) + (addedArmBreak * attachedLegs.Count * legMultiplier);
        legBreak = initialLegBreak + (addedLegBreak * attachedArms.Count * armMultiplier) + (addedLegBreak * attachedLegs.Count * legMultiplier);
        legBreakWhileSpringNotActive = (initialLegBreak + (addedLegBreak * attachedArms.Count * armMultiplier) + (addedLegBreak * attachedLegs.Count * legMultiplier))/springNotActiveDivision - springNotActiveSubtraction;
        footBreak = Mathf.Infinity;

        //for (int i = 0; i < attachedArms.Count; i++)
        //{
        //    armStrain[i] = attachedArms[i].fixedJoint.reactionForce.magnitude;
        //}

        //for (int i = 0; i < attachedLegs.Count; i++)
        //{
        //    legStrain[i] = attachedLegs[i].leg.hingeJointLeg.reactionForce.magnitude;
        //}
    }
}
