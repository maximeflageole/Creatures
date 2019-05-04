using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsComponent : MonoBehaviour
{
    [SerializeField]
    protected List<Buff> m_buffs;
    [SerializeField]
    public List<Debuff> m_debuffs;

    public void TryAddBuff(Buff buff)
    {
        switch(buff.GetBuffType())
        {
            case EBuffType.Armor:
                GetComponent<Creature>().IncrementArmor(buff.GetIntensity());
                break;
            case EBuffType.Damage:
                break;
            case EBuffType.Random:
                break;
            case EBuffType.None:
                break;
        }
    }

    public void TryAddDebuff(Debuff debuff)
    {

    }

    //WE PROBABLY WANT TO REDO ALL OF THIS FOR THE METHODS ABOVE
    public void TryAddBuff(string buff, int intensity, int duration)
    {
        if (buff == "Armor")
        {
            GetComponent<Creature>().IncrementArmor(intensity);
        }
    }
}
