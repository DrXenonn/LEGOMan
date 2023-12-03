using System;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance { get; private set; }
    [SerializeField] private Color[] colors;
    private int _index = 0;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Color GetColor()
    {
        if (_index >= colors.Length)
        {
            _index = 0;
        }
        
        var selectedColor = colors[_index];
        _index++;

        return selectedColor;
    }
}