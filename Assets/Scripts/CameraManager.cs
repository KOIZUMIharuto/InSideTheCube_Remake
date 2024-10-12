using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
	[SerializeField] private GameManager gameManager;
	[SerializeField] private CubeManager cubeManager;

	public Vector3 forwardDirection = Vector3.zero;
	public Vector3 upDirection = Vector3.zero;
	public Vector3 rightDirection = Vector3.zero;

    [SerializeField] private float moveDuration = 1.0f;
    [SerializeField] private float rotationDuration = 1.0f;

    // 移動先の座標と方向
    [SerializeField] private Vector3 targetLocalPosition;
    [SerializeField] private Quaternion targetLocalRotation;

	[SerializeField] private GameObject cameraSystem;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
	private Quaternion currentRotation;

	private Sequence enterSequence;

	void Start()
	{
		originalPosition = transform.position;
		originalRotation = transform.rotation;
	}

	private void UpdateCameraStatus()
	{
		forwardDirection = cameraSystem.transform.parent.InverseTransformDirection(cameraSystem.transform.forward);
		forwardDirection = new Vector3(Mathf.Round(forwardDirection.x), Mathf.Round(forwardDirection.y), Mathf.Round(forwardDirection.z));
		upDirection = cameraSystem.transform.parent.InverseTransformDirection(cameraSystem.transform.up);
		upDirection = new Vector3(Mathf.Round(upDirection.x), Mathf.Round(upDirection.y), Mathf.Round(upDirection.z));
		rightDirection = cameraSystem.transform.parent.InverseTransformDirection(cameraSystem.transform.right);
		rightDirection = new Vector3(Mathf.Round(rightDirection.x), Mathf.Round(rightDirection.y), Mathf.Round(rightDirection.z));
	}

	public Sequence EnterCube()
	{
		enterSequence = DOTween.Sequence();
		enterSequence.Append(transform.DOLocalMove(targetLocalPosition, moveDuration));
		enterSequence.Join(transform.DOLocalRotateQuaternion(targetLocalRotation, moveDuration));
		enterSequence.OnStart(() => {
			this.transform.parent = cameraSystem.transform;
		})
		.OnComplete(() => {
			currentRotation = cameraSystem.transform.localRotation;
			UpdateCameraStatus();
			cubeManager.UpdatePanelRotationStatus();
			enterSequence = null;
		});
		
		return enterSequence;
		
	}

	public Sequence ExitCube()
	{
		if (enterSequence != null)
			enterSequence.Kill(true);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(transform.DOMove(originalPosition, moveDuration));
		sequence.Join(transform.DORotateQuaternion(originalRotation, moveDuration));
		sequence.OnStart(() => {
			this.transform.parent = null;
		});

		return sequence;
	}

	public void HandleCameraRotation()
	{
		if (cameraSystem == null)
			return;

		Quaternion rotationChange = Quaternion.identity;

		if (Input.GetKeyDown(KeyCode.W))
			rotationChange = Quaternion.Euler(-90f, 0f, 0f);
		if (Input.GetKeyDown(KeyCode.S))
			rotationChange = Quaternion.Euler(90f, 0f, 0f);
		if (Input.GetKeyDown(KeyCode.A))
			rotationChange = Quaternion.Euler(0f, -90f, 0f);
		if (Input.GetKeyDown(KeyCode.D))
			rotationChange = Quaternion.Euler(0f, 90f, 0f);
		if (Input.GetKeyDown(KeyCode.Q))
			rotationChange = Quaternion.Euler(0f, 0f, 90f);
		if (Input.GetKeyDown(KeyCode.E))
			rotationChange = Quaternion.Euler(0f, 0f, -90f);

		if (rotationChange != Quaternion.identity)
		{
			currentRotation *= rotationChange;
			Sequence sequence = DOTween.Sequence();
			sequence.Append(cameraSystem.transform.DOLocalRotateQuaternion(currentRotation, rotationDuration));
			sequence.OnComplete(() => {
				UpdateCameraStatus();
				cubeManager.UpdatePanelRotationStatus();
			});
		}
	}

}
