using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{
	[SerializeField] private GameManager gameManager;
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

	private float GetXAxisRotation(Transform target)
	{
		Quaternion rotation = target.localRotation;
		float xRotation = 
			Mathf.Atan2(2f * (rotation.w * rotation.x + rotation.y * rotation.z), 
			1f - 2f * (rotation.x * rotation.x + rotation.y * rotation.y)) * Mathf.Rad2Deg;

		return xRotation;
	}

	public void FinishRotation(MouseManager mouseManager)
	{
		if (!rotating)
			return ;
		float rotateAngleX = RoundToNearest90(GetXAxisRotation(transform));
		float rotateDuration = GetXAxisRotation(transform) - rotateAngleX;
		rotateDuration = (Mathf.Abs(rotateDuration) / 90) * 0.5f;
		if (!Mathf.Approximately(rotateAngleX, 0) && !Mathf.Approximately(rotateAngleX, 360))
			gameManager.IncreaseRotateCount();
		Quaternion rotateQuaternion = Quaternion.Euler(rotateAngleX, 0, 0);
		transform.DOLocalRotateQuaternion(rotateQuaternion, rotateDuration).OnComplete(() =>
		{
			ReleaseBlocks();
			transform.localRotation = Quaternion.Euler(0, 0, 0);
			mouseManager.verticalRotateUnit = null;
			mouseManager.horizontalRotateUnit = null;
			CubeManager cubeManager = cube.GetComponent<CubeManager>();
			cubeManager.UpdateCube();
		});
	}

	public void ForceFinishRotation()
	{
		if (!rotating)
			return ;
		DOTween.Kill(transform);
		transform.localRotation = Quaternion.Euler(RoundToNearest90(transform.localEulerAngles.x), 0, 0);;
		ReleaseBlocks();
		transform.localRotation = Quaternion.Euler(0, 0, 0);
		cube.GetComponent<CubeManager>().UpdateCube();
	}
	public float RoundToNearest90(float value)
	{
		float sign = Mathf.Sign(value);
		value = Mathf.Abs(value) + 15f;
		value = Mathf.Round(value / 90f) * 90f;
		return (value * sign);
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