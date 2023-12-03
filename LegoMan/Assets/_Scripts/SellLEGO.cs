using TMPro;
using UnityEngine;

public class SellLEGO : MonoBehaviour
{
    [SerializeField] private Transform legoShop;
    [SerializeField] private Transform legoSell;
    [SerializeField] private TextMeshProUGUI sellUI;
    [SerializeField] private GameEvent trade;
    [SerializeField] private AudioSource audioSource;
    private bool _isShopActivated;

    private void Update()
    {
        if (GameManager.Instance.isHolding)
        {
            ActivateShop();
        }
        else
        {
            DeActivateShop();
        }
    }

    private void ActivateShop()
    {
        if (_isShopActivated) return;
        legoSell.gameObject.SetActive(true);
        legoShop.position -= new Vector3(0, 10, 0);
        _isShopActivated = true;
    }

    private void DeActivateShop()
    {
        if (!_isShopActivated) return;
        legoShop.position += new Vector3(0, 10, 0);
        legoSell.gameObject.SetActive(false);
        _isShopActivated = false;
    }

    public void SellLego(Component sender, object price)
    {
        trade.Raise((int)price);
        audioSource.Play();
        Destroy(sender.gameObject);
    }

    public void OnLegoMouseDown(Component sender, object data)
    {
        var price = (int)data;
        sellUI.text = $"Sell for {price}g";
    }
}
