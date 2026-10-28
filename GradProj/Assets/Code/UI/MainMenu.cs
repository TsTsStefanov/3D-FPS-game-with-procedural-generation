using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameScene;
    public void Play()
    {
        OtherMenus.winCondition = false;
        OtherMenus.loseCondition = false;
        SceneManager.LoadScene(gameScene);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
