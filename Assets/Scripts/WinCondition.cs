using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private EventManager eventManager;
    [SerializeField] private LevelLoader levelLoader;

    private int enemiesLeft = 0;

    public void Start()
    {
        eventManager.LevelWon.AddListener(OnWin);
    }

    public void OnDestroy()
    {
        eventManager.LevelWon.RemoveListener(OnWin);
    }

    public void AddEnemy(GridBehaviour enemy)
    {
        enemiesLeft++;
    }

    // Update is called once per frame
    public void EnemyDead(GridBehaviour enemy)
    {
        enemiesLeft--;
        if (enemiesLeft == 0)
            eventManager.LevelWon?.Invoke();
    }

    public void OnWin()
    {
        levelLoader.LoadLevel();
    }
}
