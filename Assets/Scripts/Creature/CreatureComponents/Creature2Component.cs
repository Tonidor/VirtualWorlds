using System;
using UnityEngine;

public class Creature2Component : BaseCreature {
	private Transform sensorLeft;
	private Transform sensorRight;
	private Color sensorColorLeft;
	private Color sensorColorRight;
	private float temperatureLeft;
	private float temperatureRight;

	[SerializeField, Range(0f, .1f)] private float perturbation0 = 0.01f;
	[SerializeField, Range(0f, .1f)] private float perturbation1 = 1f;
	[SerializeField, Range(0.01f, 2f)] private float turnDelta;

	public override void Init(MapComponent map, CreatureSkills creatureSkills) {
		base.Init(map, creatureSkills);
		sensorLeft = transform.Find("Body").Find("SensorArm_left").Find("Sensor");
		sensorRight = transform.Find("Body").Find("SensorArm_right").Find("Sensor");
		sensorColorLeft = new Color(1, 0, 0, 1);
		sensorColorRight = new Color(1, 0, 0, 1);
	}

	protected override void Sense() {
		base.Sense();
		temperatureLeft = map.GetTemperature(sensorLeft.position);
		temperatureRight = map.GetTemperature(sensorRight.position);
	}

	protected override void Move() {
		float velocityLeft = creatureSkills.moveSpeed * creatureSkills.sensitivityCurve.Evaluate(temperatureLeft);
		float velocityRight = creatureSkills.moveSpeed * creatureSkills.sensitivityCurve.Evaluate(temperatureRight);

		//float perturbation = (Mathf.PerlinNoise(transform.position.z, transform.position.x) - 0.5f);
		//float p0 = perturbation0 * perturbation;
		//float p1 = perturbation1 * perturbation;
		float wheelVelocityLeft = velocityRight; //* (1f - p1) - p0;
		float wheelVelocityRight = velocityLeft; //* (1f + p1) + p0;

		if (wheelVelocityLeft != wheelVelocityRight) {
            float rotationRadius = 0.5f * (wheelVelocityLeft + wheelVelocityRight) / (wheelVelocityRight - wheelVelocityLeft);
            float rotationAngle = Mathf.Rad2Deg * (wheelVelocityRight - wheelVelocityLeft) / wheelBase;
            Vector3 rotationPivot = transform.position + transform.right * rotationRadius;
            transform.RotateAround(rotationPivot, transform.up, rotationAngle * creatureSkills.rotationSpeed * Time.deltaTime);
        }
		else 
		{
            transform.Translate(0, 0, wheelVelocityLeft * Time.deltaTime, Space.Self);
        }
	}

	protected override void Evolve() {
		base.Evolve();
		sensorColorLeft.r = temperatureLeft;
		sensorColorRight.r = temperatureRight;
		sensorLeft.gameObject.GetComponent<Renderer>().material.color = sensorColorLeft;
		sensorRight.gameObject.GetComponent<Renderer>().material.color = sensorColorRight;
	}
}