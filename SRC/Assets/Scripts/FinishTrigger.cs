using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour {


	public Action OnFinishGame;

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			if (OnFinishGame != null)
				OnFinishGame();
		}
	}
}
