using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveAbilityUI : MonoBehaviour
{
    [SerializeField]
    protected SpriteRenderer m_spriteRenderer;
    [SerializeField]
    protected BoxCollider2D m_boxCollider;
    [SerializeField]
    protected SpriteRenderer m_manaCostSpriteRenderer;
    [SerializeField]
    protected SpriteRenderer m_cooldownSpriteRenderer;
    [SerializeField]
    protected TextMeshPro m_manaCostText;
    [SerializeField]
    protected TextMeshPro m_cooldownText;

    private void Update()
    {
        //CardEffects cardEffects = CardEffects.GetCardEffectsInstance();
        //if (cardEffects != null)
        //{
        //    cardEffects.ClickOnAbility();
        //}
        // TODO: We are here max. I need to implement the playing of active abilities 
        //if (m_boxCollider.)
    }

    public void LoadAbility(ActiveAbilityData data)
    {
        if (data == null)
        {
            gameObject.SetActive(false);
            return;
        }
        m_spriteRenderer.sprite = data.artwork;
        m_manaCostSpriteRenderer.sprite = data.ManaCostArtwork;
        m_cooldownSpriteRenderer.sprite = data.CooldownArtwork;
        m_manaCostText.text = data.energyCost.ToString();
        if (data.cooldown == 0)
        {
            m_cooldownText.text = "-";
        }
        else
        {
            m_cooldownText.text = data.cooldown.ToString();
        }
    }

    public void SetCooldown(int cooldown)
    {
        m_cooldownText.text = cooldown.ToString();
    }
}