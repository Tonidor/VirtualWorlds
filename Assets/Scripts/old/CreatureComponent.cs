using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreatureComponent : MonoBehaviour
{
    [SerializeField, Expandable]
    private CreatureType type;

    #region skills
    [Header("Mental Skills")]
    [SerializeField]
    private int sensitivity;

    [Header("Physical Skills")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    public float rotationSpeed;

    [Header("Social Skills")]
    [SerializeField]
    private int intimidation;
    #endregion

    [SerializeField]
    private int age;
    [SerializeField]
    private int health;

    private SimManager sim;
    private Quaternion toRotation;
    private float temperature;

    void Awake() {
        toRotation = Quaternion.AngleAxis(Random.Range(-180.0f, 180.0f), Vector3.up);
    }

    void Start() {
       // sim = SimManager.Get();
    }

    void Update()
    {
        MapLock();

        Move();
        Interact();
        Sense();
        Evolve();
    }

    public void Init(CreatureSkills creatureSkills) {
        ////sensitivity = creatureSkills.sensitivityCurve;
        moveSpeed = creatureSkills.moveSpeed;
    }

    private void MapLock() {
        //if (!sim.map.isInsideBounds(transform.position)) {
        //    transform.position = new Vector3(Random.Range(sim.map.mapBounds.min.x, sim.map.mapBounds.max.x),
        //        Random.Range(sim.map.mapBounds.min.x, sim.map.mapBounds.max.x),
        //        Random.Range(sim.map.mapBounds.min.z, sim.map.mapBounds.max.z));
        //}

        //transform.position = sim.map.GetMeshPosition(transform);
    }

    protected virtual void Move() {
        Translate();
        Rotate();
    }

    protected virtual void Interact() {

    }

    protected virtual void Sense() {
        SenseTemperature();
    }

    protected virtual void Evolve() {

    }

    void Translate() {
        transform.Translate(0, 0, moveSpeed);
    }

    void Rotate() {
        if (1 - Mathf.Abs(Quaternion.Dot(transform.rotation, toRotation)) < 0.1f) {
            toRotation = Quaternion.AngleAxis(Random.Range(-180.0f, 180.0f), Vector3.up);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    void SenseTemperature() {
        
    }
}
