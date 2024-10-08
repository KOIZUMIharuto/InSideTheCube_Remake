using UnityEngine;
using System.Collections.Generic;

public class PanelManager : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField] private GameObject cube;
	[SerializeField] private UnitsManager unitsManagerFSB;
	[SerializeField] private UnitsManager unitsManagerUED;
	[SerializeField] private UnitsManager unitsManagerRML;
	[SerializeField] private CameraManager cameraManager;

	private GameObject parentBlock;
	public Vector3 panelDirection;

	public UnitsManager verticalRotateUnit;
	public bool verticalRotateDirection;
	public UnitsManager horizontalRotateUnit;
	public bool horizontalRotateDirection;

	void Start()
	{
		parentBlock = transform.parent.gameObject;
	}
	public void getPanelDirection()
	{
		panelDirection = cube.transform.InverseTransformDirection(transform.up);
		panelDirection = new Vector3(Mathf.Round(panelDirection.x), Mathf.Round(panelDirection.y), Mathf.Round(panelDirection.z));

		Vector3 attachedUnit = transform.parent.GetComponent<BlockManager>().attachedUnit;
		UpdateRotationStatus();
	}

	public void UpdateRotationStatus()
	{
		if (cameraManager.upDirection != panelDirection && cameraManager.upDirection != -panelDirection)
		{
			if (cameraManager.upDirection.x != 0)
				getHorizontalRotation_1((int)cameraManager.upDirection.x, unitsManagerFSB);
			else if (cameraManager.upDirection.y != 0)
				getHorizontalRotation_1((int)cameraManager.upDirection.y, unitsManagerUED);
			else if (cameraManager.upDirection.z != 0)
				getHorizontalRotation_1((int)cameraManager.upDirection.z, unitsManagerRML);
		}
		else
		{
			if (cameraManager.forwardDirection.x != 0)
				getHorizontalRotation_2((int)cameraManager.forwardDirection.x, unitsManagerFSB);
			else if (cameraManager.forwardDirection.y != 0)
				getHorizontalRotation_2((int)cameraManager.forwardDirection.y, unitsManagerUED);
			else if (cameraManager.forwardDirection.z != 0)
				getHorizontalRotation_2((int)cameraManager.forwardDirection.z, unitsManagerRML);
		}

		if (cameraManager.rightDirection != panelDirection && cameraManager.rightDirection != -panelDirection)
		{
			if (cameraManager.rightDirection.x != 0)
				verticalRotateDirection_1((int)cameraManager.rightDirection.x, unitsManagerFSB);
			else if (cameraManager.rightDirection.y != 0)
				verticalRotateDirection_1((int)cameraManager.rightDirection.y, unitsManagerUED);
			else if (cameraManager.rightDirection.z != 0)
				verticalRotateDirection_1((int)cameraManager.rightDirection.z, unitsManagerRML);
		}
		else
		{
			if (cameraManager.forwardDirection.x != 0)
				verticalRotateDirection_2((int)cameraManager.forwardDirection.x, unitsManagerFSB);
			else if (cameraManager.forwardDirection.y != 0)
				verticalRotateDirection_2((int)cameraManager.forwardDirection.y, unitsManagerUED);
			else if (cameraManager.forwardDirection.z != 0)
				verticalRotateDirection_2((int)cameraManager.forwardDirection.z, unitsManagerRML);
		}
	}

	private void getHorizontalRotation_1(int cameraDirection, UnitsManager unitsManager)
	{
		horizontalRotateUnit = unitsManager;
		horizontalRotateDirection = (cameraDirection > 0);
	}

	private void getHorizontalRotation_2(int cameraDirection, UnitsManager unitsManager)
	{
		horizontalRotateUnit = unitsManager;
		horizontalRotateDirection = ((cameraManager.upDirection != panelDirection) == (cameraDirection > 0));
	}

	private void verticalRotateDirection_1(int cameraDirection, UnitsManager unitsManager)
	{
		verticalRotateUnit = unitsManager;
		verticalRotateDirection = (cameraDirection < 0);
	}

	private void verticalRotateDirection_2(int cameraDirection, UnitsManager unitsManager)
	{
		verticalRotateUnit = unitsManager;
		verticalRotateDirection = ((cameraManager.rightDirection == panelDirection) == (cameraDirection > 0));
	}

	public void SetRotationUnit()
	{
		parentBlock.GetComponent<BlockManager>().SetRotationUnit(this);
	}

	// Add your custom methods and logic here
}