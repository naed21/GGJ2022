using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class NextLevelTrigger : MonoBehaviour
{
	public float transitionDuration = 2;
	
	private void OnTriggerEnter(Collider other)
	{
		GameManager.StopFollowingPlayer();
		StartCoroutine(NextLevelRoutine());
	}

	private IEnumerator NextLevelRoutine()
	{
		yield return new WaitForSeconds(transitionDuration);

		SceneManager.LoadSceneAsync("town");
	}
}
