using UnityEngine;
using System;
using System.Collections.Generic;

public class BlockManager : MonoBehaviour
{
	// 3つの UnitsManager を保持する
	[SerializeField] private MouseManager mouseManager;
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

	public void UpdatePanelRotationStatus()
	{
		foreach (GameObject panel in Panels)
			panel.GetComponent<PanelManager>().UpdateRotationStatus();
	}

	public void SetRotationUnit(PanelManager clickedPanel)
	{
		UnitManager verticalUnit = null;
		bool verticalDirection = clickedPanel.verticalRotateDirection;
		UnitManager horizontalUnit = null;
		bool horizontalDirection = clickedPanel.horizontalRotateDirection;

		if (clickedPanel.verticalRotateUnit != null)
		{
			if (clickedPanel.verticalRotateUnit == unitsManagerFSB)
				verticalUnit = unitsManagerFSB.GetUnit((int)attachedUnit.x);
			else if (clickedPanel.verticalRotateUnit == unitsManagerUED)
				verticalUnit = unitsManagerUED.GetUnit((int)attachedUnit.y);
			else if (clickedPanel.verticalRotateUnit == unitsManagerRML)
				verticalUnit = unitsManagerRML.GetUnit((int)attachedUnit.z);
		}
		if (clickedPanel.horizontalRotateUnit != null)
		{
			if (clickedPanel.horizontalRotateUnit == unitsManagerFSB)
				horizontalUnit = unitsManagerFSB.GetUnit((int)attachedUnit.x);
			else if (clickedPanel.horizontalRotateUnit == unitsManagerUED)
				horizontalUnit = unitsManagerUED.GetUnit((int)attachedUnit.y);
			else if (clickedPanel.horizontalRotateUnit == unitsManagerRML)
				horizontalUnit = unitsManagerRML.GetUnit((int)attachedUnit.z);
		}
		mouseManager.SetRotationUnit(verticalUnit, verticalDirection, horizontalUnit, horizontalDirection);
	}
	private float SnapToNearest(float value)
	{
		if (value < -5f)
			return -10f;
		else if (value < 5f)
			return 0f;
		else
			return 10f;
	}

	public void SnapLocalPosition(Func<float, float> RoundToNearest90)
	{
		// 現在のローカル座標を取得
		Vector3 localPosition = transform.localPosition;

		// x, y, z軸それぞれに最も近い -10, 0, 10 の値にスナップ
		localPosition.x = SnapToNearest(localPosition.x);
		localPosition.y = SnapToNearest(localPosition.y);
		localPosition.z = SnapToNearest(localPosition.z);

		Vector3 localRotation = transform.localEulerAngles;
		localRotation.x = RoundToNearest90(localRotation.x);
		localRotation.y = RoundToNearest90(localRotation.y);
		localRotation.z = RoundToNearest90(localRotation.z);

		// 新しいローカル座標に設定
		transform.localPosition = localPosition;
		transform.localEulerAngles = localRotation;
	}

	public void ToggleCollider(bool onOff)
	{
		foreach (GameObject panel in Panels)
			panel.GetComponent<BoxCollider>().enabled = onOff;
	}

}
