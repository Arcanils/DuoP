using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractGameplayController : MonoBehaviour
{
	public Text Timer;
	public Transform StartPoint;

	public CameraBehaviour Cam;

	protected bool _finish;
	protected bool _animDeath;
	private float _currentTimer;

	protected virtual void Start()
	{
		Timer.text = ConverTimerToString(0f);
		var triggers = FindObjectsOfType<LogicTrigger>();
		for (int i = 0; i < triggers.Length; i++)
		{
			triggers[i].OnTriggerEnter += OnPlayerEnterTrigger;
		}
		SpawnGameplay();
	}

	protected virtual void Update()
	{
		if (_finish)
			return;

		_currentTimer += Time.unscaledDeltaTime;
		Timer.text = ConverTimerToString(_currentTimer);

	}
	protected abstract void SpawnGameplay();
	protected abstract IEnumerator AnimDeathEnum();

	protected void FinishGame()
	{
		if (_finish)
			return;

		_finish = true;
	}

	protected static string ConverTimerToString(float time)
	{
		var min = (int)(time / 60f);

		time -= min * 60f;
		var sec = (int)time;

		time -= sec;
		var centiemes = (int)(time * 100f);

		return min.ToString("00") + " : " + sec.ToString("00") + " : " + centiemes.ToString("00");
	}

	private void OnDeath()
	{
		if (_animDeath)
			return;

		StartCoroutine(AnimDeathEnum());
	}

	private void OnPlayerEnterTrigger(LogicTrigger.EModeTrigger mode)
	{
		switch (mode)
		{
			case LogicTrigger.EModeTrigger.DEATH:
				OnDeath();
				break;
			case LogicTrigger.EModeTrigger.FINISH:
				FinishGame();
				break;
		}
	}
}
