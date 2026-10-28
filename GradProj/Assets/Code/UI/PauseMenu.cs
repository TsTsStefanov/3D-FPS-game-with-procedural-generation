using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    public string mainMenu;
    private void Start()
    {
        Resume();
    }
    public void OnPause(InputAction.CallbackContext cntxt)
    {
        if (cntxt.started) 
        {
            if(isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            } 
        }
    }
    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
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
