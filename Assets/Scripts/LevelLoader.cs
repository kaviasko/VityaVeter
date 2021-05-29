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
        StartCoroutine(LoadLevelCoroutine(SceneManager.GetActiveScene().buildIndex + 1));
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
        StartCoroutine(LoadLevelCoroutine(buildIndex));
    }

    IEnumerator LoadLevelCoroutine(int buildindex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(buildindex);
    }
}
