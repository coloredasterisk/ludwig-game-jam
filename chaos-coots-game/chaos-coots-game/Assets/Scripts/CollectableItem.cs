using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectType
{
    Fish,
    Life,
};

public class CollectableItem : MonoBehaviour
{
    public int value;
    public CollectType type;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect(collision);
        }
    }

    public void Collect(Collider2D collision)
    {
        if(type == CollectType.Fish)
        {
            DataManager.currency += value;
            CanvasReference.Instance.currencyText.text = "" + DataManager.currency;
            
        }
        else if(type == CollectType.Life)
        {
            collision.GetComponent<PlayerController>().health += value;
            CanvasReference.Instance.CreateLife();
        }
        Destroy(gameObject);

    }

}
