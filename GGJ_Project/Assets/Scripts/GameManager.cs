public static class GameManager
{
    public static CameraController mainCameraController;
    public static PlayerController user;

    public static void StopFollowingPlayer()
    {
        mainCameraController.enabled = false;
    }
}
