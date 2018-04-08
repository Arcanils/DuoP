using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpriteRenderer : MonoBehaviour
{
	public Sprite[] Sprites;
	public float Duration = 1f;

	public void Start()
	{
		StartCoroutine(AnimEnum());
	}

	private IEnumerator AnimEnum()
	{
		var sr = GetComponent<SpriteRenderer>();
		var index = 0;
		var length = Sprites.Length;
		float durationByImg = Duration / length;
		yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
		while(true)
		{
			sr.sprite = Sprites[index];
			yield return new WaitForSeconds(durationByImg);
			index = (index + 1) % length;
		}
	}

}
