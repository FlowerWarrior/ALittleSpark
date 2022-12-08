using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr instance;

    private void Awake()
    {
        instance = this;
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadNextLevel()
    {
        int currentSceneId = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneId == 2)
        {
            SceneManager.LoadScene(0);
            return;
        }

        SceneManager.LoadScene(currentSceneId + 1);
    }
}
