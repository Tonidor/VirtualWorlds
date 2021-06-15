using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RelationshipSettings", menuName = "Relationship/RelationshipSettings")]
public class RalationshipSettings : ScriptableObject
{
    [Serializable]
    public struct Relationship
    {
        public BaseCreature subject;
        public RelationshipType type;
        public GameObject target;
    }

    public List<Relationship> relationships;
}
