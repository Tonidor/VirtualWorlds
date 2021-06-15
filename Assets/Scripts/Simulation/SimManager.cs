using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SimManager : MonoBehaviour {
	[SerializeField] private MapComponent map;
	[SerializeField, Expandable] private SimulationSettings simulationSettings;
	public GameObject creatures;
	public GameObject items;

	void Start() {
		CreateCreatures();
		SpawnItems();
	}

	private void CreateCreatures() {
		creatures = Instantiate(new GameObject("creatures"));

		foreach (CreatureSpawn creatureSpawn in simulationSettings.creatureSpawnSettings) {
			for (int i = 0; i < creatureSpawn.numCreatures; i++) {
				Vector3 pos = map.GetRandomPosition();
				Quaternion rot = new Quaternion();
				rot.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f);

				BaseCreature v = Instantiate(creatureSpawn.creaturePrefab, pos, rot).GetComponent<BaseCreature>();
				v.transform.parent = creatures.transform;
				v.name = creatureSpawn.name + "." + i;
				v.Init(map, creatureSpawn.creatureSkills);
			}
		}
	}

	private void SpawnItems() {
		items = Instantiate(new GameObject("items"));

		foreach (ItemSpawn itemSpawn in simulationSettings.itemSpawnSettings) {
			for (int i = 0; i < itemSpawn.numItems; i++) {
				Vector3 pos = map.GetRandomPosition();
				Quaternion rot = new Quaternion();
				rot.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f);

				BaseItem v = Instantiate(itemSpawn.itemPrefab, pos, rot).GetComponent<BaseItem>();
				v.transform.parent = items.transform;
				v.name = itemSpawn.name + "." + i;
				v.Init(map, itemSpawn.itemProperties);
			}
		}
	}
}