using UnityEngine;
using System.Collections.Generic;

public class CubeManager : MonoBehaviour
{
	[SerializeField] private List<GameObject> blocks = new List<GameObject>();

	// Start is called before the first frame update
	void Start()
	{
		// Initialization code here
		foreach (GameObject block in blocks)
			block.GetComponent<BlockManager>().UpdateBelongingUnit();
	}

	// Update is called once per frame
	void Update()
	{
		// Frame update code here
	}
}