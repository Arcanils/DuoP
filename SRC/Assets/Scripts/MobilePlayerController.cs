﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePlayerController
{
	private MobilePawnComponent _pawn;
	private MobileInputController _input;
	
	public MobilePlayerController(MobilePawnComponent pawn, MobileInputController input)
	{
		Init(pawn, input);
	}

	public void Init(MobilePawnComponent pawn, MobileInputController input)
	{
		_pawn = pawn;
		_pawn.OnDeath += PropulsionCooldown.LaForceVeloce.ResetForceVeloce;
		_input = input;
		_input.OnTouchSlowMo += PropulsionCooldown.LaForceVeloce.SlowDown;
		_input.OnReleaseSlowMo += PropulsionCooldown.LaForceVeloce.SlowUp;
		_input.OnReleaseSlowMo += FirePlayer;
	}
	public void Clear()
	{
		_input.OnTouchSlowMo -= PropulsionCooldown.LaForceVeloce.SlowDown;
		_input.OnReleaseSlowMo -= PropulsionCooldown.LaForceVeloce.SlowUp;
		_input.OnReleaseSlowMo -= FirePlayer;
	}

	private void FirePlayer(Vector2 dir)
	{
		_pawn.Fire(dir);
	}

}
