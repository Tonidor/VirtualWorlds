using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreatureSkills", menuName = "Creature/CreatureSkills")]
public class CreatureSkills : ScriptableObject
{
    [Header("Mental Skills")]
    public AnimationCurve sensitivityCurve;

    [Header("Physical Skills")]
    public float moveSpeed;
    public float rotationSpeed;
    public int perceptionRadius;

    [Header("Social Skills")]
    public int intimidation;
}
