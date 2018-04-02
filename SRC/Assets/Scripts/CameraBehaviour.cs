using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

	public Transform Target;

	private Transform _trans;
	private Vector3 _posCamera;

	public void Awake()
	{
		_trans = transform;
		_posCamera = _trans.position;
	}
	void Update ()
	{
		_posCamera.Set(Target.position.x, Target.position.y, _posCamera.z);
		_trans.position = _posCamera;
	}
}
