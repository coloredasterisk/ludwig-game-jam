using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAttachment : MonoBehaviour
{
    public int itemCost;
    public DropItems dropItems;
    // Start is called before the first frame update
    void Start()
    {
        dropItems = GetComponent<DropItems>();
    }

    public void SellItem()
    {
        if (DataManager.currency >= itemCost)
        {
            DataManager.currency -= itemCost;
            CanvasReference.Instance.currencyText.text = "" + DataManager.currency;
            if (dropItems != null)
            {
                dropItems.SpawnDrops();
            }
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(NotEnough());
        }
    }

    public IEnumerator NotEnough()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
