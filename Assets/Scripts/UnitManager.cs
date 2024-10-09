using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
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

	public Tween AutoRotate(bool rotateDirection, float tweenDuration)
	{
		float rotationAngle = rotateDirection ? 90 : -90;
		return (transform.DOLocalRotateQuaternion(Quaternion.Euler(rotationAngle, 0, 0), tweenDuration)
		.OnStart(() => {
			SetBlocksChildren();
		})
		.OnComplete(() =>{
			ReleaseBlocks();
			transform.localRotation = Quaternion.Euler(0, 0, 0);
			cube.GetComponent<CubeManager>().UpdateCube();
		}));
	}

	public void Rotate(float mouseDeltaDirection, bool rotateDirection)
    {
        if (!rotating)
            SetBlocksChildren();
        if (!rotateDirection)
            mouseDeltaDirection *= -1;

        float rotationAngle = mouseDeltaDirection / 5;
        transform.localRotation = Quaternion.Euler(rotationAngle, 0, 0);
    }

	public void FinishRotation(MouseManager mouseManager)
	{
		if (!rotating)
			return ;
		float rotateDuration = transform.localEulerAngles.x - RoundToNearest90(transform.localEulerAngles.x);
		rotateDuration = (Mathf.Abs(rotateDuration) / 90) * 0.5f;
		Quaternion rotateQuaternion = Quaternion.Euler(RoundToNearest90(transform.localEulerAngles.x), 0, 0);
		transform.DOLocalRotateQuaternion(rotateQuaternion, rotateDuration).OnComplete(() =>
		{
			ReleaseBlocks();
			transform.localRotation = Quaternion.Euler(0, 0, 0);
			mouseManager.verticalRotateUnit = null;
			mouseManager.horizontalRotateUnit = null;
			cube.GetComponent<CubeManager>().UpdateCube();
		});
	}

	public void ForceFinishRotation()
	{
		if (!rotating)
			return ;
		//stop Dotween
		DOTween.Kill(transform);
		transform.localRotation = Quaternion.Euler(RoundToNearest90(transform.localEulerAngles.x), 0, 0);;
		ReleaseBlocks();
		transform.localRotation = Quaternion.Euler(0, 0, 0);
		cube.GetComponent<CubeManager>().UpdateCube();
	}
	public float RoundToNearest90(float value)
	{
		return Mathf.Round(value / 90f) * 90f;
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
			block.GetComponent<BlockManager>().SnapLocalPosition(RoundToNearest90);
		}
		rotating = false;
	}
}