using TMPro;
using UnityEngine;

public class Balance : MonoBehaviour
{
    [SerializeField] private int balance;
    [SerializeField] private TextMeshProUGUI balanceUI;
    [SerializeField] private GameEvent updateDisabledShop;
    [SerializeField] private Animator animator;

    private void Start()
    {
        UpdateUI();
    }

    public void Trade(Component sender, object data)
    {
        var amount = (int)data;
        if (balance + amount >= 0)
        {
            balance += amount;
        }
        else
        {
            balance = 0;
        }

        UpdateUI();
        PlayTradeAnimation(amount);
    }

    public int GetBalance()
    {
        return balance;
    }

    private void UpdateUI()
    {
        balanceUI.text = balance.ToString();
        updateDisabledShop.Raise();
    }

    private void PlayTradeAnimation(int value = 0)
    {
        animator.Play(value < 0 ? "BuyUI" : "SellUI");
    }
}