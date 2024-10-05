using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
	public bool inSideTheCube = false;

	public Vector3 forwardDirection = Vector3.zero;
	public Vector3 upDirection = Vector3.zero;

    [SerializeField] private float moveDuration = 1.0f;
    [SerializeField] private float rotationDuration = 1.0f;

    // 移動先の座標と方向
    [SerializeField] private Vector3 targetLocalPosition;
    [SerializeField] private Quaternion targetLocalRotation;

	[SerializeField] private GameObject cameraSystem;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
	private Quaternion currentRotation;

	void Start()
	{
		// 初期回転を記録（クォータニオン）
		currentRotation = cameraSystem.transform.rotation;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.I) && !inSideTheCube)
			MoveInToCube();

		if (Input.GetKeyDown(KeyCode.O) && inSideTheCube)
			GetOutOfCube();

		if (inSideTheCube)
		{
			HandleCameraRotation();
			forwardDirection = cameraSystem.transform.parent.InverseTransformDirection(cameraSystem.transform.forward);
			forwardDirection = new Vector3(Mathf.Round(forwardDirection.x), Mathf.Round(forwardDirection.y), Mathf.Round(forwardDirection.z));
			upDirection = cameraSystem.transform.parent.InverseTransformDirection(cameraSystem.transform.up);
			upDirection = new Vector3(Mathf.Round(upDirection.x), Mathf.Round(upDirection.y), Mathf.Round(upDirection.z));
		}
		else
		{
			forwardDirection = Vector3.zero;
			upDirection = Vector3.zero;
		}
	}

    public void MoveInToCube()
    {
        // 現在の座標と方向を記録
        originalPosition = transform.position;
        originalRotation = transform.rotation;

		this.transform.parent = cameraSystem.transform;

        // 移動と回転を同時に実行
        Sequence sequence = DOTween.Sequence();
		sequence.Append(transform.DOLocalMove(targetLocalPosition, moveDuration));
		sequence.Join(transform.DOLocalRotateQuaternion(targetLocalRotation, rotationDuration));
        sequence.OnComplete(() => inSideTheCube = true);
    }

    public void GetOutOfCube()
    {
        inSideTheCube = false;
		this.transform.parent = null;

        // 元の座標と方向に戻る
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(originalPosition, moveDuration));
        sequence.Join(transform.DORotateQuaternion(originalRotation, rotationDuration));
    }

	private void HandleCameraRotation()
	{
		if (cameraSystem == null) return;

		Quaternion rotationChange = Quaternion.identity;

		if (Input.GetKeyDown(KeyCode.W))
			rotationChange = Quaternion.Euler(-90f, 0f, 0f);
		if (Input.GetKeyDown(KeyCode.S))
			rotationChange = Quaternion.Euler(90f, 0f, 0f);
		if (Input.GetKeyDown(KeyCode.A))
			rotationChange = Quaternion.Euler(0f, -90f, 0f);
		if (Input.GetKeyDown(KeyCode.D))
			rotationChange = Quaternion.Euler(0f, 90f, 0f);

		if (rotationChange != Quaternion.identity)
		{
			currentRotation *= rotationChange;
			cameraSystem.transform.DOLocalRotateQuaternion(currentRotation, 0.3f);
		}
	}


}
