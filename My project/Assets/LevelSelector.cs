using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons;

    void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if( i+1 > levelReached)
                levelButtons[i].interactable = false;
        }
    }
    public void LOadingLevels(string nameOftheLevel)
    {
        SceneManager.LoadScene(nameOftheLevel);
    }
}