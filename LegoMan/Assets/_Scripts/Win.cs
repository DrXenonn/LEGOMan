using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
                winPanel!.SetActive(true);
                Time.timeScale = 0;
                Destroy(gameObject);
            }
            else
            {
                UnlockNewLevel();
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
                Destroy(gameObject);
            }
        }
    }

    private void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 2 < PlayerPrefs.GetInt("ReachedIndex")) return;
        PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetInt("UnlockLevel", PlayerPrefs.GetInt("UnlockLevel", 1) + 1);
        PlayerPrefs.Save();
    }
}