using UnityEngine;
using System.Collections.Generic;

public class CubeManager : MonoBehaviour
{
	[SerializeField] private List<GameObject> blocks = new List<GameObject>();

	public void UpdateCube()
	{
		foreach (GameObject block in blocks)
			block.GetComponent<BlockManager>().UpdateBelongingUnit();
	}

	public void UpdatePanelRotationStatus()
	{
		foreach (GameObject block in blocks)
			block.GetComponent<BlockManager>().UpdatePanelRotationStatus();
	}

	public void ToggleCollider(bool onOff)
	{
		foreach (GameObject block in blocks)
		{
			block.GetComponent<BlockManager>().ToggleCollider(onOff);
		}
	}
}