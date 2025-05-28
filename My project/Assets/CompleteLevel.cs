using UnityEngine;

public class CompleteLevel : MonoBehaviour
{
    public string menuSceneName = "MainMenu";

    public string nextLevel = "Level4";
    public int levelToUnlock = 4;

    public SceneFader sceneFader;

    public void Continue()
    {
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
        sceneFader.FadeTo(nextLevel);
    }

    public void Menu()
    {
        sceneFader.FadeTo(menuSceneName);
    }
}
