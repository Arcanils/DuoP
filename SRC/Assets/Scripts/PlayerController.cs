using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public PawnComponent Target;

	private Vector2 _aimDir;

	public void Update()
	{
		if (Input.GetButtonDown("Jump"))
			Target.Jump(true);
		else if (Input.GetButtonUp("Jump"))
			Target.Jump(false);

		Target.Move(Input.GetAxisRaw("Horizontal"));

		_aimDir.Set(Input.GetAxis("AimX"), Input.GetAxis("AimY"));
		if (_aimDir != Vector2.zero)
			_aimDir.Normalize();

		Target.Aiming(_aimDir);

		if (Input.GetButtonDown("Fire1"))
			Target.FirePlayer(true);
		else if (Input.GetButtonUp("Fire1"))
			Target.FirePlayer(false);
	}
}
