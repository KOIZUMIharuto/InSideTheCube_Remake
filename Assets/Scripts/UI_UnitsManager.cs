using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class UI_UnitsManager : MonoBehaviour
{
	[SerializeField] private UI_UnitManager top;
	[SerializeField] private UI_UnitManager center;
	[SerializeField] private UI_UnitManager bottom;

	private List<UI_UnitManager> units = new List<UI_UnitManager>();

	void Start()
	{
		units.Add(top);
		units.Add(center);
		units.Add(bottom);
	}

	public void AddObjectToUnit(GameObject obj, float position)
	{
		UI_UnitManager unitToRemove = null;
		UI_UnitManager unitToAdd = null;

		foreach (UI_UnitManager unit in units)
		{
			if (unit.Contains(obj))
			{
				unitToRemove = unit;
				break;
			}
		}

		if (position > 5)
			unitToAdd = top;
		else if (position >= -5)
			unitToAdd = center;
		else
			unitToAdd = bottom;

		if (unitToAdd != unitToRemove)
		{
			unitToAdd.Add(obj);
			if (unitToRemove)
				unitToRemove.Remove(obj);
		}
	}

	public Tween RotateAnimation(bool rotateDirection, float tweenDuration, int rotateUnit)
	{
		UI_UnitManager unitToRotate = (0 <= rotateUnit && rotateUnit < units.Count) ? units[rotateUnit] : null;
		if (unitToRotate == null)
			return null;
		return unitToRotate.RotateAnimation(rotateDirection, tweenDuration);
	}

	public void AutoRotate(bool rotateDirection, int rotateUnit)
	{
		UI_UnitManager unitToRotate = (0 <= rotateUnit && rotateUnit < units.Count) ? units[rotateUnit] : null;
		unitToRotate.AutoRotate(rotateDirection);
	}
}
