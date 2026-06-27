using UnityEngine;
using TMPro;

public class PriceDisplay : MonoBehaviour
{
    public ShopManager shopManager;
    public string targetItemName;
    [SerializeField] private TextMeshProUGUI priceText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdatePriceText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh()
    {
        UpdatePriceText();
    }

    public void UpdatePriceText()
    {
        if (shopManager.shopDatabase.ContainsKey(targetItemName))
        {
            ShopItem item = shopManager.shopDatabase[targetItemName];
            int currentLevel = item.levelPurchased;

            if (currentLevel < item.prices.Count)
            {
                priceText.text = "$" + item.prices[currentLevel].ToString();
            }
            else
            {
                priceText.text = "MAXED";
            }
        }
    }

    public void OnButtonClick()
    {
        shopManager.PurchaseItem(targetItemName);
        UpdatePriceText(); 
    }
}
