using UnityEngine;

public class ClickSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    
    public void OnLegoClick(Component sender, object data)
    {
        audioSource.Play();
    }
}
