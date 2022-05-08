using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Configs/Levels")]
public class LevelsConfig : ScriptableObject
{
    public List<LevelPack> packs;

    public Level GetLevelByScene(Scene scene)
    {
        return packs.SelectMany(pack => pack.levels)
            .First(level => level.sceneName == scene.name);
    }

    public Level GetNextLevel(Level level)
    {
        var pack = packs.Find(p => p.levels.Contains(level));
        var levelIndex = pack.levels.IndexOf(level);
        var nextIndex = levelIndex + 1;
        if (nextIndex >= pack.levels.Count) return null;
        return pack.levels[nextIndex];
    }
}


[Serializable]
public class LevelPack
{
    [Guid] public string id;
    public int coinsToUnlock;
    public int starsToUnlock;
    public List<Level> levels;
    [NonSerialized] public bool enabled;
}


[Serializable]
public class Level
{
    [Guid] public string id;
    public string sceneName;
    public int totalStars = 3;
    [NonSerialized] public bool enabled = false;
    [NonSerialized] public bool complete = false;
}


public class GuidAttribute : PropertyAttribute
{
}