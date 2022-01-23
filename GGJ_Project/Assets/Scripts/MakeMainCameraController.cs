using UnityEngine;

[RequireComponent(typeof(CameraController))]
public class MakeMainCameraController : MonoBehaviour
{
	private void Awake()
	{
		GameManager.mainCameraController = GetComponent<CameraController>();
	}
}
