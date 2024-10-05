using UnityEngine;

public class MouseManager : MonoBehaviour
{
	[SerializeField] private LayerMask ignoreLayerMask;
	[SerializeField] private string targetTag = "ColorPanel";

	private IClickable clickedPanelComponent;
	public GameObject clickedPanel;

	private Vector2 startMousePosition;
	private Vector2 currentMousePosition;
	public Vector2 deltaDirection;

	void Update()
	{
		// マウスボタンが押された瞬間
		if (Input.GetMouseButtonDown(0))
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
						clickedPanelComponent.OnClickDown();
						startMousePosition = Input.mousePosition;

						return;
					}
				}
			}
		}

		// マウスを押している間の処理
		if (Input.GetMouseButton(0) && clickedPanelComponent != null)
		{
			currentMousePosition = Input.mousePosition;

			Vector2 delta = currentMousePosition - startMousePosition;

			float difference = Mathf.Abs(Mathf.Abs(delta.x) - Mathf.Abs(delta.y));
			float threshold = 10f;
			if (difference < threshold)
				return;
			if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
				deltaDirection = new Vector2(Mathf.Sign(delta.x) * (difference - threshold), 0);
			else
				deltaDirection = new Vector2(0, Mathf.Sign(delta.y) * (difference - threshold));

			clickedPanelComponent.OnClickHold();
		}

		// マウスが離された瞬間
		if (Input.GetMouseButtonUp(0) && clickedPanelComponent != null)
		{
			clickedPanelComponent.OnClickUp();
			clickedPanel = null;
			clickedPanelComponent = null;
			deltaDirection = Vector2.zero;
		}
	}
}
