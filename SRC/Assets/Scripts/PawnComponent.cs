using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnComponent : MonoBehaviour {

	public float SpeedMove;

	public Vector2 LeftBorder;
	public Vector2 RightBorder;

	public Vector2 JumpForce;
	public LayerMask LayerWakeable;

	public float ShootForce;
	public PawnComponent TargetShoot;
	public float FireRate;

	public float DurationSlowMo;
	public AnimationCurve CurveSlowMo;
	public AnimationCurve CurveScale;

	public Rigidbody2D PawnRigid2D { get; private set; }

	public Action OnShootPawn;

	private Transform _trans;
	private Transform _graphBody;
	
	private Vector2 _deltaMove;
	private Vector2 _aimDir;
	
	private bool _inputJump;
	private bool _inputFire;
	private bool _isGrounded;


	private void Reset()
	{
		SpeedMove = 10f;
		JumpForce = new Vector2(10f, 0f);
		LeftBorder = new Vector2(0f, 0.5f);
		FireRate = 1f;
		DurationSlowMo = 0.5f;
	}

	private void Awake()
	{
		_trans = transform;
		PawnRigid2D = GetComponent<Rigidbody2D>();
		_graphBody = _trans.Find("Body");
		if (!_graphBody)
			_graphBody = _trans;
	}


	public void OnDrawGizmos()
	{
		if (_aimDir != Vector2.zero)
			Gizmos.DrawLine(_trans.position, _trans.position + new Vector3(_aimDir.x, _aimDir.y) * 2f);
	}


	private void FixedUpdate()
	{
		UpdateIsGrounded();
		JumpLogic();
		MoveLogic();
		FireLogic();
	}

	public void Init(PawnComponent target)
	{
		TargetShoot = target;
	}

	public void Move (float InputMove)
	{
		_deltaMove += new Vector2(InputMove * SpeedMove, 0f);
	}

	public void Jump (bool IsDown)
	{
		_inputJump = IsDown;
	}

	public void Aiming(Vector2 AimDir)
	{
		_aimDir = AimDir;
	}

	public void FirePlayer(bool IsDown)
	{
		_inputFire = IsDown;
	}

	private void JumpLogic()
	{

		if (!_isGrounded || !_inputJump)
			return;

		PawnRigid2D.AddForce(JumpForce, ForceMode2D.Impulse);
		
	}

	private void MoveLogic()
	{
		//PawnRigid2D.velocity = _deltaMove + new Vector2(0f, PawnRigid2D.velocity.y);
		_trans.position += new Vector3(_deltaMove.x, _deltaMove.y) * Time.fixedDeltaTime;
		_deltaMove.Set(0f, 0f);
	}

	private void UpdateIsGrounded()
	{
		var pos = new Vector2(_trans.position.x, _trans.position.y);
		var hitInfo = Physics2D.Raycast(LeftBorder + pos, Vector2.down, 0.2f, LayerWakeable);
		if (hitInfo.transform != null)
			_isGrounded = true;
		else
			_isGrounded = false;
	}

	private void FireLogic()
	{

		if (!_inputFire || _aimDir == Vector2.zero || !PropulsionCooldown.LaForceVeloce.HasPowerMax)
			return;
		
		_inputFire = false;
		PropulsionCooldown.LaForceVeloce.ResetCooldown();;

		//TargetShoot.MovePosition(new Vector2(_trans.position.x, _trans.position.y));

		StartCoroutine(FireSlowMo());
	}

	private IEnumerator ScaleEnum(bool toOne, float duration)
	{
		var beg = toOne ? Vector3.zero : Vector3.one;
		var end = !toOne ? Vector3.zero : Vector3.one;
		for (float t = 0f, perc = 0f; perc < 1f; t += Time.unscaledDeltaTime)
		{
			perc = Mathf.Clamp01(t / duration);
			_graphBody.localScale = Vector3.Lerp(beg, end, CurveScale.Evaluate(perc));
			yield return null;
		}
	}

	private IEnumerator FireSlowMo()
	{
		Time.timeScale = 0f;
		//Move & scale
		StartCoroutine(HelperTween.MoveTransformEnum(TargetShoot.transform, TargetShoot.transform.position, _trans.position,
			0.3f, AnimationCurve.EaseInOut(0f, 0f, 1f, 1f)));
		yield return HelperTween.ScaleEnum(TargetShoot.transform, TargetShoot.transform.localScale, Vector3.zero, 0.3f, AnimationCurve.EaseInOut(0f, 0f, 1f, 1f));

		TargetShoot.transform.position = _trans.position;
		TargetShoot.PawnRigid2D.velocity = Vector2.zero;

		if (OnShootPawn != null)
			OnShootPawn();
		TargetShoot.PawnRigid2D.AddForce(_aimDir * ShootForce, ForceMode2D.Impulse);
		StartCoroutine(HelperTween.ScaleEnum(TargetShoot.transform, TargetShoot.transform.localScale, Vector3.one, 0.2f,
			AnimationCurve.EaseInOut(0f, 0f, 1f, 1f)));
		yield return HelperTween.TimeScaleEnum(0f, 1f, 0.3f, AnimationCurve.EaseInOut(0f, 0f, 1f, 1f));
		//Move & scale and timescale
	}
}


public static class HelperTween
{

	public static IEnumerator ScaleEnum(Transform target, Vector3 beg, Vector3 end, float duration, AnimationCurve curve, bool useUnscaledDeltaTime = true)
	{
		return IterateEnum((value) => target.localScale = value, Vector3.Lerp, beg, end, duration, curve, useUnscaledDeltaTime);
	}

	public static IEnumerator MoveRigid2DEnum(Rigidbody2D target, Vector3 beg, Vector3 end, float duration, AnimationCurve curve, bool useUnscaledDeltaTime = true)
	{
		return IterateEnum((value) => target.MovePosition(value), Vector3.Lerp, beg, end, duration, curve, useUnscaledDeltaTime);
	}
	public static IEnumerator MoveTransformEnum(Transform target, Vector3 beg, Vector3 end, float duration, AnimationCurve curve, bool useUnscaledDeltaTime = true)
	{
		return IterateEnum((value) => target.position = value, Vector3.Lerp, beg, end, duration, curve, useUnscaledDeltaTime);
	}

	public static IEnumerator TimeScaleEnum(float beg, float end, float duration, AnimationCurve curve, bool useUnscaledDeltaTime = true)
	{
		return IterateEnum((value) => Time.timeScale = value, Mathf.Lerp, beg, end, duration, curve, useUnscaledDeltaTime);
	}

	private static IEnumerator IterateEnum<T>(Action<T> callback, Func<T, T, float, T> lerpFunc, T beg, T end, float duration, AnimationCurve curve, bool useUnscaledDeltaTime)
	{
		for (float t = 0f, perc = 0f; perc < 1f; t += useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime)
		{
			perc = Mathf.Clamp01(t / duration);
			callback(lerpFunc(beg, end, curve.Evaluate(perc)));
			yield return null;
		}
	}
}