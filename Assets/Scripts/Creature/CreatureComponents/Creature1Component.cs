using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature1Component : BaseCreature
{
    private Transform sensor;
    private Color sensorColor;
    private float temperature;
    private Transform wheel;
    [Range(-1.0f, 1.0f)]
    private float wheelVelocity = 0.0f;


    public override void Init(MapComponent map, CreatureSkills creatureSkills) {
        base.Init(map, creatureSkills);
        sensor = transform.Find("Body").Find("SensorArm").Find("Sensor");
        wheel = transform.Find("Body").Find("Wheel");
    }

    protected override void Sense() {
        base.Sense();
        temperature = map.GetTemperature(sensor.position);
    }

    protected override void Move() {
        wheelVelocity = creatureSkills.moveSpeed * creatureSkills.sensitivityCurve.Evaluate(temperature);
        transform.Translate(0, 0, wheelVelocity * Time.deltaTime, Space.Self);
    }

    protected override void Evolve() {
        base.Evolve();
        sensorColor.r = temperature;
        sensor.gameObject.GetComponent<Renderer>().material.color = sensorColor;
    }
}
