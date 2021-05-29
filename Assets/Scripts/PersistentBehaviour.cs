using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PersistentBehaviour : MonoBehaviour
{
    [SerializeField] private bool[] hideOnMenu;
    private static Dictionary<string, PersistentBehaviour> savedObjects = new Dictionary<string, PersistentBehaviour>();

    private void Start()
    {
        if (savedObjects.ContainsKey(name))
        {
            Destroy(gameObject);
            return;
        }
        savedObjects.Add(name, this);
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        CheckScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckScene(scene.buildIndex);
    }

    private void CheckScene(int index)
    {
        if (index < 0) index = 0;
        bool hide = index < hideOnMenu.Length && hideOnMenu[index];
        gameObject.SetActive(!hide);
    }
}