using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsPanelUI : MonoBehaviour
{
    [SerializeField]
    protected GameObject m_statPanelPrefab;
    protected List<StatisticPanel> m_child = new List<StatisticPanel>();

    public void DisplayStats()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            foreach (var child in m_child)
            {
                Destroy(child.gameObject);
            }
            m_child.Clear();
        }
        else
        {
            gameObject.SetActive(true);
            foreach (var tuple in StatisticsManager.GetInstance().GetStatistics())
            {
                StatisticPanel statPanel = Instantiate(m_statPanelPrefab, transform).GetComponent<StatisticPanel>();
                statPanel.m_statName.text = tuple.stat.ToString();
                statPanel.m_statAmount.text = tuple.value.ToString();
                m_child.Add(statPanel);
            }
        }
    }
}
