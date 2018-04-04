using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public PawnComponent Target;
	public string KeyPlayerInput = "P1";

	private Vector2 _aimDir;

	private string _keyHorizontal;
	private string _keyFire;
	private string _keyAimX;
	private string _keyAimY;
	private string _keyJump;

	public void Awake()
	{
		_keyHorizontal = KeyPlayerInput + "_Horizontal";
		_keyFire = KeyPlayerInput + "_Fire1";
		_keyAimX = KeyPlayerInput + "_AimX";
		_keyAimY = KeyPlayerInput + "_AimY";
		_keyJump = KeyPlayerInput + "_Jump";
		_aimDir = Vector2.zero;
	}

	public void Update()
	{
		if (Input.GetButtonDown(_keyJump))
			Target.Jump(true);
		else if (Input.GetButtonUp(_keyJump))
			Target.Jump(false);

		Target.Move(Input.GetAxisRaw(_keyHorizontal));

		_aimDir.Set(Input.GetAxis(_keyAimX), Input.GetAxis(_keyAimY));
		if (_aimDir != Vector2.zero)
			_aimDir.Normalize();

		Target.Aiming(_aimDir);

		if (Input.GetButtonDown(_keyFire))
			Target.FirePlayer(true);
		else if (Input.GetButtonUp(_keyFire))
			Target.FirePlayer(false);
	}

	public void Init(PawnComponent pawn, PawnComponent otherPawn, bool enableSwitchPawn, int index)
	{
		Target = pawn;
		Target.Init(otherPawn);

		if (enableSwitchPawn)
		{
			Target.OnShootPawn += () => Target = Target.TargetShoot;
			otherPawn.OnShootPawn += () => Target = Target.TargetShoot;

			otherPawn.Init(pawn);
		}

		KeyPlayerInput = "P" + (index + 1);

		pawn.GetComponentInChildren<SpriteRenderer>().color = index == 0 ? Color.green : Color.blue;
	}
}
