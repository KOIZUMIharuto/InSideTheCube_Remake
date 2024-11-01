using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class UI_UnitManager : MonoBehaviour
{
	[SerializeField] private GameObject cube;
	[SerializeField] private List<GameObject> blocks = new List<GameObject>();

	public bool rotating = false;

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

	public Tween RotateAnimation(bool rotateDirection, float tweenDuration)
	{
		float rotationAngle = rotateDirection ? 90 : -90;
		return (transform.DOLocalRotateQuaternion(Quaternion.Euler(rotationAngle, 0, 0), tweenDuration)
		.OnStart(() => {
			SetBlocksChildren();
		})
		.OnComplete(() =>{
			Debug.Log("RotateAnimation Complete");
			ReleaseBlocks();
			cube.GetComponent<UI_CubeManager>().UpdateCube();
			transform.localRotation = Quaternion.Euler(0, 0, 0);
		}));
	}

	public void AutoRotate(bool rotateDirection)
	{
		float rotationAngle = rotateDirection ? 90 : -90;
		SetBlocksChildren();
		transform.localRotation = Quaternion.Euler(rotationAngle, 0, 0);
		ReleaseBlocks();
		cube.GetComponent<UI_CubeManager>().UpdateCube();
		transform.localRotation = Quaternion.Euler(0, 0, 0);
	}

	public void SetBlocksChildren()
	{
		foreach (GameObject block in blocks)
			block.transform.parent = this.transform;
		rotating = true;
	}

	public void ReleaseBlocks()
	{
		foreach (GameObject block in blocks)
		{
			block.transform.parent = cube.transform;
			// block.GetComponent<UI_BlockManager>().UpdateBelongingUnit();
		}
		rotating = false;
	}
}