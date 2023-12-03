using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private int numRows = 5;
    [SerializeField] private int numColumns = 5;
    [SerializeField] private float cellSize = 1.0f;

    private GameObject _selectedLegoPiece;
    private string _selectedLegoType;
    private readonly List<GameObject> _grids = new List<GameObject>();
    private Vector3 _initialPos;

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        for (var row = 0; row < numRows; row++)
        {
            for (var col = 0; col < numColumns; col++)
            {
                var xPos = col * cellSize;
                var yPos = row * cellSize;

                var position = new Vector3(xPos, yPos, 0.0f);
                var grid = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                _grids.Add(grid);
            }
        }
        gameObject.SetActive(false);
    }
    
    public void SelectLegoPiece(GameObject legoPiece, string legoType, Vector3 initialPos)
    {
        _selectedLegoPiece = legoPiece;
        _selectedLegoType = legoType;
        _initialPos = initialPos;
    }
    
    public void SnapToGrid()
    {
        if (_selectedLegoPiece == null) return;
        switch (_selectedLegoType)
        {
            case "Even":
            {
                var boxCollider = _selectedLegoPiece.GetComponent<BoxCollider2D>();
                var collidingGrids = new List<Transform>();

                if (boxCollider != null)
                {
                    foreach (var grid in _grids)
                    {
                        if (boxCollider.OverlapPoint(grid.transform.position))
                        {
                            collidingGrids.Add(grid.transform);
                        }
                    }

                    if (collidingGrids.Count < 2)
                    {
                        _selectedLegoPiece.transform.position = _initialPos;
                    }
                    else
                    {
                        var newPosition = Vector3.Lerp(collidingGrids[0]!.transform.position, collidingGrids[1]!.transform.position, 0.5f);
                        _selectedLegoPiece.transform.position = newPosition!;
                    }
                }

                break;
            }
            case "Odd":
            {
                var position = _selectedLegoPiece.transform.position;
                var cellX = Mathf.RoundToInt(position.x / cellSize);
                var cellY = Mathf.RoundToInt(position.y / cellSize);
                var snapPosition = new Vector3(cellX * cellSize, cellY * cellSize, 0.0f);
                position = snapPosition;

                _selectedLegoPiece.transform.position = position;
                break;
            }
            case "Four":
            {
                var boxCollider = _selectedLegoPiece.GetComponent<BoxCollider2D>();
                var collidingGrids = new List<Transform>();

                if (boxCollider != null)
                {
                    foreach (var grid in _grids)
                    {
                        if (boxCollider.OverlapPoint(grid.transform.position))
                        {
                            collidingGrids.Add(grid.transform);
                        }
                    }

                    if (collidingGrids.Count < 4)
                    {
                        _selectedLegoPiece.transform.position = _initialPos;
                    }
                    else
                    {
                        var newPositionX = Vector3.Lerp(collidingGrids[0]!.transform.position, collidingGrids[1]!.transform.position, 0.5f);
                        var newPositionY = Vector3.Lerp(collidingGrids[2]!.transform.position, collidingGrids[3]!.transform.position, 0.5f);
                        var newPosition = Vector3.Lerp(newPositionX!, newPositionY!, 0.5f);
                        _selectedLegoPiece.transform.position = newPosition!;
                    }
                }

                break;
            }
        }
    }
}