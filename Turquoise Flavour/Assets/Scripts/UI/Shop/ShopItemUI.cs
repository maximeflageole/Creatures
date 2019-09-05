using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI m_priceTextMesh;
    [SerializeField]
    protected int m_price;

    public void Start()
    {
        m_priceTextMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void AssignPrice(int price)
    {
        m_price = price;
        m_priceTextMesh.text = m_price.ToString();
    }
}
