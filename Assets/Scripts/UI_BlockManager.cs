using UnityEngine;
using System;
using DG.Tweening;

public class UI_BlockManager : MonoBehaviour
{
	[SerializeField] private UI_CubeManager cube;
	[SerializeField] private UI_UnitsManager units_FSB;
	[SerializeField] private UI_UnitsManager units_UED;
	[SerializeField] private UI_UnitsManager units_RML;

	private Vector3 homePosition;

	void Start()
	{
		homePosition = transform.localPosition;
		cube.setBlock(gameObject);
		UpdateBelongingUnit();
	}

	public void SetHomePosition()
	{
		transform.localPosition = homePosition;
	}

	public void MoveToHomePosition()
	{
		transform.localPosition = homePosition;
		transform.localRotation = Quaternion.Euler(0, 0, 0);
	}

	public void UpdateBelongingUnit()
	{
		units_FSB.AddObjectToUnit(this.gameObject, transform.localPosition.x);
		units_UED.AddObjectToUnit(this.gameObject, transform.localPosition.y);
		units_RML.AddObjectToUnit(this.gameObject, transform.localPosition.z);
	}
}
