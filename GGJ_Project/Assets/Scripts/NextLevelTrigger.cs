using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class NextLevelTrigger : MonoBehaviour
{
	public float transitionDuration = 2;
	// TODO: would be nice to confirm that the string is a scene in the build settings
	// Custom Inspector GUI?
	public string nextScene;
	public bool shouldDestroyPlayer;
	
	private void OnTriggerEnter(Collider other)
	{
		GameManager.StopFollowingPlayer();
		StartCoroutine(NextLevelRoutine());
	}

	private IEnumerator NextLevelRoutine()
	{
		yield return new WaitForSeconds(transitionDuration);

		GameManager.DisablePlayer();
		SceneManager.LoadSceneAsync(nextScene);
		if (shouldDestroyPlayer)
		{
			Destroy(GameManager.user.gameObject);
			Destroy(GameManager.uiController.gameObject);
			Destroy(GameManager.mainCameraController.gameObject);
		}
	}
}
