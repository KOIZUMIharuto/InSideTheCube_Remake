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
		for (int i = 0; i < 15; i++)
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
		Debug.Log("KillAnimation");
		sequence.Complete();
		sequence.Kill();
		sequence = null;
		ResetCube();
	}

	public void shuffle()
	{
		Debug.Log("Shuffle");
		if (sequence == null)
		{
			makeShuffleSequence();
			sequence.Restart();
		}
	}

	public void makeShuffleSequence()
	{
		Debug.Log("MakeShuffleSequence");
		sequence = DOTween.Sequence();
		foreach (Dictionary<string, int> rotate in rotateOrder)
			sequence.Append(units[rotate["units"]].RotateAnimation(rotate["direction"] == 1, 0.1f, rotate["unit"]));
		sequence.AppendInterval(0.6f);
		sequence.OnComplete(() =>
		{
			ResetCube();
			sequence.Kill();
			sequence = null;
			shuffle();
		});
		sequence.Pause();
	}

	public void play()
	{
		Debug.Log("Play");
		if (sequence == null)
		{
			SetShuffled();
			makePlaySequence();
			sequence.Restart();
		}

	}


	public void makePlaySequence()
	{
		Debug.Log("MakeShuffleSequence");
		sequence = DOTween.Sequence();
		foreach (Dictionary<string, int> rotate in rotateOrder)
			sequence.Prepend(units[rotate["units"]].RotateAnimation(rotate["direction"] != 1, 0.1f, rotate["unit"]));
		sequence.AppendInterval(0.6f);
		sequence.OnComplete(() =>
		{
			ResetCube();
			sequence.Kill();
			sequence = null;
			play();
		});
		sequence.Pause();
	}

	public void retire()
	{

	}

	public void reset()
	{
		
	}

	private void SetShuffled()
	{
		foreach (Dictionary<string, int> rotate in rotateOrder)
			units[rotate["units"]].AutoRotate(rotate["direction"] == 1, rotate["unit"]);
	}

	private void ResetCube()
	{
		foreach (GameObject block in blocks)
		{
			block.transform.parent = this.transform;
			block.GetComponent<UI_BlockManager>().MoveToHomePosition();
			block.GetComponent<UI_BlockManager>().UpdateBelongingUnit();
		}
		Debug.Log("ResetCube");
	}
}