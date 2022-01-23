using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    private void Start()
    {
        GameManager.SpawnPlayer(transform.position);
    }
}
