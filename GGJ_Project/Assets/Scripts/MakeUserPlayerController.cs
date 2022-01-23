using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class MakeUserPlayerController : MonoBehaviour
{
	private void Awake()
	{
		GameManager.user = GetComponent<PlayerController>();
	}
}
