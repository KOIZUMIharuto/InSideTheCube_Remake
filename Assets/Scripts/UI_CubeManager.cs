using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class UI_CubeManager : MonoBehaviour
{
	[SerializeField] private UI_UnitsManager units_FSB;
	[SerializeField] private UI_UnitsManager units_UED;
	[SerializeField] private UI_UnitsManager units_RML;
	private List<UI_UnitsManager> units = new List<UI_UnitsManager>();
	private List<GameObject> blocks = new List<GameObject>();
	private bool do_animation;

	private List<Dictionary<string, int>> rotateOrder = new List<Dictionary<string, int>>();

	private Sequence sequence;

	void Start()
	{
		units.Add(units_FSB);
		units.Add(units_UED);
		units.Add(units_RML);
		for (int i = 0; i < 10; i++)
		{
			Dictionary<string, int> order = new Dictionary<string, int>();
			order.Add("units", Random.Range(0, 3));
			order.Add("unit", Random.Range(0, 3));
			order.Add("direction", Random.Range(0, 2));
			rotateOrder.Add(order);
		}
		sequence = null;
		do_animation = false;
	}

	public void setBlock(GameObject block)
	{
		blocks.Add(block);
	}

	public void UpdateCube()
	{
		foreach (GameObject block in blocks)
			block.GetComponent<UI_BlockManager>().UpdateBelongingUnit();
	}

	public void KillAnimation()
	{
		do_animation = false;
	}

	public void shuffle()
	{
		if (sequence != null)
			return;
		do_animation = true;
		ResetCube();
		sequence = DOTween.Sequence();
		foreach (Dictionary<string, int> rotate in rotateOrder)
		{
			Sequence onceRotateSequence = DOTween.Sequence();
			onceRotateSequence.Append(units[rotate["units"]].AutoRotate(rotate["direction"] == 1, 0.2f, rotate["unit"]));
			onceRotateSequence.OnStart(() =>
			{
				if (!do_animation)
				{
					onceRotateSequence.Kill();
					sequence.Kill();
					sequence = null;
					ResetCube();
				}
			});
			sequence.Append(onceRotateSequence);
		}
		sequence.OnStepComplete(() =>
		{
			ResetCube();
			if (!do_animation)
			{
				sequence.Kill();
				sequence = null;
			}

		});
		sequence.SetLoops(-1, LoopType.Restart);
		sequence.OnKill(() =>
		{
			sequence = null;
			ResetCube();
		});
	}

	public void play()
	{

	}

	public void retire()
	{

	}

	public void reset()
	{
		
	}

	private void ResetCube()
	{
		foreach (GameObject block in blocks)
		{
			block.GetComponent<UI_BlockManager>().MoveToHomePosition();
		}
		UpdateCube();
	}
}