using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class CubeManager : MonoBehaviour
{
	[SerializeField] private Light pointLight;
	[SerializeField] private List<GameObject> blocks = new List<GameObject>();

	[SerializeField] private float explosionForce = 500f;
	[SerializeField] private float explosionRadius = 100f;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.U))
			FloatCube();
		if (Input.GetKeyDown(KeyCode.F))
			FallCube();
		if (Input.GetKeyDown(KeyCode.C))
			CrashCube();
		if (Input.GetKeyDown(KeyCode.R))
			ResetCube();
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

	public void ResetCube()
	{
		pointLight.GetComponent<Light>().enabled = true;
		GetComponent<BoxCollider>().enabled = true;
		Sequence sequence = DOTween.Sequence();
		foreach (GameObject block in blocks)
		{
			sequence.AppendCallback(() => block.GetComponent<BlockManager>().ResetBlock());
			sequence.AppendInterval(0.08f);
		}
		sequence.OnComplete(() => {
			pointLight.GetComponent<Light>().enabled = true;
		});
	}

	public void FloatCube()
	{
		GetComponent<Rigidbody>().isKinematic = true;
		float floatHeight = 10f;
		float floatDuration = 0.5f;
		transform.DOLocalMoveY(floatHeight, floatDuration).SetEase(Ease.OutBack);
	}

	public void FallCube()
	{
		GetComponent<Rigidbody>().isKinematic = false;
	}
}