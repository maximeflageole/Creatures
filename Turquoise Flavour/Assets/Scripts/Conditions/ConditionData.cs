using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boon", menuName = "Boon")]
public class ConditionData : ScriptableObject
{
    public Turquoise.ECardEffect cardEffect;
    public string conditionName;
    public Sprite sprite;
    public Turquoise.EBoonDecayType boonDecayType;
    public Turquoise.EBoonDecayTime boonDecayTime;
}
