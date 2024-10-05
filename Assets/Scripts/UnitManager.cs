using UnityEngine;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{

	[SerializeField] private GameObject cube;
	[SerializeField] private List<GameObject> blocks = new List<GameObject>();

	public bool Contains(GameObject obj)
	{
		return blocks.Contains(obj);
	}

	public void Add(GameObject obj)
	{
		blocks.Add(obj);
	}

	public void Remove(GameObject obj)
	{
		blocks.Remove(obj);
	}
}