using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	[SerializeField] private CubeManager cubeManager;
	[SerializeField] private CameraManager cameraManager;

	public bool inGame = false;
	public bool crashed = false;
	void Start()
	{
		cubeManager.UpdateCube();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (inGame)
				EndGame();
			else
				StartGame();
		}
		if (Input.GetKeyDown(KeyCode.I))
			EnterCube();
		if (Input.GetKeyDown(KeyCode.O))
			ExitCube();
		if (Input.GetKeyDown(KeyCode.R))
			ResetGame();
		if (Input.GetKeyDown(KeyCode.S))
			ShuffleCube();
	}

	private void ShuffleCube()
	{
		if (inGame || crashed)
			return;
		cubeManager.ShuffleCube();
	}

	private void StartGame()
	{
		inGame = true;
		Sequence sequence = DOTween.Sequence();
		sequence.Append(cubeManager.FloatCube());
		sequence.Append(cameraManager.MoveInToCube());
	}

	private void EnterCube()
	{
		if (!inGame)
			return;
		Sequence sequence = DOTween.Sequence();
		sequence.Append(cameraManager.MoveInToCube());
	}

	private void ExitCube()
	{
		if (!inGame)
			return;
		Sequence sequence = DOTween.Sequence();
		sequence.Append(cameraManager.GetOutOfCube());
	}

	private void EndGame()
	{
		Sequence sequence = DOTween.Sequence();
		if (cameraManager.inSideTheCube)
			sequence.Append(cameraManager.GetOutOfCube());
		sequence.OnStart(() =>
		{
			crashed = true;
			cubeManager.CrashCube();
		})
		.OnComplete(() =>
		{
			inGame = false;
		});
	}

	private void ResetGame()
	{
		if (inGame)
			return;
		Sequence sequence = DOTween.Sequence();
		sequence.Append(cubeManager.ResetCube());
		sequence.OnComplete(() =>
		{
			crashed = false;
		});
	}

	// Add your game management methods here
}