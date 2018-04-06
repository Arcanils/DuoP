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

	public Action<EModeTrigger> OnTriggerEnter;

	public EModeTrigger Mode;


	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			if (OnTriggerEnter != null)
				OnTriggerEnter(Mode);
		}
	}
}
