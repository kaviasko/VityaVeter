using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialImage : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKey)
        {
            gameObject.SetActive(false);
        }
    }

    public bool getActive()
    {
        return gameObject.activeSelf;
    }
}
