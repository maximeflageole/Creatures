using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace Cards
{
    public enum ETarget
    {
        Self,
        Enemy,
        Creature,
        Card,
        None
    }
}

public class Card : MonoBehaviour
{
    [SerializeField]
    protected uint m_manaCost = 0;
    [SerializeField]
    protected ETarget m_target = ETarget.None;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
