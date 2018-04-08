using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicTrigger : MonoBehaviour {

	public enum EModeTrigger
	{
		DEATH,
		FINISH,
	}

	public Action<EModeTrigger> OnTouch;

	public EModeTrigger Mode;


	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			if (OnTouch != null)
				OnTouch(Mode);
		}
	}
	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == "Player")
		{
			if (OnTouch != null)
				OnTouch(Mode);
		}
	}
}
