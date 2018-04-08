using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileInputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public Action OnTouchSlowMo;
	public Action<Vector2> OnReleaseSlowMo;

	private int _currentInputId = int.MinValue;

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (_currentInputId != int.MinValue)
			return;

		_currentInputId = eventData.pointerId;
		if (OnTouchSlowMo != null)
			OnTouchSlowMo();
	}

	public virtual void OnPointerUp(PointerEventData eventData)
	{
		if (_currentInputId != eventData.pointerId)
			return;

		if (OnReleaseSlowMo != null)
			OnReleaseSlowMo(eventData.position - new Vector2(Screen.width, Screen.height) / 2f);

		_currentInputId = int.MinValue;
	}

	public void ResetData()
	{
		_currentInputId = int.MinValue;
	}
}
