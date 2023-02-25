using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DropItems : MonoBehaviour
{
    public List<Items> ItemDrops;

    [System.Serializable] public struct Items
    {
        public int count;
        public GameObject prefab;
    }

    public void SpawnDrops()
    {
        foreach(Items item in ItemDrops)
        {
            for(int i = 0; i < item.count; i++)
            {
                
                
                GameObject drop = Instantiate(item.prefab, transform.position, Quaternion.identity);
                if (drop.GetComponent<Rigidbody2D>() != null)
                {
                    Vector3 movement = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * Random.Range(25,50);
                    drop.GetComponent<Rigidbody2D>().AddForce(movement, ForceMode2D.Impulse);
                }
                
            }
            
        }
    }
}
