using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSequence : MonoBehaviour
{
    [SerializeField]
    protected EBattlePhase m_currentBattlePhase;
    [SerializeField]
    protected List<LevelUpInBattle> m_levelUpsInBattle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_currentBattlePhase)
        {
            case EBattlePhase.CreaturePick:
                break;
            case EBattlePhase.BattleRounds:
                break;
            case EBattlePhase.BattleRewards:
                break;
        }
    }

    void UpdateBattleRounds()
    {
        //1: when a creature is defeated, distribute xp. When level up, add the level up to stack
    }

    public void OnBattleLevelUp(Creature creature, int levelGained)
    {
        m_levelUpsInBattle.Add(new LevelUpInBattle(creature, levelGained));
    }

    void UpdateBattleRewards()
    {
        //1: Manage Level ups 1 by 1
        UpdateLevelUps();
    }

    void UpdateLevelUps()
    {
        foreach(var levelUp in m_levelUpsInBattle)
        {
            //TODO: Do all of this way better tomorrow. Promise, ok?
        }
    }
}

public enum EBattlePhase
{
    CreaturePick,
    BattleRounds,
    BattleRewards
}

public struct LevelUpInBattle
{
    public Creature m_creature;
    public int m_levelGained;
    public LevelUpInBattle(Creature creature, int levelGained)
    {
        m_creature = creature;
        m_levelGained = levelGained;
    }
}