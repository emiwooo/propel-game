using UnityEngine;
using TMPro;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    public int Money
    {
        get { return GameManager.Instance.money; }
        set
        {
            GameManager.Instance.money = value;
            UpdateMoneyText();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateMoneyText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = "$" + GameManager.Instance.money.ToString();
    }
}
