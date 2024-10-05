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
	public Vector3 panelDirection;

	[SerializeField] private List<GameObject> rotateUnits = new List<GameObject>();

	public GameObject verticalUnit;
	public GameObject horizontalUnit;

	public void getPanelDirection()
	{
		panelDirection = cube.transform.InverseTransformDirection(transform.up);
		panelDirection = new Vector3(Mathf.Round(panelDirection.x), Mathf.Round(panelDirection.y), Mathf.Round(panelDirection.z));

		Vector3 attachedUnit = transform.parent.GetComponent<BlockManager>().attachedUnit;
		if (panelDirection.x == 0)
			rotateUnits.Add(unitsManagerFSB.GetUnit((int)attachedUnit.x));
		if (panelDirection.y == 0)
			rotateUnits.Add(unitsManagerUED.GetUnit((int)attachedUnit.y));
		if (panelDirection.z == 0)
			rotateUnits.Add(unitsManagerRML.GetUnit((int)attachedUnit.z));
		
	}

	// Add your custom methods and logic here
}