using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CreatureSpawn
{
    public string name;
    public CreatureSkills creatureSkills;
    public BaseCreature creaturePrefab;
    public int numCreatures;
}

[Serializable]
public struct ItemSpawn
{
    public string name;
    public ItemProperties itemProperties;
    public BaseItem itemPrefab;
    public int numItems;
}

[CreateAssetMenu(fileName = "SimulationSettings", menuName = "Simulation/SimulationSettings")]
public class SimulationSettings : ScriptableObject
{
    public List<CreatureSpawn> creatureSpawnSettings;
    public List<ItemSpawn> itemSpawnSettings;
}
