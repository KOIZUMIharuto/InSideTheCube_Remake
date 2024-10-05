using UnityEngine;

public class ClickableObject : MonoBehaviour, IClickable
{
	public bool isClicked = false;

	// クリックが開始された時に呼ばれる
	public void OnClickDown()
	{
		isClicked = true;
		Debug.Log($"{gameObject.name}がクリックされました");
		// GetComponent<Renderer>().material.color = Color.red;  // クリックされたら色を変える
	}

	// クリックが続いている間に呼ばれる
	public void OnClickHold()
	{
		// 必要に応じてクリック保持中の処理をここに追加
		// Debug.Log($"{gameObject.name}がクリック中です");
	}

	// クリックが解除された時に呼ばれる
	public void OnClickUp()
	{
		isClicked = false;
		Debug.Log($"{gameObject.name}のクリックが解除されました");
		// GetComponent<Renderer>().material.color = Color.white;  // 元の色に戻す
	}
}
