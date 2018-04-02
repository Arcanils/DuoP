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
	public Rigidbody2D TargetShoot;


	private Transform _trans;
	private Rigidbody2D _rigid2D;
	
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
	}

	private void Awake()
	{
		_trans = transform;
		_rigid2D = GetComponent<Rigidbody2D>();
	}


	public void OnDrawGizmos()
	{
		if (_aimDir != Vector2.zero)
			Gizmos.DrawLine(_trans.position, _trans.position + new Vector3(_aimDir.x, _aimDir.y) * 2f);
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

	private void FixedUpdate()
	{
		UpdateIsGrounded();
		JumpLogic();
		MoveLogic();
		FireLogic();
	}

	private void JumpLogic()
	{

		if (!_isGrounded || !_inputJump)
			return;

		_rigid2D.AddForce(JumpForce, ForceMode2D.Impulse);
		
	}

	private void MoveLogic()
	{
		_rigid2D.velocity = _deltaMove + new Vector2(0f, _rigid2D.velocity.y);
		_deltaMove.Set(0f, 0f);
	}

	private void UpdateIsGrounded()
	{
		Vector2 pos = new Vector2(_trans.position.x, _trans.position.y);
		var hitInfo = Physics2D.Raycast(LeftBorder + pos, Vector2.down, 0.2f, LayerWakeable);
		if (hitInfo.transform != null)
			_isGrounded = true;
		else
			_isGrounded = false;
	}
	
	private void FireLogic()
	{
		if (!_inputFire || _aimDir == Vector2.zero)
			return;

		_inputFire = false;

		//TargetShoot.MovePosition(new Vector2(_trans.position.x, _trans.position.y));
		TargetShoot.transform.position = _trans.position;
		TargetShoot.velocity = Vector2.zero;
		TargetShoot.AddForce(_aimDir * ShootForce, ForceMode2D.Impulse);

	}
}
