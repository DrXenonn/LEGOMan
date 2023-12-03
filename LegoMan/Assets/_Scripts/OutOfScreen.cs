using UnityEngine;
using UnityEngine.SceneManagement;

public class OutOfScreen : MonoBehaviour
{
    [SerializeField] private GameEvent sellLego;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Brick"))
        {
            sellLego.Raise(other.transform, other.GetComponent<LegoDragAndDrop>().price);
        }
        
        if (other.CompareTag("Player"))
        {
            ReloadCurrentScene();
        }
    }
    
    private void ReloadCurrentScene()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        
        SceneManager.LoadScene(currentSceneName);
    }
}
