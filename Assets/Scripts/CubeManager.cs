using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class CubeManager : MonoBehaviour
{
	[SerializeField] private Light pointLight;
	[SerializeField] private GameManager gameManager;
	[SerializeField] private GameObject cameraSystem;
	[SerializeField] private List<UnitsManager> unitsManagers = new List<UnitsManager>();
	[SerializeField] private List<GameObject> blocks = new List<GameObject>();
	[SerializeField] private Dictionary<string, List<GameObject>> panelsDictionary = new Dictionary<string, List<GameObject>>
	{
		{ "White", new List<GameObject>() },
		{ "Red", new List<GameObject>() },
		{ "Blue", new List<GameObject>() },
		{ "Yellow", new List<GameObject>() },
		{ "Orange", new List<GameObject>() },
		{ "Green", new List<GameObject>() }
	};

	private Quaternion currentRotation;
    [SerializeField] private float rotationDuration = 1.0f;

	[SerializeField] private float explosionForce = 500f;
	[SerializeField] private float explosionRadius = 100f;

	[SerializeField] private float torqueRange = 10f;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			if (clearCheck())
			{
				Debug.Log("Clear!");
			}
			else
			{
				Debug.Log("Not Clear!");
			}
		}
	}

	public void setBlock(GameObject block)
	{
		blocks.Add(block);
	}

	public void setPanel(GameObject panel)
	{
		string color = panel.name;
		if (panelsDictionary.ContainsKey(color))
			panelsDictionary[color].Add(panel);
	}

	public void UpdateCube()
	{
		foreach (GameObject block in blocks)
			block.GetComponent<BlockManager>().UpdateBelongingUnit();
		if (gameManager.inGame && clearCheck())
			gameManager.GameClear();
	}

	public void UpdatePanelRotationStatus()
	{
		foreach (GameObject block in blocks)
			block.GetComponent<BlockManager>().UpdatePanelRotationStatus();
	}

	private Vector3 GetLocalRotateAxis(Vector3 direction)
	{
		Transform parentTransform = transform.parent;

		if (parentTransform.InverseTransformDirection(transform.forward) == direction)
			return Vector3.forward;
		if (parentTransform.InverseTransformDirection(-transform.forward) == direction)
			return Vector3.back;
		if (parentTransform.InverseTransformDirection(transform.up) == direction)
			return Vector3.up;
		if (parentTransform.InverseTransformDirection(-transform.up) == direction)
			return Vector3.down;
		if (parentTransform.InverseTransformDirection(transform.right) == direction)
			return Vector3.right;
		if (parentTransform.InverseTransformDirection(-transform.right) == direction)
			return Vector3.left;
		return Vector3.zero;
	}
	public void HandleCubeRotation()
	{
		GameObject parent = transform.parent.gameObject;
		Vector3 rotationAxis = Vector3.zero;
		// 入力に応じた回転の設定
		if (Input.GetKeyDown(KeyCode.W))
			rotationAxis = GetLocalRotateAxis(Vector3.forward);
		if (Input.GetKeyDown(KeyCode.S))
			rotationAxis = GetLocalRotateAxis(Vector3.back);
		if (Input.GetKeyDown(KeyCode.A))
			rotationAxis = GetLocalRotateAxis(Vector3.up);
		if (Input.GetKeyDown(KeyCode.D))
			rotationAxis = GetLocalRotateAxis(Vector3.down);
		if (Input.GetKeyDown(KeyCode.Q))
			rotationAxis = GetLocalRotateAxis(Vector3.left);
		if (Input.GetKeyDown(KeyCode.E))
			rotationAxis = GetLocalRotateAxis(Vector3.right);

		if (rotationAxis != Vector3.zero)
		{
			currentRotation *= Quaternion.Euler(90 * rotationAxis.x, 90 * rotationAxis.y, 90 * rotationAxis.z);
			Sequence sequence = DOTween.Sequence();
			sequence.Append(transform.DOLocalRotateQuaternion(currentRotation, rotationDuration));
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
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		Sequence sequence = DOTween.Sequence();
		foreach (GameObject block in blocks)
		{
			sequence.AppendCallback(() => block.GetComponent<BlockManager>().ResetBlock());
			sequence.AppendInterval(0.03f);
		}
		sequence.OnStart(() => {
			rigidbody.isKinematic = false;
			pointLight.GetComponent<Light>().enabled = true;
			GetComponent<BoxCollider>().enabled = true;
			cameraSystem.transform.DOLocalRotateQuaternion(Quaternion.identity, 0.6f);
		})
		.OnComplete(() => {
			pointLight.GetComponent<Light>().enabled = true;
		});
		return sequence;
	}

	public Sequence FloatCube()
	{
		float floatHeight = 20f;
		float floatDuration = 0.5f;
		Vector3 floatRotation = new Vector3(
			Random.Range(0, 10),
			Random.Range(-10, 10),
			Random.Range(0, 10)
		);
		GameObject cubeParent = transform.parent.gameObject;

		Sequence sequence = DOTween.Sequence();
		sequence.Append(transform.DOLocalMove(new Vector3(0, floatHeight, 0), floatDuration).SetEase(Ease.OutBack));
		sequence.Join(transform.DOLocalRotate(new Vector3(0, 0, 0), floatDuration).SetEase(Ease.OutBack));
		sequence.Join(cubeParent.transform.DOLocalRotate(floatRotation, floatDuration).SetEase(Ease.OutBack));
		sequence.OnStart(() => {
			GetComponent<Rigidbody>().isKinematic = true;
		})
		.OnComplete(() => {
			currentRotation = transform.localRotation;
		});
		return sequence;
	}

	public void FallCube()
	{
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rigidbody.useGravity = true;
		rigidbody.isKinematic = false;

		Vector3 randomTorque = new Vector3(
			Random.Range(-torqueRange, torqueRange),
			Random.Range(-torqueRange, torqueRange),
			Random.Range(-torqueRange, torqueRange)
		);
		rigidbody.AddTorque(randomTorque, ForceMode.Impulse);
	}

	public Sequence ShuffleCube()
	{
		Sequence sequence = DOTween.Sequence();
		int shuffleCount = 25 + Random.Range(0, 10);
		float tweenDuration = 0.08f;

		for (int i = 0; i < shuffleCount; i++)
		{
			Tween tween = unitsManagers[Random.Range(0, unitsManagers.Count)].AutoRotate(Random.Range(0, 2) == 0, tweenDuration);
			sequence.Append(tween);
		}
		return sequence;
	}

	public bool clearCheck()
	{
		foreach (List<GameObject> panels in panelsDictionary.Values)
		{
			PanelManager firstPanel = panels[0].GetComponent<PanelManager>();
			for (int i = 1; i < panels.Count; i++)
			{
				PanelManager panel = panels[i].GetComponent<PanelManager>();
				if (firstPanel.panelDirection != panel.panelDirection)
					return false;
			}
		}
		return true;
	}
}