using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToLevel(string name)
	{
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    public void QuitGame()
	{
        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
	}
}
