using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public SceneController sceneController;
    public GameManager GM;
    public GameObject PausePanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            PauseBtn();
        }
    }

    public void PauseBtn()
    {
        GM.stopSnake();
        PausePanel.SetActive(true);
    }

    public void ResumeBtn()
    {
        PausePanel.SetActive(false);
        GM.startSnake();
    }

    public void MenuBtn()
    {
        sceneController.loadScene("MainMenu");
    }

    public void RestartBtn()
    {
        sceneController.loadScene("Game");
    }
}
