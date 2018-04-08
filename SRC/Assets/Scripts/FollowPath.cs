using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

	public Vector3[] Points;
	public float Duration;
	public int StartIndex;

	public void Start()
	{
		StartCoroutine(FollowEnum());
	}

	private IEnumerator FollowEnum()
	{
		var trans = transform;
		var origine = trans.position;
		var length = Points.Length;
		var onePathDuration = Duration / length;
		var lerpCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		int index = StartIndex % length;
		while (true)
		{
			var beg = Points[index] + origine;
			index = ++index % length;
			var end = Points[index] + origine;
			yield return HelperTween.MoveTransformEnum(trans, beg, end, onePathDuration, lerpCurve, false);
		}
	}
}
