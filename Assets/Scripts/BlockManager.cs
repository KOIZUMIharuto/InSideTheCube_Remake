using UnityEngine;
using System.Collections.Generic;

public class BlockManager : MonoBehaviour
{
	// 3つの UnitsManager を保持する
	[SerializeField] private GameObject cube;
	[SerializeField] private UnitsManager unitsManagerFSB;
	[SerializeField] private UnitsManager unitsManagerUED;
	[SerializeField] private UnitsManager unitsManagerRML;

	[SerializeField] private List<GameObject> Panels = new List<GameObject>();

	public Vector3 attachedUnit = Vector3.zero;

	void Start()
	{
		foreach (Transform child in transform)
			Panels.Add(child.gameObject);
	}
	public void	UpdateBelongingUnit()
	{
		Vector3 localPosition = cube.transform.InverseTransformPoint(this.gameObject.transform.position);
		attachedUnit.x = unitsManagerFSB.AddObjectToList(this.gameObject, localPosition.x);
		attachedUnit.y = unitsManagerUED.AddObjectToList(this.gameObject, localPosition.y);
		attachedUnit.z = unitsManagerRML.AddObjectToList(this.gameObject, localPosition.z);
		foreach (GameObject panel in Panels)
			panel.GetComponent<PanelManager>().getPanelDirection();
	}
}
