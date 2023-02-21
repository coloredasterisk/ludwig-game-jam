using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageModifier : MonoBehaviour
{
    public Image imageRenderer;
    //private List<Color> glitchColors = new List<Color>() { Color.black, Color.magenta };
    private List<Color> glitchColors = new List<Color>() { Color.red, Color.cyan, Color.black, Color.white, Color.magenta };
    public int GlitchAmount = 5;
    public float GlitchInterval = 0.05f;
    public float GlitchRadius = 1f;
    // Start is called before the first frame update
    void Awake()
    {
        imageRenderer = GetComponent<Image>();
    }

    public void GlitchDestroy()
    {
        StartCoroutine(GlitchAndDestroy());
    }

    private IEnumerator GlitchAndDestroy()
    {
        for (int i = 0; i < GlitchAmount; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-GlitchRadius, GlitchRadius), Random.Range(-GlitchRadius, GlitchRadius), 0);
            transform.localPosition += randomPos;
            imageRenderer.color = glitchColors[Random.Range(0, glitchColors.Count)];
            yield return new WaitForSeconds(GlitchInterval);
            transform.localPosition -= randomPos;
        }
        Destroy(gameObject);
    }
}
