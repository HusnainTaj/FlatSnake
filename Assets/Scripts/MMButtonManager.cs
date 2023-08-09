using UnityEngine;
using UnityEngine.UI;

public class MMButtonManager : MonoBehaviour
{
    public SceneController sceneController;
	public InputField nameText;
	public GameObject namePanel;

	public static bool nameSet = false;


    private void Start()
    {
        PlayerPrefs.SetInt(PlayerPrefsVariables.Difficulty, PlayerPrefs.GetInt(PlayerPrefsVariables.Difficulty, Difficulty.medium));
        SetHighscoreVariable();
		
		nameText.text = PlayerPrefs.GetString (PlayerPrefsVariables.Username, "");

		if(nameSet)
			namePanel.SetActive (false);
    }

    private void SetHighscoreVariable()
    {
        switch (PlayerPrefs.GetInt(PlayerPrefsVariables.Difficulty, Difficulty.medium))
        {
            case Difficulty.easy:
                PlayerPrefsVariables.HighScore = PlayerPrefsVariables.EasyHighscore;
                break;
            case Difficulty.medium:
                PlayerPrefsVariables.HighScore = PlayerPrefsVariables.MediumHighscore;
                break;
            case Difficulty.hard:
                PlayerPrefsVariables.HighScore = PlayerPrefsVariables.HardHighscore;
                break;
        }
    }

    public void PlayBtn()
    {
        sceneController.loadScene("Game");
    }

    public void SettingsBtn()
    {
        sceneController.loadScene("Settings");
    }

	public void NameBtn()
	{
		if (nameText.text.Length > 0) 
		{
			Debug.Log ("L : " + nameText.text);
			PlayerPrefs.SetString (PlayerPrefsVariables.Username, nameText.text);
			namePanel.SetActive (false);
			nameSet = true;
		}
	}

    //public void LeaderBoardBtn()
    //{
    //    sceneController.loadScene("LeaderBoard");
    //}
}
