using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carousel : MonoBehaviour
{
    public GameObject m_carouselElementPrefab;
    public List<GameObject> m_carouselElementChilds = new List<GameObject>();
    public RectTransform m_carouselTransform;
    public List<Sprite> m_sprites = new List<Sprite>();
    public List<string> m_names = new List<string>();

    public void AssignElements(List<InventoryItemData> itemsData)
    {
        Reset();
        foreach (var data in itemsData)
        {
            m_names.Add(data.itemName);
            m_sprites.Add(data.sprite);
        }
        Display();
    }

    public void Display()
    {
        if (gameObject.activeSelf)
        {
            Reset();
            return;
        }
        gameObject.SetActive(true);
        int size = 60 + 100 * m_sprites.Count;
        m_carouselTransform.sizeDelta = new Vector2(size, m_carouselTransform.sizeDelta.y);
        int i = 0;
        foreach (var sprite in m_sprites)
        {
            CarouselElement carouselElement = Instantiate(m_carouselElementPrefab, m_carouselTransform).GetComponent<CarouselElement>();
            carouselElement.m_image.sprite = sprite;
            carouselElement.m_text.text = m_names[i];
            i++;
        }
    }

    void Reset()
    {
        gameObject.SetActive(false);
        foreach (var element in m_carouselElementChilds)
        {
            Destroy(element.gameObject);
        }
        m_carouselElementChilds.Clear();
        m_sprites.Clear();
        m_names.Clear();
    }
}