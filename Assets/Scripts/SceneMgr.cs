using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr instance;

    [SerializeField] GameObject gameCompletedScreen;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // limit fps
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        QualitySettings.vSyncCount = 1;

        if (GetCurrentSceneId() == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private int GetCurrentSceneId()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadNextLevel()
    {
        int currentSceneId = GetCurrentSceneId();

        if (currentSceneId == 2)
        {
            StartCoroutine(GameCompletedRoutine());
            return;
        }

        SceneManager.LoadScene(currentSceneId + 1);
    }

    IEnumerator GameCompletedRoutine()
    {
        gameCompletedScreen.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        LoadMenu();
    }
}
