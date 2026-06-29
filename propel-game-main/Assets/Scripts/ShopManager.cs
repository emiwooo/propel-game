using UnityEngine;
using System.Collections.Generic;

[System.Serializable] 
public class ShopItem
{
    public string itemName;
    public List<int> prices;
    public string description;
    public int levelPurchased;
    public Sprite icon;
    
    public ShopItem(string name, List<int> priceList, string desc, int level, Sprite img)
    {
        itemName = name;
        prices = priceList;
        description = desc;
        levelPurchased = level;
        icon = img;
    }

}

public class ShopManager : MonoBehaviour
{
    [System.NonSerialized] 
    public Dictionary<string, ShopItem> shopDatabase = new Dictionary<string, ShopItem>();
    public PlayerCotroller player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        initialiseShop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initialiseShop()
    {
        shopDatabase.Clear();
        
        // Add items to the shop database
        shopDatabase.Add("Max Thrust", new ShopItem("Max Thrust", new List<int> { 10, 50, 100, 200, 500, 1000 }, "Increase maximum thrust.", 0, null));
        shopDatabase.Add("Small Candy", new ShopItem("Small Candy+", new List<int> { 20, 50, 100, 200, 500, 1000 }, "Increases amount of thrust regained from small candy.", 0, null));
        shopDatabase.Add("Large Candy", new ShopItem("Large Candy", new List<int> { 20, 50, 100, 200, 500, 1000 }, "Increases duration of boost from large candy.", 0, null));
        shopDatabase.Add("Max Ammo", new ShopItem("Max Ammo", new List<int> { 20, 50, 100, 500, 1000 }, "Increases amount of bullets in gun by 1.", 0, null));
        shopDatabase.Add("Pizzazz", new ShopItem("Pizzazz", new List<int> { 1 }, "Increases your pizzazz.", 0, null));

    }

    public void PurchaseItem(string itemName)
    {
        if (shopDatabase.ContainsKey(itemName))
        {
            ShopItem item = shopDatabase[itemName];
            int currentLevel = item.levelPurchased;
            if (currentLevel < item.prices.Count)
            {
                int price = item.prices[currentLevel];
                if (GameManager.Instance.money >= price)
                {
                    GameManager.Instance.money -= price;
                    item.levelPurchased++;
                    shopDatabase[itemName] = item; 
                    player.ApplyShopUpgrades(this);
                    PriceDisplay[] displays = FindObjectsOfType<PriceDisplay>();
                    foreach (var d in displays)
                    {
                        d.Refresh();
                    }
                    
                    Debug.Log($"Purchased {itemName} for {price}. New level: {item.levelPurchased}");
                }
                else
                {
                    Debug.Log($"Not enough money to purchase {itemName}");
                }
            }
        }
    }
}
