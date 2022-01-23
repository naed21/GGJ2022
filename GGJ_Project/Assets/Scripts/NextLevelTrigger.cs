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
	}
}
