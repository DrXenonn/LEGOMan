using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private static bool _isGamePaused = false;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject darkMask;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        darkMask.SetActive(false);
        Time.timeScale = 1.0f;
        _isGamePaused = false;
    }
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        darkMask.SetActive(true);
        Time.timeScale = 0;
        _isGamePaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
        Time.timeScale = 1.0f;
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}
