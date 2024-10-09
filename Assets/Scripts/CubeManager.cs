using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class CubeManager : MonoBehaviour
{
	[SerializeField] private Light pointLight;
	[SerializeField] private GameObject cameraSystem;
	[SerializeField] private List<UnitsManager> unitsManagers = new List<UnitsManager>();
	[SerializeField] private List<GameObject> blocks = new List<GameObject>();

	[SerializeField] private float explosionForce = 500f;
	[SerializeField] private float explosionRadius = 100f;

	void Update()
	{
		// if (Input.GetKeyDown(KeyCode.U))
		// 	FloatCube();
		// if (Input.GetKeyDown(KeyCode.F))
		// 	FallCube();
		// if (Input.GetKeyDown(KeyCode.C))
		// 	CrashCube();
		// if (Input.GetKeyDown(KeyCode.R))
		// 	ResetCube();
	}

	public void UpdateCube()
	{
		foreach (GameObject block in blocks)
			block.GetComponent<BlockManager>().UpdateBelongingUnit();
	}

	public void UpdatePanelRotationStatus()
	{
		foreach (GameObject block in blocks)
			block.GetComponent<BlockManager>().UpdatePanelRotationStatus();
	}

	private void TogglePanelCollider(bool onOff)
	{
		foreach (GameObject block in blocks)
		{
			block.GetComponent<BlockManager>().SeparateBlock();
		}
	}

	public void CrashCube()
	{
		pointLight.GetComponent<Light>().enabled = false;
		GetComponent<BoxCollider>().enabled = false;
		foreach (GameObject block in blocks)
		{
			block.GetComponent<BlockManager>().SeparateBlock();
			Rigidbody rigidbody = block.GetComponent<Rigidbody>();
			if (rigidbody != null)
			{
				Vector3 direction = (block.transform.position - transform.position).normalized;
				rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
			}
		}
	}

	public Sequence ResetCube()
	{
		
		Sequence sequence = DOTween.Sequence();
		foreach (GameObject block in blocks)
		{
			sequence.AppendCallback(() => block.GetComponent<BlockManager>().ResetBlock());
			sequence.AppendInterval(0.03f);
		}
		sequence.OnStart(() => {
			pointLight.GetComponent<Light>().enabled = true;
			GetComponent<BoxCollider>().enabled = true;
			cameraSystem.transform.DOLocalRotateQuaternion(Quaternion.identity, 0.6f);
		})
		.OnComplete(() => {
			pointLight.GetComponent<Light>().enabled = true;
		});
		return sequence;
	}

	public Tween FloatCube()
	{
		float floatHeight = 10f;
		float floatDuration = 0.5f;
		// GetComponent<Rigidbody>().isKinematic = true;
		// transform.DOLocalMoveY(floatHeight, floatDuration).SetEase(Ease.OutBack);
		return (transform.DOLocalMoveY(floatHeight, floatDuration).SetEase(Ease.OutBack)
			.OnStart(() => {
				GetComponent<Rigidbody>().isKinematic = true;
			}));
	}

	public void FallCube()
	{
		GetComponent<Rigidbody>().isKinematic = false;
	}

	public void ShuffleCube()
	{
		Sequence sequence = DOTween.Sequence();
		int shuffleCount = 25 + Random.Range(0, 10);
		float tweenDuration = 0.08f;

		for (int i = 0; i < shuffleCount; i++)
		{
			Tween tween = unitsManagers[Random.Range(0, unitsManagers.Count)].AutoRotate(Random.Range(0, 2) == 0, tweenDuration);
			sequence.Append(tween);
		}
	}
}