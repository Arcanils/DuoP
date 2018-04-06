using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
	public enum EGameplayMode
	{
		OnePlayer,
		TwoPlayer,
	}

	public GameObject PrefabController;
	public GameObject PrefabPawn;
	public Text Timer;

	public Transform StartPoint;

	public CameraBehaviour Cam;

	public EGameplayMode CurrentMode;

	private bool _finish;
	private float _currentTimer;

	public void Awake()
	{
		SpawnGameplay(CurrentMode);
		Timer.text = ConverTimerToString(0f);
		var triggers = FindObjectsOfType<LogicTrigger>();
		for (int i = 0; i < triggers.Length; i++)
		{
			triggers[i].OnTriggerEnter += OnPlayerEnterTrigger;
		}
	}

	private void OnPlayerEnterTrigger(LogicTrigger.EModeTrigger mode)
	{
		switch(mode)
		{
			case LogicTrigger.EModeTrigger.DEATH:
				OnDeath();
				break;
			case LogicTrigger.EModeTrigger.FINISH:
				FinishGame();
				break;
		}
	}
	private bool _animDeath;
	private void OnDeath()
	{
		if (_animDeath)
			return;

		StartCoroutine(AnimDeathEnum());
	}

	private IEnumerator AnimDeathEnum()
	{
		_finish = true;
		_animDeath = true;
		var pawns = FindObjectsOfType<PawnComponent>();
		var controllers = FindObjectsOfType<PlayerController>();
		for (int i = 0; i < controllers.Length; i++)
		{
			Destroy(controllers[i].gameObject);
		}
		for (int i = 0; i < pawns.Length; i++)
		{
			Destroy(pawns[i].gameObject);
		}

		yield return new WaitForSeconds(1f);

		SpawnGameplay(CurrentMode);
		_finish = false;
		_animDeath = false;
	}

	private void FinishGame()
	{
		if (_finish)
			return;

		_finish = true;
	}

	private void SpawnGameplay(EGameplayMode mode)
	{
		PawnComponent pawnP1, pawnP2;
		PlayerController controllerP1, controllerP2;
		CreatePlayer(out pawnP1, out controllerP1);
		CreatePlayer(out pawnP2, out controllerP2);
		controllerP1.Init(pawnP1, pawnP2, mode == EGameplayMode.OnePlayer, 0);
		if (CurrentMode == EGameplayMode.TwoPlayer)
			controllerP1.Init(pawnP1, pawnP2, false, 1);
		else
		{
			Destroy(controllerP2);
		}
		Cam.Init(pawnP1.transform, pawnP2.transform);
	}

	private void CreatePlayer(out PawnComponent instancePawn, out PlayerController instanceController)
	{

		var instance = GameObject.Instantiate(PrefabPawn);
		var transPawn = instance.transform;
		transPawn.position = StartPoint.position;
		instancePawn = instance.GetComponent<PawnComponent>();

		instance = GameObject.Instantiate(PrefabController);
		instanceController = instance.GetComponent<PlayerController>();
	}

	public void Update()
	{
		if (_finish)
			return;

		_currentTimer += Time.unscaledDeltaTime;
		Timer.text = ConverTimerToString(_currentTimer);

	}

	private static string ConverTimerToString(float time)
	{
		var min = (int)(time / 60f);

		time -= min * 60f;
		var sec = (int)time;

		time -= sec;
		var centiemes = (int)(time * 100f);

		return min.ToString("00") + " : " + sec.ToString("00") + " : " + centiemes.ToString("00"); 
	}
}
