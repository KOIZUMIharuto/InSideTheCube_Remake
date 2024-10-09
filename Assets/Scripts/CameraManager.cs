using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
	[SerializeField] private GameObject cube;

	public bool inSideTheCube = false;

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

	void Update()
	{
		if (inSideTheCube)
			HandleCameraRotation();
		else if (forwardDirection != Vector3.zero || upDirection != Vector3.zero)
		{
			forwardDirection = Vector3.zero;
			upDirection = Vector3.zero;
		}
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

    public Sequence MoveInToCube()
    {
        Sequence sequence = DOTween.Sequence();
		sequence.Append(transform.DOLocalMove(targetLocalPosition, moveDuration));
		sequence.Join(transform.DOLocalRotateQuaternion(targetLocalRotation, moveDuration));
        sequence.OnStart(() => {
			originalPosition = transform.position;
			originalRotation = transform.rotation;
			this.transform.parent = cameraSystem.transform;
		})
		.OnComplete(() => {
			inSideTheCube = true;
			currentRotation = cameraSystem.transform.localRotation;
			UpdateCameraStatus();
			cube.GetComponent<CubeManager>().UpdateCube();
		});
		
		return sequence;
		
    }

    public Sequence GetOutOfCube()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(originalPosition, moveDuration));
        sequence.Join(transform.DORotateQuaternion(originalRotation, moveDuration));
		sequence.OnStart(() => {
			inSideTheCube = false;
			this.transform.parent = null;
		});

		return sequence;
    }

	private void HandleCameraRotation()
	{
		if (cameraSystem == null || Input.anyKeyDown == false)
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
				cube.GetComponent<CubeManager>().UpdatePanelRotationStatus();
			});
		}
	}

}
