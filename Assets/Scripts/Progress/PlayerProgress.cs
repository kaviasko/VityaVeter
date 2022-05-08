using System;
using UnityEngine;

namespace Assets.Scripts.Progress
{
    public static class PlayerProgress
    {
        public static bool Init(this LevelsConfig levelsConfig)
        {
            if (PlayerPrefs2.GetBool(InitializedKey)) return false;

            PlayerPrefs2.SetBool(InitializedKey, true);

            var firstPack = levelsConfig.packs[0];
            firstPack.enabled = true;

            var firstLevel = firstPack.levels[0];
            firstLevel.enabled = true;

            Save(levelsConfig);

            return true;
        }

        public static void Load(this LevelsConfig levelsConfig)
        {
            foreach (var pack in levelsConfig.packs)
            {
                pack.enabled = IsEnabled(pack);
                foreach (var level in pack.levels)
                {
                    LevelStatus status = GetLevelStatus(level.id);
                    level.complete = status == LevelStatus.Complete;
                    level.enabled = status != LevelStatus.Blocked;
                }
            }
        }

        public static void Save(this LevelsConfig levelsConfig)
        {
            foreach (var pack in levelsConfig.packs)
            {
                SetEnabled(
                    pack.id,
                    pack.enabled);
                foreach (var level in pack.levels)
                {
                    SetLevelStatus(
                        level.id,
                        level.complete ? LevelStatus.Complete
                        : level.enabled ? LevelStatus.Started
                        : LevelStatus.Blocked);
                }
            }
        }

        public static void ResetLevels(this LevelsConfig levelsConfig)
        {
            foreach (var pack in levelsConfig.packs)
            {
                pack.enabled = false;
                foreach (var level in pack.levels)
                {
                    level.complete = false;
                    level.enabled = false;
                }
            }

            Save(levelsConfig);

            PlayerPrefs2.SetBool(InitializedKey, false);
        }


        private static LevelStatus GetLevelStatus(string levelId)
        {
            string key = string.Format(LevelStatusKey_Format, levelId);
            Debug.Log($"Reading {key} = {PlayerPrefs.GetInt(key)}");
            return (LevelStatus)PlayerPrefs.GetInt(key);
        }

        private static void SetLevelStatus(string levelId, LevelStatus status)
        {
            string key = string.Format(LevelStatusKey_Format, levelId);
            Debug.Log($"{key} = {(int) status}");
            PlayerPrefs.SetInt(key, (int)status);
        }

        public static bool IsComplete(Level level) => GetLevelStatus(level.id) == LevelStatus.Complete;

        public static bool CanPlay(Level level) => (GetLevelStatus(level.id) & LevelStatus.CanPlay) != 0;

        public static bool IsComplete(string levelId) => GetLevelStatus(levelId) == LevelStatus.Complete;

        public static bool CanPlay(string levelId) => (GetLevelStatus(levelId) & LevelStatus.CanPlay) != 0;


        public static bool IsEnabled(LevelPack pack) => IsEnabled(pack.id);

        public static bool IsEnabled(string packId)
        {
            string key = string.Format(PackStatusKey_Format, packId);
            Debug.Log($"Reading {key} = {PlayerPrefs2.GetBool(key)}");
            return PlayerPrefs2.GetBool(key);
        }

        public static void SetEnabled(string packId, bool enabled)
        {
            string key = string.Format(PackStatusKey_Format, packId);
            PlayerPrefs2.SetBool(key, enabled);

            Debug.Log($"{key} = {enabled}");
        }


        private static string InitializedKey = "Init";
        private static string LevelStatusKey_Format = "LevelStatus_{0}";
        private static string PackStatusKey_Format = "PackStatus_{0}";


        [Serializable, Flags]
        private enum LevelStatus
        {
            Blocked = 0,
            Started = 1,
            Complete = 2,

            CanPlay = Started | Complete
        }
    }


    public static class PlayerPrefs2
    {
        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public static bool GetBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) != 0;
        }


        public static void SetJson<T>(string key, T value)
        {
            string json = JsonUtility.ToJson(value);
            PlayerPrefs.SetString(key, json);
        }

        public static T GetJson<T>(string key)
        {
            string json = PlayerPrefs.GetString(key, "{}");
            return JsonUtility.FromJson<T>(json);
        }
    }
}
