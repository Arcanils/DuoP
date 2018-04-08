using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePawnComponent : MonoBehaviour {

	public float ShootForce;
	public GameObject FxDestroy;

	public Action OnDeath;
	private Rigidbody2D _pawnRigid2D;

	private void Reset()
	{
		ShootForce = 40f;
	}
	public void Awake()
	{
		_pawnRigid2D = GetComponent<Rigidbody2D>();
	}

	private void OnDestroy()
	{
		GameObject.Instantiate(FxDestroy, transform.position, Quaternion.identity);
		if (OnDeath != null)
			OnDeath();
	}

	public void Fire(Vector2 aim)
	{
		if (aim == Vector2.zero || !PropulsionCooldown.LaForceVeloce.HasPowerMax)
			return;
		aim.Normalize();
		PropulsionCooldown.LaForceVeloce.ResetCooldown();
		_pawnRigid2D.velocity = Vector2.zero;
		_pawnRigid2D.AddForce(aim * ShootForce, ForceMode2D.Impulse);
	}
}
