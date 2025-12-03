using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public Image heartImagePrefab;
    public Sprite HeartSprite;

    public List<Image> heartImages = new List<Image>();

    public void SetHeartUI(int maxHearts)
    {
        foreach (Image image in heartImages)
        {
            Destroy(image.gameObject);
        }

        heartImages.Clear();

        for (int i = 0; i < maxHearts; i++)
        {
            Image newHeart = Instantiate(heartImagePrefab, transform);
            newHeart.sprite = HeartSprite;
            newHeart.color = Color.white;
            heartImages.Add(newHeart);
        }
    }

    public void UpdateHeartUI(int currentHeart)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < currentHeart)
            {
                heartImages[i].color = Color.white;
            }
            else
            {
                heartImages[i].color = Color.black;
            }
        }
    }
}
