using UnityEngine;

public class ClickableObject : MonoBehaviour, IClickable
{
	public void OnClickDown()
	{
		transform.parent.GetComponent<PanelManager>().SetRotationUnit();
	}

}
