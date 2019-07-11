using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoonsUI : MonoBehaviour
{
    [SerializeField]
    protected GameObject m_boonUIPrefab;
    [SerializeField]
    protected List<GameObject> m_currentBoons;
    [SerializeField]
    protected List<Condition> m_currentConditions = new List<Condition>();

    public void UpdateUI(List<Condition> boons)
    {
        if (CompareLists(m_currentConditions, boons))
        {
            Debug.Log("Boons unchanged");
            UpdateStacks(boons);
            return;
        }
        Debug.Log("Boons are achanging");
        foreach (var boon in m_currentBoons)
        {
            Destroy(boon);
        }
        m_currentBoons.Clear();
        m_currentConditions.Clear();
        foreach (var boon in boons)
        {
            var instance = Instantiate(m_boonUIPrefab, transform);
            m_currentBoons.Add(instance);
            instance.GetComponent<Image>().sprite = boon.GetData().sprite;
            m_currentConditions.Add(boon);
            instance.GetComponentInChildren<TextMeshProUGUI>().text = boon.GetIntensity().ToString();
        }
    }

    public void UpdateStacks(List<Condition> boons)
    {
        for (int i = 0; i<boons.Count; i++)
        {
            m_currentBoons[i].GetComponentInChildren<TextMeshProUGUI>().text = boons[i].GetIntensity().ToString();
        }
    }

    public bool CompareLists<T>(List<T> a, List<T> b)
    {
        if (a.Count != b.Count)
            return false;
        for (int i = 0; i < a.Count; i++)
        {
            if (!a[i].Equals(b[i]))
            {
                return false;
            }
        }
        return true;
    }
}
