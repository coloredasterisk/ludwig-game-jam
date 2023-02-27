
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static ShopAttachment;

public class ShopGroup : MonoBehaviour
{
    public List<ShopItem> shopItems;
    [System.Serializable]
    public struct ShopItem
    {
        public GameObject dropPrefab;
        public int count;
        public int minCost;
        public int maxCost;
        public string description;
        public float descriptionWidth;
    }

    public List<ShopAttachment> shops = new List<ShopAttachment>();
    // Start is called before the first frame update
    void Start()
    {
        List<ShopItem> randomItems = new List<ShopItem>();
        for(int i = 0; i < shops.Count; i++)
        {
            ShopItem shopItem;
            do
            {
                int index = Random.Range(0, shopItems.Count);
                shopItem = shopItems[index];
            } while (randomItems.Contains(shopItem));
            
            randomItems.Add(shopItem);
            
        }
        for (int i = 0; i < shops.Count; i++)
        {
            ShopAttachment shopAttachment = shops[i];
            ShopItem shopItem = randomItems[i];
            shopAttachment.Initialize(shopItem);
            

        }
        
    }
}
