using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.Scripts.Progress;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private EventManager eventManager;
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private LevelsConfig levelsConfig;

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
        if (levelsConfig != null)
        {
            levelsConfig.GetLevelByScene(gameObject.scene).complete = true;
            Level thisLevel = levelsConfig.GetLevelByScene(gameObject.scene);
            Level nextLevel = levelsConfig.GetNextLevel(thisLevel);

            thisLevel.complete = true;
            nextLevel.enabled = true;
            levelsConfig.Save();

            levelLoader.LoadLevel(nextLevel);
        }
        else
        {
            Debug.LogError($"Win Condition of level '{gameObject.scene.name}' has no LevelsConfig. Please set it :*", this);
            Debug.Break();

            levelLoader.LoadLevel();
        }
    }
}
