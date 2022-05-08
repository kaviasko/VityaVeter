using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    class PackView : MonoBehaviour
    {
        public event Action<Level> levelClicked;

        public Transform levelListParent;
        public GameObject levelButtonPrefab;
        public GameObject lockedPanel;
        public TMP_Text coinsRequired;
        public TMP_Text starsRequired;

        public void ShowPack(LevelPack pack)
        {
            for (int i = 0; i < pack.levels.Count; i++)
            {
                Level level = pack.levels[i];

                var button = Instantiate(levelButtonPrefab, levelListParent).GetComponent<LevelButton>();
                button.ShowLevel(i, level);
                button.clicked += OnLevelClicked;
            }

            lockedPanel.SetActive(!pack.enabled);
            if (!pack.enabled)
            {
                coinsRequired.text = pack.coinsToUnlock.ToString();
                starsRequired.text = pack.starsToUnlock.ToString();
            }
        }

        private void OnLevelClicked(Level level)
        {
            levelClicked?.Invoke(level);
        }
    }
}
