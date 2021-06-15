using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemProperties", menuName = "Item/ItemProperties")]
public class ItemProperties : ScriptableObject {
    [Tooltip("maximum lifetime in sminutes")]
    public float maxLifetime;
    [Tooltip("chance that the item decays before it reaches its maximum lifetime")]
    public float chanceOfDecay;
    [Tooltip("the duratio of the process of decay takes until the item disappears")]
    public float durationOfDecay;
    [Tooltip("energy that the item provides when used")]
    public int energy;
}
