using Assets.Scripts.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI
{
    class LevelListView : MonoBehaviour
    {
        public LevelsConfig config;
        public LevelLoader levelLoader;
        public Transform packsParent;
        public GameObject packPrefab;

        public void Start()
        {
            if (!config.Init())
            {
                Debug.Log("Loading");
                config.Load();
            }

            foreach (var pack in config.packs)
            {
                var packView = Instantiate(packPrefab, packsParent).GetComponent<PackView>();
                packView.ShowPack(pack);
                packView.levelClicked += OnLevelClicked;
            }
        }

        private void OnLevelClicked(Level level)
        {
            levelLoader.LoadLevel(level.sceneName);
        }
    }
}
