using Unity.Mathematics;
using UnityEngine;

public static class GameManager
{
    public static CameraController mainCameraController;
    public static PlayerController user;
    public static UIController uiController;

    private static bool _hasSpawnedPlayer;
    private static GameSettings _settings;

    private const string GAME_SETTINGS_NAME = "GameSettings";

    static GameManager()
    {
        _hasSpawnedPlayer = false;
        _settings = Resources.Load<GameSettings>(GAME_SETTINGS_NAME);
    }

    public static void StopFollowingPlayer()
    {
        mainCameraController.enabled = false;
    }

    public static void DisablePlayer()
    {
        user.enabled = false;
    }
    
    public static void SpawnPlayer(Vector3 position)
    {
        if (!_hasSpawnedPlayer)
        {
            var playerGO = GameObject.Instantiate(_settings.playerPrefab, position, quaternion.identity);
            user = playerGO.GetComponent<PlayerController>();

            var cameraGO = GameObject.Instantiate(_settings.cameraPrefab, position, quaternion.identity);
            mainCameraController = cameraGO.GetComponent<CameraController>();
            mainCameraController.player = playerGO.transform;

            mainCameraController.enabled = true;

            var uiGO = GameObject.Instantiate(_settings.uiPrefab, Vector3.zero, Quaternion.identity);
            uiController = uiGO.GetComponent<UIController>();

            user.ui = uiController;

            _hasSpawnedPlayer = true;
        }
        else
        {
            user.enabled = true;
            user.Respawn(position);

            mainCameraController.enabled = true;
        }
    }
}
