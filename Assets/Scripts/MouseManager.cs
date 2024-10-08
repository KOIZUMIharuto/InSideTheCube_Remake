using UnityEngine;

public class MouseManager : MonoBehaviour
{
	[SerializeField] private GameObject cube;
	[SerializeField] private LayerMask ignoreLayerMask;
	[SerializeField] private string targetTag = "ColorPanel";

	private IClickable clickedPanelComponent;
	public GameObject clickedPanel;

	private Vector2 startMousePosition;
	private Vector2 currentMousePosition;
	public Vector2 deltaDirection;

	public UnitManager verticalRotateUnit;
	private bool verticalRotateDirection;
	public UnitManager horizontalRotateUnit;
	private bool horizontalRotateDirection;
	

	void Update()
	{
		// マウスボタンが押された瞬間
		if (Input.GetMouseButtonDown(0) && clickedPanel == null)
			OnMouseDown();

		// マウスを押している間の処理
		if (Input.GetMouseButton(0) && clickedPanelComponent != null)
			WhileMouseDown();

		// マウスが離された瞬間
		if (Input.GetMouseButtonUp(0) && clickedPanelComponent != null)
			OnMouseRelease();
	}

	private void OnMouseDown()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreLayerMask))
		{
			if (hit.collider.gameObject.tag == targetTag)
			{
				clickedPanel = hit.collider.gameObject;
				clickedPanelComponent = hit.collider.gameObject.GetComponent<IClickable>();
				if (clickedPanelComponent != null)
				{
					if (verticalRotateUnit && horizontalRotateUnit)
					{
						if (verticalRotateUnit.rotating || horizontalRotateUnit.rotating)
							return;
					}
					clickedPanelComponent.OnClickDown();
					startMousePosition = Input.mousePosition;

					return;
				}
			}
		}
	}

	private void WhileMouseDown()
	{
		if (verticalRotateUnit == null || horizontalRotateUnit == null)
			return;
		currentMousePosition = Input.mousePosition;

		Vector2 delta = currentMousePosition - startMousePosition;

		float difference = Mathf.Abs(Mathf.Abs(delta.x) - Mathf.Abs(delta.y));
		float threshold = 10f;
		if (difference > threshold)
		{
			if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
				deltaDirection = new Vector2(Mathf.Sign(delta.x) * (difference - threshold), 0);
			else
				deltaDirection = new Vector2(0, Mathf.Sign(delta.y) * (difference - threshold));
		}
		else
			deltaDirection = Vector2.zero;
		if (deltaDirection.x != 0)
		{
			verticalRotateUnit.ForceFinishRotation();
			horizontalRotateUnit.Rotate(deltaDirection.x, horizontalRotateDirection);
		}
		else
			horizontalRotateUnit.ForceFinishRotation();
		if (deltaDirection.y != 0)
		{
			horizontalRotateUnit.ForceFinishRotation();
			verticalRotateUnit.Rotate(deltaDirection.y, verticalRotateDirection);
		}
		else
			verticalRotateUnit.ForceFinishRotation();
	}

	private void OnMouseRelease()
	{
		clickedPanel = null;
		clickedPanelComponent = null;
		deltaDirection = Vector2.zero;
		if (verticalRotateUnit == null || horizontalRotateUnit == null)
			return;
		verticalRotateUnit.FinishRotation(this);
		horizontalRotateUnit.FinishRotation(this);
	}

	public void SetRotationUnit(UnitManager vertical, bool verticalDirection, UnitManager horizontal, bool horizontalDirection)
	{
		verticalRotateUnit = vertical;
		verticalRotateDirection = verticalDirection;
		horizontalRotateUnit = horizontal;
		horizontalRotateDirection = horizontalDirection;
	}


}
