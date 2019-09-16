using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;
using UnityEngine.UI;

public class ExploratorPanel : UIIconParent
{
    [SerializeField]
    protected GameObject m_exploratorIconPrefab;
    [SerializeField]
    protected List<EExplorator> m_unlockedExplorators = new List<EExplorator>();
    [SerializeField]
    protected EExplorator m_currentExplorator;
    float m_xSize;

    void DisplayPanel(List<EExplorator> unlockedExplorators, EExplorator currentExplorator)
    {
        if (gameObject.activeSelf)
        {
            Reset();
            return;
        }
        Reset();
        gameObject.SetActive(true);
        m_currentExplorator = currentExplorator;
        m_unlockedExplorators = unlockedExplorators;
        m_xSize = unlockedExplorators.Count * 60;
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(m_xSize, rect.sizeDelta.y);
        int i = 0;
        foreach (var explorator in m_unlockedExplorators)
        {
            var instance = Instantiate(m_exploratorIconPrefab, transform);
            instance.GetComponent<UIIcon>().index = i;
            instance.GetComponent<Image>().sprite = ExploratorManager.GetInstance().GetExploratorDataFromExploName(explorator).sprite;
            i++;
        }
    }

    void Reset()
    {
        foreach (var go in GetComponentsInChildren<UIIcon>())
        {
            Destroy(go.gameObject);
        }
        m_unlockedExplorators = null;
        m_currentExplorator = EExplorator.Count;
        gameObject.SetActive(false);
    }

    public void DisplayPanel()
    {
        DisplayPanel(ExploratorManager.GetInstance().m_unlockedExplorators, Player.GetPlayerInstance().GetCurrentExploratorEnum());
    }

    public override void OnChildIconCLicked(int childIndex)
    {
        EExplorator chosenExplorator = m_unlockedExplorators[childIndex];
        Player.GetPlayerInstance().ChangeExplorator(chosenExplorator);
        Reset();
    }
}
