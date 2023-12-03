using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class LegoShop : MonoBehaviour
{
    [SerializeField] private GameObject lego;
    [SerializeField] private int price;
    [SerializeField] private Transform parent;
    [SerializeField] private GameEvent trade;
    [SerializeField] private Balance balance;
    [SerializeField] private GameObject disabledEffect;
    [SerializeField] private AudioSource notEnoughCoinsSound;
    [SerializeField] private Outline outline;

    private GameObject _instantiatedLego;
    private Camera _mainCam;
    private GameObject _gridManager;
    private CapsuleCollider2D _playersCollider;
    private LegoDragAndDrop _instantiatedDragAndDrop;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        _mainCam = Camera.main;
        _gridManager = GameObject.FindWithTag("GridManager").gameObject;
        _playersCollider = GameObject.FindWithTag("Player").GetComponent<CapsuleCollider2D>();
        UpdateDisabledEffect(null, null);
    }

    private void OnMouseDown()
    {
        if (!(balance.GetBalance() >= price))
        {
            NotEnoughCoins();
        }
        else
        {
            BuyLego();
        }
    }

    private void OnMouseOver()
    {
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        outline.enabled = false;
    }

    private void NotEnoughCoins()
    {
        notEnoughCoinsSound.Play();
    }

    private void OnMouseUp()
    {
        if (_instantiatedLego == null) return;
        _instantiatedDragAndDrop.OnMouseUp();
        _instantiatedLego = null;
    }

    private void OnMouseDrag()
    {
        if (_instantiatedLego == null) return;
        _instantiatedDragAndDrop.OnMouseDrag();
    }

    private void BuyLego()
    {
        _instantiatedLego = Instantiate(lego, parent);
        _instantiatedDragAndDrop = _instantiatedLego.GetComponent<LegoDragAndDrop>();
        var mousePosition = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        _instantiatedLego.transform.position = mousePosition;
        _instantiatedDragAndDrop.Init(_gridManager, _mainCam, _playersCollider);
        trade.Raise(-price);
    }

    public void UpdateDisabledEffect(Component sender, object data)
    {
        disabledEffect.SetActive(balance.GetBalance() < price);
    }
}