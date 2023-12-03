using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private int _currentIndex;
    [SerializeField] private List<GameObject> tutorials;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject button;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("FirstTime",1) == 1)
        {
            Invoke(nameof(ShowTutorial),1);
        }
    }

    public void OnNext()
    {
        if (_currentIndex+1 == tutorials.Count - 1)
        {
            buttonText.text = "Cool!";
        }

        if (_currentIndex == tutorials.Count - 1)
        {
            HideTutorial();
        }
        
        tutorials[_currentIndex].SetActive(false);
        tutorials[_currentIndex + 1].SetActive(true);
        _currentIndex++;
    }
    
    private void ShowTutorial()
    {
        tutorials[0].gameObject.SetActive(true);
        background.SetActive(true);
        button.SetActive(true);
        Time.timeScale = 0;
    }

    private void HideTutorial()
    {
        Time.timeScale = 1;
        tutorials[0].transform.parent.gameObject.SetActive(false);
        PlayerPrefs.SetInt("FirstTime", 0);
        PlayerPrefs.Save();
    }
}