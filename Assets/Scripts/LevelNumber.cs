using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelNumber : MonoBehaviour
{
    public Text text;

    void Start()
    {
        text.text += (SceneManager.GetActiveScene().buildIndex - 1).ToString();
    }

}
