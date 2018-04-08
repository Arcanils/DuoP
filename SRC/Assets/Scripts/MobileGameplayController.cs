using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
//MobileGameplayController
public class MobileGameplayController : AbstractGameplayController
{
	public MobileInputController InputController;
	public MobilePawnComponent PawnPlayer;
	public UIEndScreen UIEnd;

	private MobilePawnComponent _instancePawn;
	private MobilePlayerController _instanceController;
	private const string keyBestTime = "BEST_TIME";

	protected override IEnumerator AnimDeathEnum()
	{
		_finish = true;
		_animDeath = true;
		Destroy(_instancePawn.gameObject);
		_instanceController.Clear();
		_instanceController = null;

		InputController.ResetData();
		PropulsionCooldown.LaForceVeloce.ResetForceVeloce();

		yield return new WaitForSeconds(2f);

		SpawnGameplay();
		_finish = false;
		_animDeath = false;
	}

	protected override void FinishGame()
	{
		base.FinishGame();

		var bestTime = UpdateMaxScore();

		Destroy(_instancePawn.gameObject);
		_instanceController.Clear();
		_instanceController = null;

		UIEnd.PlayEnd(ConverTimerToString(_currentTimer), ConverTimerToString(bestTime));
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

	public void Retry()
	{
		SceneManager.LoadScene(1);
	}

	public void Quit()
	{
		SceneManager.LoadScene(0);
	}

	private float UpdateMaxScore()
	{
		var bestTime = float.MaxValue;
		if (PlayerPrefs.HasKey(keyBestTime))
			bestTime = PlayerPrefs.GetFloat(keyBestTime);

		if (bestTime < _currentTimer)
			return bestTime;
		PlayerPrefs.SetFloat(keyBestTime, _currentTimer);
		PlayerPrefs.Save();

		return _currentTimer;

	}
}
