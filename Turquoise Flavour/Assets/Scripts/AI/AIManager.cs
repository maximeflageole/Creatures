using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

//Singleton
public class AIManager : MonoBehaviour
{
    public Dictionary<EAIActions, Sprite> m_actionSpriteDictionnary = new Dictionary<EAIActions, Sprite>();
    public List<ActionTuplets> m_actionTuplets;
    public static AIManager s_instance;

    public static AIManager GetInstance()
    {
        return s_instance;
    }

    public void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
            foreach(var action in m_actionTuplets)
            {
                m_actionSpriteDictionnary.Add(action.AIAction, action.sprite);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Sprite GetActionSprite(EAIActions action)
    {
        if (m_actionSpriteDictionnary.ContainsKey(action))
        {
            return m_actionSpriteDictionnary[action];
        }
        return null;
    }
}

namespace Turquoise
{
    public enum EAIActions
    {
        Attack,
        Defend,
        Buff,
        Debuff,
        Surprise,
        Other,
        None
    }

    [System.Serializable]
    public struct ActionTuplets
    {
        public EAIActions AIAction;
        public Sprite sprite;
    }
}
