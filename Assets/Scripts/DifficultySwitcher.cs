using UnityEngine;
using UnityEngine.UI;

public class DifficultySwitcher : MonoBehaviour
{
    public Text difficultyText;
    public Text SSText;
    public SceneController sceneController;

    private AudioSource themeAS;
    void Awake()
    {
        themeAS = FindObjectOfType<API>().GetComponent<AudioSource>();
        showDifficulty();
        showSoundState();
    }

    void showDifficulty()
    {
        switch (PlayerPrefs.GetInt(PlayerPrefsVariables.Difficulty, Difficulty.medium))
        {
            case Difficulty.easy:
                difficultyText.text = "Easy";
                break;
            case Difficulty.medium:
                difficultyText.text = "Medium";
                // PlayerPrefs.SetInt(PlayerPrefsVariables.Difficulty, Difficulty.medium);
                break;
            case Difficulty.hard:
                difficultyText.text = "Hard";
                break;
        }
    }

    public void PDifficultyBtn()
    {
        PlayerPrefs.SetInt(PlayerPrefsVariables.Difficulty, PlayerPrefs.GetInt(PlayerPrefsVariables.Difficulty, Difficulty.medium) - 1);

        if (PlayerPrefs.GetInt(PlayerPrefsVariables.Difficulty, Difficulty.medium) < Difficulty.easy)
            PlayerPrefs.SetInt(PlayerPrefsVariables.Difficulty, Difficulty.hard);

        showDifficulty();
    }

    public void NDifficultyBtn()
    {
        PlayerPrefs.SetInt(PlayerPrefsVariables.Difficulty, PlayerPrefs.GetInt(PlayerPrefsVariables.Difficulty, Difficulty.medium) + 1);

        if (PlayerPrefs.GetInt(PlayerPrefsVariables.Difficulty, Difficulty.medium) > Difficulty.hard)
            PlayerPrefs.SetInt(PlayerPrefsVariables.Difficulty, Difficulty.easy);

        showDifficulty();
    }

    public void showSoundState()
    {
        switch (PlayerPrefs.GetInt("Sound", 1))
        {
            case 0:
                themeAS.mute = true;
                SSText.text = "off";
                // PlayerPrefs.SetInt("Sound", 0);
                break;
            case 1:
                themeAS.mute = false;
                SSText.text = "On";
                // PlayerPrefs.SetInt("Sound", 1);
                break;
        }
    }

    public void PSSBtn()
    {
        PlayerPrefs.SetInt("Sound", PlayerPrefs.GetInt("Sound", 1) - 1);

        if (PlayerPrefs.GetInt("Sound", 1) < 0)
            PlayerPrefs.SetInt("Sound", 1);

        showSoundState();
    }

    public void NSSBtn()
    {
        PlayerPrefs.SetInt("Sound", PlayerPrefs.GetInt("Sound", 1) + 1);

        if (PlayerPrefs.GetInt("Sound", 1) > 1)
            PlayerPrefs.SetInt("Sound", 0);

        showSoundState();
    }

    public void MenuBtn()
    {
        sceneController.loadScene("MainMenu");
    }

    public void ResetBtn()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
