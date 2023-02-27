using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ShopGroup;

public class ShopAttachment : MonoBehaviour
{
    public int currentCost;
    public DropItems dropItems;
    public TextMeshProUGUI textDisplay;
    public GameObject descriptionDisplay;
    public TextMeshProUGUI descriptionText;
    public RectTransform background;
    
    public AudioSource audioSource;
    public List<AudioClip> soundEffect;

    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DataManager.AddSoundEffect(audioSource);
    }

    public void Initialize(ShopItem shopItem)
    {
        currentCost = Random.Range(shopItem.minCost, shopItem.maxCost);
        textDisplay.text = "" + currentCost;
        background.sizeDelta = new Vector2(shopItem.descriptionWidth, 1);
        descriptionText.text = shopItem.description;

        GetComponent<SpriteRenderer>().sprite = shopItem.dropPrefab.GetComponent<SpriteRenderer>().sprite;

        dropItems = GetComponent<DropItems>();
        DropItems.Items item = new DropItems.Items();
        item.prefab = shopItem.dropPrefab;
        item.count = shopItem.count;
        dropItems.ItemDrops.Add(item);
    }

    public void SellItem()
    {
        if (DataManager.currency >= currentCost)
        {
            DataManager.currency -= currentCost;
            CanvasReference.Instance.currencyText.text = "" + DataManager.currency;
            if (dropItems != null)
            {
                //Debug.Log("test");
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
        audioSource.PlayOneShot(soundEffect[0]);
        descriptionDisplay.SetActive(false);
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            descriptionDisplay.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            descriptionDisplay.SetActive(false);
        }
    }
}
