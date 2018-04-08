using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOffsetOnPosition : MonoBehaviour {
	
	private Material _mat;
	private void Awake()
	{
		_mat = GetComponent<Renderer>().material;
	}
	void Update ()
	{
		_mat.mainTextureOffset = new Vector2(-transform.position.x / 100f, -transform.position.y/ 100f);
	}
}
