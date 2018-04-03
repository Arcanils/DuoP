using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

	public Transform[] Targets;

	private Transform _trans;
	private Vector3 _posCamera;

	public void Awake()
	{
		_trans = transform;
		_posCamera = _trans.position;
	}
	private void Update ()
	{
		var posX = 0f;
		var posY = 0f;
		var count = 0;
		for (var i = Targets.Length - 1; i >= 0; i--)
		{
			if (!Targets[i])
				continue;

			posX += Targets[i].position.x;
			posY += Targets[i].position.y;
			++count;
		}
		_posCamera.Set(posX / count, posY / count, _posCamera.z);
		_trans.position = _posCamera;
	}
}
