using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	[SerializeField] private CubeManager cubeManager;
	[SerializeField] private CameraManager cameraManager;

	public int rotateCount = 0;
	public float time = 0;
	public bool inGame = false;
	public bool inSideTheCube = false;
	private bool crashed = false;
	private Sequence sequence;
	void Start()
	{
		cubeManager.UpdateCube();
	}

	void Update()
	{
		if (inGame)
			time += Time.deltaTime;
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
			ResetCube();
		if (Input.GetKeyDown(KeyCode.S))
			ShuffleCube();
	}

	public void IncreaseRotateCount()
	{
		rotateCount++;
	}

	private void ShuffleCube()
	{
		if (inGame || crashed || sequence != null)
			return;
		sequence = DOTween.Sequence();
		sequence.Append(cubeManager.FloatCube());
		sequence.Append(cubeManager.ShuffleCube());
		sequence.OnComplete(() =>
		{
			cubeManager.FallCube();
			sequence = null;
		});
	}

	private void StartGame()
	{
		if (crashed || sequence != null)
			return;
		inGame = true;
		time = 0;
		rotateCount = 0;
		cubeManager.UpdateCube();
		sequence = DOTween.Sequence();
		sequence.Append(cubeManager.FloatCube());
		sequence.Append(cameraManager.EnterCube());
		sequence.OnComplete(() =>
		{
			inSideTheCube = true;
			sequence = null;
		});
	}

	private void EnterCube()
	{
		if (!inGame)
			return;
		if (sequence != null)
			sequence.Kill(false);
		sequence = DOTween.Sequence();
		sequence.Append(cameraManager.EnterCube());
		sequence.OnComplete(() =>
		{
			inSideTheCube = true;
			sequence = null;
		});
	}

	private void ExitCube()
	{
		if (!inGame)
			return;
		if (sequence != null)
			sequence.Kill(false);
		inSideTheCube = false;
		sequence = DOTween.Sequence();
		sequence.Append(cameraManager.ExitCube());
		sequence.OnComplete(() =>
		{
			sequence = null;
		});
	}

	private void EndGame()
	{
		if (sequence != null)
			sequence.Kill(false);
		sequence = DOTween.Sequence();
		sequence.Append(cameraManager.ExitCube());
		sequence.OnStart(() =>
		{
			inGame = false;
			inSideTheCube = false;
			crashed = true;
			cubeManager.CrashCube();
		})
		.OnComplete(() =>
		{
			sequence = null;
		});
	}

	private void ResetCube()
	{
		if (inGame || sequence != null)
			return;
		cubeManager.FallCube();
		sequence = DOTween.Sequence();
		sequence.Append(cubeManager.ResetCube());
		sequence.OnComplete(() =>
		{
			cubeManager.FallCube();
			crashed = false;
			sequence = null;
		});
	}

	public void GameClear()
	{
		if (!inGame)
			return;
		if (sequence != null)
			sequence.Kill(false);
		sequence = DOTween.Sequence();
		sequence.Append(cameraManager.ExitCube());
		sequence.OnStart(() =>
		{
			inGame = false;
			inSideTheCube = false;
		})
		.OnComplete(() =>
		{
			cubeManager.FallCube();
			sequence = null;
		});
	}

	// Add your game management methods here
}