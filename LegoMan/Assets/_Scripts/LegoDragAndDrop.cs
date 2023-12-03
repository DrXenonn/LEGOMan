using System;
using UnityEngine;

public class LegoDragAndDrop : MonoBehaviour
{
    #region variables
    private enum LegoType
    {
        Odd,
        Even,
        Four
    }
    
    [SerializeField] private LegoType legoType;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private GameEvent sell;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameEvent onLegoMouseDown;
    
    private Camera _camera;
    private Vector3 _mouseOffset;
    private GameObject _grids;
    private GridManager _gridManager;
    private Vector3 _initialPosition;
    private CapsuleCollider2D _playerCapsuleCollider;

    public int price;
    
    private bool _isDragging;
    #endregion

    public void Init(GameObject gridManager, Camera mainCam, CapsuleCollider2D playersCollider)
    {
        _grids = gridManager;
        _camera = mainCam;
        _playerCapsuleCollider = playersCollider;
        _gridManager = _grids.GetComponent<GridManager>();

        OnMouseUp();
        OnMouseDown();
    }

    private void OnMouseDown()
    {
        GameManager.Instance.isHolding = true;
        _isDragging = true;
        _grids.SetActive(true);
        _gridManager.SelectLegoPiece(gameObject, legoType.ToString(), _initialPosition);
        
        IgnoreCollisionWithPlayer(true);

        var position = transform.position;
        _initialPosition = position;
        _mouseOffset = position - GetMousePos();
        onLegoMouseDown.Raise(price);
    }

    private void Update()
    {
        if (!_isDragging) return;
        transform.position = GetMousePos() + _mouseOffset;
    }

    public void OnMouseDrag()
    {
        foreach (var otherCollider in GetColliders())
        {
            if (IsPlaceable(otherCollider))
            {
                SetAlpha(0.33f);
            }
            else
            {
                SetAlpha(1);
            }
        }
    }

    public void OnMouseUp()
    {
        GameManager.Instance.isHolding = false;
        _isDragging = false;
        IgnoreCollisionWithPlayer(false);
        Invoke(nameof(SetAlphaNoParam), 0.025f);

        _grids.SetActive(false);

        foreach (var otherCollider in GetColliders())
        {
            if (otherCollider.CompareTag("Sell"))
            {
                sell.Raise(gameObject.transform, price);
            }

            if (IsPlaceable(otherCollider))
            {
                transform.position = _initialPosition;
                break;
            }

            _gridManager.SnapToGrid();
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.isHolding = false;
            _grids.SetActive(false);
            _isDragging = false;
            sell.Raise(gameObject.transform, price);
        }
    }

    private void IgnoreCollisionWithPlayer(bool shouldIgnore)
    {
        Physics2D.IgnoreCollision(boxCollider2D, _playerCapsuleCollider, shouldIgnore);
    }

    private Collider2D[] GetColliders()
    {
        var bounds1 = boxCollider2D.bounds;
        var bounds = bounds1;
        var newCenter = new Vector3(bounds.center.x, bounds.center.y - 0.015f, 0);
        var size =
            bounds1.size.x > 3.5f
                ? new Vector3(bounds.size.x * 0.575f, bounds.size.y * 0.475f, 0)
                : new Vector3(bounds.size.x * 0.75f, bounds.size.y * 0.475f, 0);
        
        // ReSharper disable once Unity.PreferNonAllocApi
        return Physics2D.OverlapBoxAll(newCenter, size, 0f);
    }
    
    private bool IsPlaceable(Collider2D otherCollider)
    {
        return otherCollider.CompareTag("Ground") || otherCollider.CompareTag("Player") ||
               (otherCollider.CompareTag("Brick") && otherCollider.gameObject != gameObject);
    }

    private void SetAlphaNoParam()
    {
        SetAlpha();
    }

    private void SetAlpha(float value = 1)
    {
        var color = spriteRenderer.color;
        color.a = value;
        spriteRenderer.color = color;
    }

    private Vector3 GetMousePos()
    {
        var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}