using UnityEngine;

[RequireComponent(typeof(CameraController))]
public class MakeMainCameraController : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		GameManager.mainCameraController = GetComponent<CameraController>();
	}
}
