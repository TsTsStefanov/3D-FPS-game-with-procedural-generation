using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OtherMenus : MonoBehaviour
{
    public string mainMenu;
    public GameObject winMenuUI;
    public GameObject loseMenuUI;
    public static bool winCondition = false;
    public static bool loseCondition = false;

    void Start()
    {
        winMenuUI.SetActive(false);
        loseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (winCondition)
        {
            winMenuUI.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
            PauseMenu.isPaused = true;
            Cursor.visible = true;
        }
        else if (loseCondition)
        {
            loseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
            PauseMenu.isPaused = true;
            Cursor.visible = true;
        }
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
