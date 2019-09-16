using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanel : MonoBehaviour
{
    [SerializeField]
    protected GameObject m_rewardPanelPrefab;
    [SerializeField]
    protected List<GameObject> m_childRef;
    [SerializeField]
    protected Transform m_panelTransform;
    public static RewardPanel m_instance;

    public static RewardPanel GetInstance()
    {
        return m_instance;
    }

    private void Awake()
    {
        m_instance = this;
    }

    public void UnlockReward(List<string> names, List<Sprite> sprites)
    {
        gameObject.SetActive(true);
        if (names.Count == sprites.Count)
        {
            for (int i = 0; i < names.Count; i++)
            {
                GameObject instance = Instantiate(m_rewardPanelPrefab, m_panelTransform);

                m_childRef.Add(instance);
                instance.GetComponentInChildren<Image>().sprite = sprites[i];
                instance.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = names[i];
            }
        }
    }

    public void OnClickAcceptReward()
    {
        foreach (var obj in m_childRef)
        {
            Destroy(obj);
        }
        gameObject.SetActive(false);
    }
}
