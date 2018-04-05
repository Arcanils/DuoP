using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
	public enum EGameplayMode
	{
		OnePlayer,
		TwoPlayer,
	}

	public GameObject PrefabController;
	public GameObject PrefabPawn;

	public Transform StartPoint;

	public CameraBehaviour Cam;

	public EGameplayMode CurrentMode;

	public void Awake()
	{
		SpawnGameplay(CurrentMode);
	}


	private void SpawnGameplay(EGameplayMode mode)
	{
		PawnComponent pawnP1, pawnP2;
		PlayerController controllerP1, controllerP2;
		CreatePlayer(out pawnP1, out controllerP1);
		CreatePlayer(out pawnP2, out controllerP2);
		controllerP1.Init(pawnP1, pawnP2, mode == EGameplayMode.OnePlayer, 0);
		if (CurrentMode == EGameplayMode.TwoPlayer)
			controllerP1.Init(pawnP1, pawnP2, false, 1);
		else
		{
			Destroy(controllerP2);
		}
		Cam.Init(pawnP1.transform, pawnP2.transform);
	}

	private void CreatePlayer(out PawnComponent instancePawn, out PlayerController instanceController)
	{

		var instance = GameObject.Instantiate(PrefabPawn);
		var transPawn = instance.transform;
		transPawn.position = StartPoint.position;
		instancePawn = instance.GetComponent<PawnComponent>();

		instance = GameObject.Instantiate(PrefabController);
		instanceController = instance.GetComponent<PlayerController>();
	}
}
