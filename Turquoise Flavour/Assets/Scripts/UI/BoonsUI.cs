using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoonsUI : MonoBehaviour
{
    [SerializeField]
    protected GameObject m_boonUIPrefab;
    [SerializeField]
    protected List<GameObject> m_currentBoons;

    public void UpdateUI(List<Condition> boons)
    {
        foreach (var boon in m_currentBoons)
        {
            Destroy(boon);
        }
        m_currentBoons.Clear();
        foreach (var boon in boons)
        {
            var instance = Instantiate(m_boonUIPrefab, transform);
            m_currentBoons.Add(instance);
            instance.GetComponent<Image>().sprite = boon.GetSprite();
        }
    }
}
