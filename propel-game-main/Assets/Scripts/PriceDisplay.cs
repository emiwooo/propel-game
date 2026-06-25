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
        Debug.Log(priceText);
        UpdatePriceText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePriceText()
    {
        Debug.Log("Looking for: " + targetItemName);
        Debug.Log("Contains: " + shopManager.shopDatabase.ContainsKey(targetItemName));
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
