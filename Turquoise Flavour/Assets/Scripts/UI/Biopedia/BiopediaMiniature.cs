using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BiopediaMiniature : MonoBehaviour
{
    public TextMeshProUGUI m_name;
    public Image m_image;
    protected CreatureData m_creatureData;
    
    public void AssignData(CreatureData data)
    {
        m_creatureData = data;
        m_name.text = data.creatureName;
        m_image.sprite = data.sprite;
    }

    public void OnClick()
    {
        GetComponentInParent<Biopedia>().OnChildClicked(m_creatureData);
    }
}
