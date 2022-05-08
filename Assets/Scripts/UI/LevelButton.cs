using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    class LevelButton : MonoBehaviour
    {
        public event Action<Level> clicked;

        public TMP_Text text;
        public Button button;

        [NonSerialized]
        public Level level;

        public void OnEnable()
        {
            button.onClick.AddListener(OnClick);
        }

        public void OnDisable()
        {
            button.onClick.RemoveListener(OnClick);
        }

        public void ShowLevel(int levelIndex, Level newLevel)
        {
            level = newLevel;
            text.text = (levelIndex + 1).ToString();
            button.interactable = newLevel.enabled;
        }

        public void OnClick()
        {
            clicked?.Invoke(level);
        }
    }
}
