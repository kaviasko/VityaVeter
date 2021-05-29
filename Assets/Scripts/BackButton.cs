using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour
{
    public void Back()
    {
        LevelLoader.Main.LoadLevel(0);
    }
}
