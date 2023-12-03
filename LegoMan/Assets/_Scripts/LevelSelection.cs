using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private Button[] levels;
    [SerializeField] private GameObject levelButtons;
    
    private void Awake() {

        ButtonToArray();

        var unlockLevel =  PlayerPrefs.GetInt("UnlockLevel",1);
        for (var i = 0; i < levels.Length; i++)
        {
            levels[i].interactable = false;
        }
        for (var i = 0; i < unlockLevel; i++)
        {
            levels[i].interactable = true;
        }
    }
    
    public void OpenLevel(int indexLevel){
        const string levelName = "Level";
        SceneManager.LoadSceneAsync(levelName + indexLevel);
    }
    
    private void ButtonToArray(){
        var childCount = levelButtons.transform.childCount;
        levels = new Button[childCount];
        for (var i = 0; i < childCount; i++)
        {
            levels[i] = levelButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
        }
    }
}
