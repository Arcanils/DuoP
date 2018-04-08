using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMain : MonoBehaviour
{

	public RectTransform StartScreen;


	public void Play()
	{
		StartScreen.GetComponent<CanvasGroup>().interactable = false;
		Debug.Log(
			StartScreen.anchoredPosition);
		Debug.Log(
			StartScreen.anchoredPosition +
			Vector2.up * StartScreen.rect.height);
		var routine = HelperTween.MoveRectTransformEnum(
			StartScreen, 
			StartScreen.anchoredPosition, 
			StartScreen.anchoredPosition + 
			Vector2.up * StartScreen.rect.height, 
			1f, 
			AnimationCurve.EaseInOut(0f, 0f, 1f, 1f), 
			true,
			() => SceneManager.LoadScene(1));

		StartCoroutine(routine);
	}

	public void Quit()
	{
		StartScreen.GetComponent<CanvasGroup>().interactable = false;
		Debug.Log(Vector2.down * StartScreen.rect.height);
		var routine = HelperTween.MoveRectTransformEnum(
			StartScreen,
			StartScreen.anchoredPosition,
			StartScreen.anchoredPosition +
			Vector2.down * StartScreen.rect.height,
			1f,
			AnimationCurve.EaseInOut(0f, 0f, 1f, 1f),
			true,
			() => Application.Quit());
		StartCoroutine(routine);
	}
}
