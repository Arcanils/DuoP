using System;
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

	private IEnumerator _routineSlow;
	private IEnumerator _routineSpeedup;

	public void SlowDown()
	{
		if (_routineSlow != null)
			return;
		if (_routineSpeedup != null)
			_routineSpeedup = null;

		_routineSlow = HelperTween.TimeScaleEnum(Time.timeScale, 0.05f, 0.15f, AnimationCurve.EaseInOut(0f, 0f, 1f, 1f), true);
	}

	public void SlowUp(Vector2 obj)
	{
		if (_routineSpeedup != null)
			return;
		if (_routineSlow != null)
			_routineSlow = null;

		_routineSpeedup = HelperTween.TimeScaleEnum(Time.timeScale, 1f, 0.15f, AnimationCurve.EaseInOut(0f, 0f, 1f, 1f), true);
	}

	public void Awake()
	{
		LaForceVeloce = this;
	}

	private void Update()
	{
		_currentTimer += Time.deltaTime;
		TargetUi.fillAmount = PowerChargeNormal;

		if (_routineSlow != null)
		{
			if (!_routineSlow.MoveNext())
				_routineSlow = null;
		}
		if (_routineSpeedup != null)
		{
			if (!_routineSpeedup.MoveNext())
				_routineSpeedup = null;
		}
	}

	public void ResetCooldown()
	{
		_currentTimer = -Cooldown;
	}

	public void ResetForceVeloce()
	{
		_routineSlow = null;
		_routineSpeedup = null;
		Time.timeScale = 1f;
	}
}
