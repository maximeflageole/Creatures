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


    // States for card's transform and interaction
    public float scaleSpeed = 1.0f;
    public float targetAngle = 0.0f;
    public Vector3 targetPosition = Vector3.zero;
    public int sortOrder = 0;
    public float offsetAngle = 0.0f;
    public float targetScale = 3.0f;
    public float moveSpeed = 1.0f;
    public float lastOnTime = 0.0f;
    public float curAngle = 0.0f;
    public float nonInteractBegin = 0.0f;
    public float totalDistance = 0.0f;
    public float originHighY = 0.0f;
    public bool isPlaying = false;
    public bool isDropping = false;
    public float dropDisplayTime;
    public Dictionary<string, int> info;  // Record card's info here
    public GameObject targetPlayer;       // Record character the card skilled on

    public void Reset()
    {
        isPlaying = false;
        targetPlayer = null;
        info = null;
        totalDistance = 0.0f;
        GetComponent<TrailRenderer>().enabled = false;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
