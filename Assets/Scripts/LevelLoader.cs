using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Main = null;

    public Animator transition;


    private void Awake()
    {
        Main = this;
    }


    public void LoadLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        StartCoroutine(ExitGameCoroutine());
    }

    IEnumerator ExitGameCoroutine()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        Application.Quit(0);
    }

    public void LoadLevel(int buildIndex)
    {
        StartCoroutine(LoadLevelCoroutine(() =>
        {
            SceneManager.LoadScene(buildIndex);
            return SceneManager.GetSceneByBuildIndex(buildIndex);
        }));
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadLevelCoroutine(() =>
        {
            SceneManager.LoadScene(sceneName: sceneName);
            return SceneManager.GetSceneByName(sceneName);
        }));
    }

    public void LoadLevel(Level level)
    {
        LoadLevel(level.sceneName);
    }

    IEnumerator LoadLevelCoroutine(Func<Scene> loadScene)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        var scene = loadScene();
    }
}
