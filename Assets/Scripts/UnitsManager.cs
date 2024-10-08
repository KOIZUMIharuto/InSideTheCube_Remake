using UnityEngine;
using System.Collections.Generic;

public class UnitsManager : MonoBehaviour
{
	[SerializeField] private UnitManager top;
	[SerializeField] private UnitManager center;
	[SerializeField] private UnitManager bottom;

	public int AddObjectToList(GameObject obj, float position)
	{
		UnitManager unitToremove;
		UnitManager unitToAdd;

		if (top.Contains(obj))
			unitToremove = top;
		else if (center.Contains(obj))
			unitToremove = center;
		else if (bottom.Contains(obj))
			unitToremove = bottom;
		else
			unitToremove = null;

		if (position > 5)
			unitToAdd = top;
		else if (position >= -5)
			unitToAdd = center;
		else
			unitToAdd = bottom;

		if (unitToAdd != unitToremove)
		{
			unitToAdd.Add(obj);
			if (unitToremove != null)
				unitToremove.Remove(obj);
		}

		if (unitToAdd == top)
			return 1;
		else if (unitToAdd == center)
			return 0;
		else if (unitToAdd == bottom)
			return -1;
		else
			return -2;
	}

	public UnitManager GetUnit(int unit)
	{
		if (unit == 1)
			return top;
		else if (unit == 0)
			return center;
		else if (unit == -1)
			return bottom;
		else
			return null;
	}
}
