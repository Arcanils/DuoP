using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropulsionCooldown : MonoBehaviour
{
	public static PropulsionCooldown LaForceVeloce;
	public float PowerChargeNormal
	{
		get { return Mathf.Clamp01(1 + (_currentTimer / Cooldown)); }
	}

	public bool HasPowerMax
	{
		get { return _currentTimer >= 0f; }
	}

	public Image TargetUi;
	public float Cooldown;

	private float _currentTimer;

	public void Awake()
	{
		LaForceVeloce = this;
	}

	private void Update()
	{
		_currentTimer += Time.deltaTime;
		TargetUi.fillAmount = PowerChargeNormal;
	}

	public void ResetCooldown()
	{
		_currentTimer = -Cooldown;
	}
}
