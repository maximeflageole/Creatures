using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class Condition : MonoBehaviour
{
    [SerializeField]
    protected int m_duration;
    public int GetDuration() { return m_duration; }

    [SerializeField]
    protected int m_intensity;
    public int GetIntensity() { return m_intensity; }

    [SerializeField]
    protected ECardEffect m_cardEffect;
    public ECardEffect GetCardEffect() { return m_cardEffect; }

    [SerializeField]
    protected Sprite m_sprite;
    public Sprite GetSprite() { return m_sprite; }

    public Condition(ECardEffect cardEffect, int duration, int intensity, Sprite sprite = null)
    {
        m_cardEffect = cardEffect;
        m_duration = duration;
        m_intensity = intensity;
        m_sprite = sprite;
    }
}