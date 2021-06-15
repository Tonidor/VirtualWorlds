using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCreature : MonoBehaviour {
	public MapComponent map;

	[SerializeField, Expandable] protected CreatureSkills creatureSkills;

	[SerializeField] protected Vector3 targetPosition;
	[SerializeField] protected bool targetReached;
	[SerializeField] protected Vector3 currentGradient;
	[SerializeField] protected int stepsBeforeGradientCheck = 20;

	protected float wheelRadius = .5f;
	protected float wheelBase = .3f;
	protected int currentStep = 0;

	private Vector3 r, f, u;
	private Quaternion q;

	public virtual void Init(MapComponent map, CreatureSkills creatureSkills) {
		this.map = map;
		this.creatureSkills = creatureSkills;
	}

	void Update() {
		Sense();
		Move();
		Interact();
		Evolve();

		MapLock();
	}

	private void MapLock() {
		if (!map.isInsideBounds(transform.position)) {
			transform.position = new Vector3(
				Random.Range(map.mapBounds.min.x, map.mapBounds.max.x),
				Random.Range(map.mapBounds.min.x, map.mapBounds.max.x),
				Random.Range(map.mapBounds.min.z, map.mapBounds.max.z));
		}
		transform.position = map.GetMeshPosition(transform);
		CalculateGradient();
		SetLookRotation();
	}

	private void SetLookRotation() {
		r = transform.position + transform.right;
		f = transform.position + transform.forward;
		r.y = map.GetMeshHeight(r);
		f.y = map.GetMeshHeight(f);
		r -= transform.position;
		f -= transform.position;
		r.Normalize();
		f.Normalize();
        u = Vector3.Cross(f, r);
		u.Normalize();
		q = new Quaternion();
		q.SetLookRotation(f, u);
		transform.rotation = q;
		Debug.DrawLine(transform.position, transform.position + transform.forward * 3f, Color.blue);
	}

	private void CalculateGradient() {
		if (currentStep > stepsBeforeGradientCheck) {
			currentGradient = map.GetGradient(transform.position);
			currentGradient.y = map.GetMeshHeight(currentGradient) / map.mapSettings.meshHeightMultiplier;
			currentStep = 0;
		}
		currentStep++;
	}

	protected virtual void Move() { }

	protected virtual void Interact() { }

	protected virtual void Sense() { }

	protected virtual void Evolve() { }

	protected virtual void GetMovementParameters() {
		targetPosition = !targetReached ? targetPosition : map.GetRandomPositionWithinRadius(transform.position, creatureSkills.perceptionRadius);
		Vector3 moveDirection = (transform.position + targetPosition).normalized;
	}
}
