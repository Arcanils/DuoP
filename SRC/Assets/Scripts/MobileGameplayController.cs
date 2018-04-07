using System;
using System.Collections;
using UnityEngine;
//MobileGameplayController
public class MobileGameplayController : AbstractGameplayController
{
	public MobileInputController InputController;
	public MobilePawnComponent PawnPlayer;

	private MobilePawnComponent _instancePawn;
	private MobilePlayerController _instanceController;

	protected override IEnumerator AnimDeathEnum()
	{
		_finish = true;
		_animDeath = true;
		Destroy(_instancePawn.gameObject);
		_instanceController.Clear();
		_instanceController = null;

		yield return new WaitForSeconds(1f);

		SpawnGameplay();
		_finish = false;
		_animDeath = false;
	}

	protected override void SpawnGameplay()
	{
		_instancePawn = CreatePawn();
		_instanceController = CreateController(_instancePawn);
		Cam.Init(_instancePawn.transform);
	}

	private MobilePawnComponent CreatePawn()
	{
		return GameObject.Instantiate(PawnPlayer, StartPoint.position, Quaternion.identity).GetComponent<MobilePawnComponent>();
	}

	public MobilePlayerController CreateController(MobilePawnComponent pawn)
	{
		return new MobilePlayerController(pawn, InputController);
	}
}
