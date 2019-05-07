﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardEffects : MonoBehaviour {

    [Tooltip("The angle between two neighbor cards in hand, this will be changed with different card numbers")]
    public float rotateAngle = 30.0f;
    [Tooltip("The angle between two neighbor cards will be changed with different card numbers, define card number here")]
    public List<int> cardNumForAngles = new List<int>();
    [Tooltip("The angle between two neighbor cards will be changed with different card numbers, define angle here")]
    public List<float> anglesForCardNum = new List<float>();
    [Tooltip("The center position of the hand, all cards' positions in hand will be calculated depending on this")]
    public Vector3 centerPoint = new Vector3(0.0f, -50.0f, 0.0f);
    [Tooltip("HandCards gameobject in hierarchy")]
    public GameObject handCardObj;
    [Tooltip("When the mouse is on a card in hand, the cards on its left hand will rotate (leftPushAngle - offsetAngleDelta * n) degree to left, n=0 for the nearest one, n=1 for the second nearest one...")]
    public float leftPushAngle = 2.0f;
    [Tooltip("When the mouse is on a card in hand, the cards on its right hand will rotate (rightPushAngle - offsetAngleDelta * n) degree to right, n=0 for the nearest one, n=1 for the second nearest one...")]
    public float rightPushAngle = 2.0f;
    [Tooltip("The Offset Angle Delta only can be used with leftPushAngle and rightPushAngle")]
    public float offsetAngleDelta = 0.1f;
    [Tooltip("The card will rise up to position of (0, cardOffsetY, 0) when the mouse is on")]
    public float cardOffsetY = 1.0f;
    [Tooltip("The card move speed when sent from draw pile to hand")]
    public float sendCardMoveSpeed = 20.0f;
    [Tooltip("The card scale speed when sent from draw pile to hand")]
    public float sendCardScaleSpeed = 10.0f;
    [Tooltip("The card move speed when the mouse is on (Such as rising up)")]
    public float moveSpeed = 1.0f;
    [Tooltip("The card move speed when the mouse is off (Such as falling down to the original position)")]
    public float slowMoveSpeed = 1.0f;
    [Tooltip("The card rotate speed when the mouse is on (Such as rising up)")]
    public float rotateSpeed = 1.0f;
    [Tooltip("The radius of the hand's arc shape")]
    public float centerRadius = 50.0f;
    [Tooltip("Maximum scale of the card")]
    public float cardBigScale = 0.9f;
    [Tooltip("Normal scale of the card")]
    public float cardNormalScale = 0.8f;
    [Tooltip("The normal scale change speed of the card (Such as rising up)")]
    public float scaleSpeed = 1.0f;
    [Tooltip("The slow scale change speed of the card (Such as falling down)")]
    public float slowScaleSpeed = 1.0f;
    [Tooltip("The card cannot be checked if the mouse moves on it twice with a short interval")]
    public float mouseOnInterval = 1.0f;
    [Tooltip("The card cannot be clicked if it is in Non Interact Delay state")]
    public float nonInteractDelay = 1.0f;
    [Tooltip("The card move speed in the first stage of dropping effect")]
    public float cardDropSlowSpeed = 1.0f;
    [Tooltip("The card move speed in the second stage of dropping effect")]
    public float cardDropFastSpeed = 1.0f;
    [Tooltip("The card move speed in shuffle effect")]
    public float cardShuffleFastSpeed = 1.0f;
    [Tooltip("The transform of discard pile")]
    public Transform dropCardPile;
    [Tooltip("The transform of draw pile")]
    public Transform getCardPile;
    [Tooltip("The card move speed when it is playing")]
    public float cardPlaySpeed = 1.0f;
    [Tooltip("Number of arrow parts, the head is the last one")]
    public int arrowsNum = 11;
    [Tooltip("Number of cards sent to hand automatically")]
    public int handNumAuto = 5;
    [Tooltip("The minimum card scale use by drop and shuffle effects")]
    public float miniCardScale = 0.12f;
    [Tooltip("The skill icon offset along y-axis on the head of character")]
    public float skillIconOffsetY = 2.0f;
    [Tooltip("The playing card will rise up to the position along y-axis")]
    public float cardPlayRiseDstY = 1.0f;

    //UI
    public Button drawBtn;
    public Button m_nextTurnBtn;
    public Text drawPileText;
    public Text discardPileText;
    public Text toast;

    //Sprites
    public Sprite arrowHeadGraySprite;
    public Sprite arrowBodyGraySprite;
    public Sprite arrowHeadGreenSprite;
    public Sprite arrowBodyGreenSprite;
    public Sprite arrowHeadRedSprite;
    public Sprite arrowBodyRedSprite;

    //Prefabs
    public GameObject arrowHeadPrefab;
    public GameObject arrowBodyPrefab;
    public List<GameObject> cardPrefabs;
    public GameObject attackIconPrefab;
    public GameObject defenseIconPrefab;

    // Curves
    [Tooltip("This curve defines the motion path of card during dropping card after playing card")]
    public AnimationCurve drop_card_curve;
    [Tooltip("This curve defines the motion path of cards during discarding animation")]
    public AnimationCurve clear_card_curve;
    [Tooltip("Curves in the list define the motion paths of cards during shuffling animation")]
    public List<AnimationCurve> shuffle_card_curve;

    private int lastFrameMouseOn = -1;
    private int mouseClickCard = -1;
    private int focusOnCard = -1;
    private float cardHalfSize = 0.0f;
    private bool shufflingCard = false;
    [SerializeField]
    private List<Card> playingCard = new List<Card>();
    private List<Card> shuffleCardsEffects = new List<Card>();
    private List<float> shuffleCardDelay = new List<float>();
    private float shuffleBegin;
    private GameObject focusOnPlayer = null;
    private float lastAddHandCardTime;
    [SerializeField]
    private List<Card> handCards = new List<Card>();
    private List<GameObject> arrows = new List<GameObject>();
    [SerializeField]
    private Queue<Card> drawPileCards = new Queue<Card>();
    [SerializeField]
    private Queue<Card> discardPileCards = new Queue<Card>();
    [SerializeField]
    protected Player m_player;
    protected int m_turnCount = 0;

    private const string DRAW_PILE_NUM_TEXT = "Draw Pile: ";
    private const string DISCARD_PILE_NUM_TEXT = "Discard Pile: ";
    private const string FRIEND_CHARA_NAME = "GoodEgg";
    private const string ENEMY_CHARA_NAME = "BadEgg";
    private const int HAND_CARD_LIMIT = 10;

    //Added for Turquoise Project
     [SerializeField]
     protected bool m_hasValidTarget;

    public void Initialization(Player player)
    {
        m_player = player;
        Creature creature = player.GetCurrentCreature();
        if (creature == null)
        {
            Debug.Log("CardEffects::Initialization : Creature Index Invalid!");
        }

        LoadDeckFromCreature(creature);

        // Add button click events
        drawBtn.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            AddHandCard();
        });
        m_nextTurnBtn.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            NextTurn();
        });

        // Init arrow parts, the last one is head
        for (int i = 0; i < arrowsNum; i++)
        {
            GameObject arrow = (GameObject)Instantiate(arrowBodyPrefab);
            if (i == arrowsNum - 1)
            {
                arrow = (GameObject)Instantiate(arrowHeadPrefab);
            }
            arrow.transform.position = Vector3.zero;
            arrows.Add(arrow);
        }

        // Play shuffle card animation
        ShuffleCardAnimation();
        ChangeTurn();
    }

    void Update()
    {
        // Shuffle animation has the highest priority to display
        if (shufflingCard == false)
        {
            if (mouseClickCard == -1)
            {
                CalCardsTransform();
            }
            CheckMouseRise();
            CheckMouseClickCard();
            UpdateArrows();
            PlayCard();
            GetMouseOnCharacter();
        }
    }

    private void FixedUpdate()
    {
        if (shufflingCard == false)
        {
            CardRotate();
            CardMove();
            CardScale();
            FollowMouse();
        }
        else if (shufflingCard == true)
        {
            CardShuffling();
        }
        PlayCardEffect();
        CardPlaying();
    }

    void LoadDeckFromCreature(Creature creature)
    {
        creature.LoadDeck();
        Deck deck = creature.GetDeck();
        if (deck != null)
        {
            CardList cardList = FindObjectOfType<CardList>();
            if (cardList == null)
            {
                Debug.Log("Found no card list in the scene");
                return;
            }
            foreach (var cardName in deck.m_cards)
            {
                CardData cardData = cardList.GetCardDataFromCardName(cardName);
                if (cardData != null)
                {
                    Card cardInstance = Instantiate(cardPrefabs[0].GetComponent<Card>());
                    cardInstance.SetCardData(cardData);
                    AddDrawPileCard(cardInstance);
                }
            }
        }
    }

    void AddDiscardPileCard(Card card)
    {
        discardPileCards.Enqueue(card);
        discardPileText.text = DISCARD_PILE_NUM_TEXT + discardPileCards.Count.ToString();
    }

    void AddDrawPileCard(Card card)
    {
        //card.info = cardInfo;
        card.gameObject.SetActive(false);
        drawPileCards.Enqueue(card);
        drawPileText.text = DRAW_PILE_NUM_TEXT + drawPileCards.Count.ToString();
    }

    Card GetCardFromDrawPile()
    {
        var card = drawPileCards.Dequeue();
        card.Reset();
        card.gameObject.SetActive(true);
        drawPileText.text = DRAW_PILE_NUM_TEXT + drawPileCards.Count.ToString();
        return card;
    }

    // Prepare states to play shuffle effect
    void ShuffleCardAnimation()
    {
        for(int i = 0; i < cardPrefabs.Count; ++i)
        {
            // Prepare card state to shuffle
            var curve = shuffle_card_curve[i];
            //TODO MAX WTF IS THIS
            var card = Instantiate<GameObject>(cardPrefabs[0]).GetComponent<Card>();
            card.transform.localScale = new Vector3(miniCardScale, miniCardScale, 0);
            card.transform.position = dropCardPile.position;                           // Start from discard pile 
            card.transform.Rotate(new Vector3(0, 0, Random.Range(30.0f, 90.0f)));      // Random directions
            card.GetComponent<TrailRenderer>().enabled = true;                         // Enable the trail renderer
            //card.GetComponent<SpriteRenderer>().sortingOrder = 0;
            shuffleCardsEffects.Add(card);
            // The first half of the curve list's motion paths are shorter than the second half ones
            // So the first half of the cards' delay time should be longer than the second half ones
            if (i < shuffle_card_curve.Count / 2)
                shuffleCardDelay.Add(Random.Range(0.1f, 0.2f));
            else
                shuffleCardDelay.Add(Random.Range(0.0f, 0.1f));
        }
    }

    void RefreshDrawPile()
    {
        // All cards will be sent from discard pile to draw pile
        while (discardPileCards.Count > 0)
        {
            var card = discardPileCards.Dequeue();
            AddDrawPileCard(card);
        }
        drawPileText.text = DRAW_PILE_NUM_TEXT + drawPileCards.Count.ToString();
        discardPileText.text = DISCARD_PILE_NUM_TEXT + discardPileCards.Count.ToString();
    }

    void ShuffleDrawPile()
    {
        for (int i = 0; i < drawPileCards.Count; i++)
        {
            //TODO: ON EN EST LÀ MAX!
        }
    }

    IEnumerator SendHandCards()
    {
        RefreshDrawPile();
        int cardsToDraw = Mathf.Min(drawPileCards.Count, 5);
        for (int i = 0; i < cardsToDraw; ++i)
        {
            yield return new WaitForSeconds(0.2f);
            AddHandCard();
        }
    }

    IEnumerator SendHandCards(int amount)
    {
        int cardsToDraw = Mathf.Min(drawPileCards.Count, amount);
        for (int i = 0; i < cardsToDraw; ++i)
        {
            yield return new WaitForSeconds(0.2f);
            AddHandCard();
        }
    }

    // This function is called to check if the mouse is on the character 
    private void GetMouseOnCharacter()
    {
        focusOnPlayer = null;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        m_hasValidTarget = false;
        if (hit.collider != null && focusOnCard != -1)
        {
            GameObject go = hit.collider.gameObject;
            var team = GetETeams(go);
            if ((team == Turquoise.ETeams.Enemy && handCards[focusOnCard].GetTarget() == Cards.ETarget.Enemy) || 
                (team == Turquoise.ETeams.Ally && handCards[focusOnCard].GetTarget() == Cards.ETarget.Self) ||
                ((team == Turquoise.ETeams.Enemy || team == Turquoise.ETeams.Ally) && handCards[focusOnCard].GetTarget() == Cards.ETarget.Creature))
            {
                m_hasValidTarget = true;
                focusOnPlayer = go;
                // Update arrow's color during touching characters
                for (int i = 0; i < arrowsNum; ++i)
                {
                    var arrow = arrows[i];
                    arrow.GetComponent<SpriteRenderer>().sprite = (team == Turquoise.ETeams.Enemy ? arrowBodyRedSprite : arrowBodyGreenSprite);
                    if (i == arrowsNum - 1)
                    {
                        arrow.GetComponent<SpriteRenderer>().sprite = (team == Turquoise.ETeams.Enemy ? arrowHeadRedSprite : arrowHeadGreenSprite);
                    }
                }
                return;
            }
            //ToDo: Neutral cards
            else if (team == Turquoise.ETeams.None)
            {
                //m_hasValidTarget = true;
            }
        }
        // Default color of arrow
        for (int i = 0; i < arrowsNum; i++)
        {
            var arrow = arrows[i];
            arrow.GetComponent<SpriteRenderer>().sprite = arrowBodyGraySprite;
            if (i == arrowsNum - 1)
            {
                arrow.GetComponent<SpriteRenderer>().sprite = arrowHeadGraySprite;
            }
        }
    }

    protected Turquoise.ETeams GetETeams(GameObject gameObj)
    {
        var creature = gameObj.GetComponent<Creature>();
        if (creature != null)
        {
            return creature.m_team;
        }
        return Turquoise.ETeams.None;
    }

    // This function is called when the arrow touches the character and the mouse has been clicked
    void PlayCard()
    {
        if(focusOnCard != -1 && focusOnPlayer != null && Input.GetMouseButtonUp(0) && m_hasValidTarget)
        {
            if (!m_player.CanPlayCard(handCards[focusOnCard]))
            {
                return;
            }
            // Record the character which the card skilled on
            handCards[focusOnCard].targetPlayer = focusOnPlayer;
            playingCard.Add(handCards[focusOnCard]);
            // Drop the card from hand
            DropHandCard(focusOnCard);
            focusOnCard = -1;
            mouseClickCard = -1;
            // Hide the arrow
            for (int i = 0; i < arrows.Count; ++i)
            {
                if (arrows[i].activeSelf == true)
                {
                    arrows[i].SetActive(false);
                }
            }
        }
    }

    // In card playing effect's first stage, the card rising up to the center of the screen
    void CardPlaying()
    {
        for (int i = 0; i < playingCard.Count; ++i)
        {
            if (playingCard[i] == null || playingCard[i].isPlaying == true || playingCard[i].isDropping == true) continue;
            var card = playingCard[i];
            var dstPos = new Vector3(0, cardPlayRiseDstY, 0);  // The playing card will rising up
            if ((card.gameObject.transform.position - dstPos).magnitude <= Time.fixedDeltaTime * cardPlaySpeed)
            {
                if (Time.time - card.dropDisplayTime < 0.3f)
                {
                    return;
                }
                // Prepare card state to drop
                card.gameObject.transform.position = dstPos;
                card.isPlaying = true;
                card.totalDistance = dropCardPile.position.x - dstPos.x;
                card.gameObject.GetComponent<TrailRenderer>().enabled = true;
                card.gameObject.transform.localScale = new Vector3(miniCardScale, miniCardScale, miniCardScale);
                card.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                card.gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, -120.0f));
                card.totalDistance = Mathf.Abs(card.gameObject.transform.position.x - dropCardPile.position.x);
                card.originHighY = card.gameObject.transform.position.y;
                // Display skill effect
                /*
                if (card.targetPlayer != null)
                {
                    var icon = (GameObject)Instantiate((card.targetPlayer.name == ENEMY_CHARA_NAME ? attackIconPrefab : defenseIconPrefab));
                    var target_player_pos = card.targetPlayer.transform.position;
                    icon.transform.position = new Vector3(target_player_pos.x, target_player_pos.y + skillIconOffsetY, target_player_pos.z);
                }
                */
                //Apply skill effects
                if (card.targetPlayer != null)
                {
                    var selectedCreature = card.targetPlayer.GetComponent<Creature>();
                    if (selectedCreature != null)
                    {
                        card.ApplyEffects(selectedCreature);
                    }
                }
                foreach (var effect in card.m_effects)
                {
                    if (effect.m_effect == Cards.ECardEffect.Draw)
                    {
                        StartCoroutine(SendHandCards(effect.m_value));
                    }
                }
                return;
            }
            card.gameObject.transform.position = Vector3.MoveTowards(card.gameObject.transform.position, dstPos, Time.fixedDeltaTime * cardPlaySpeed);
            card.dropDisplayTime = Time.time;
        }
    }

    // Card shuffling effect
    void CardShuffling()
    {
        for(int i = 0; i < shuffleCardsEffects.Count; ++i)
        {
            // Calculate card position on motion path by shuffle card curves
            var currentTime = Time.time;
            if (currentTime - shuffleBegin < shuffleCardDelay[i]) continue;
            var card = shuffleCardsEffects[i];
            var delta_x = Time.deltaTime * cardShuffleFastSpeed;
            var x_distance = card.transform.position.x - delta_x - getCardPile.position.x;
            var totalDistance = dropCardPile.position.x - getCardPile.position.x;
            var factor = (totalDistance - x_distance) / totalDistance;
            card.transform.position = new Vector3(card.transform.position.x - delta_x, dropCardPile.position.y + shuffle_card_curve[i].Evaluate(factor), 0);
            var cardShuffleEps = 0.5f;
            if (Mathf.Abs(getCardPile.position.x - card.transform.position.x) <= cardShuffleEps)
            {
                card.gameObject.SetActive(false);
            }
        }
        int cnt = 0;
        for (int i = 0; i < shuffleCardsEffects.Count; ++i)
        {
            if (shuffleCardsEffects[i].gameObject.activeSelf == false)
            {
                cnt += 1;
            }
        }
        if (cnt == shuffleCardsEffects.Count)
        {
            for (int i = 0; i < shuffleCardsEffects.Count; ++i)
            {
                Destroy(shuffleCardsEffects[i].gameObject);  // Destroy show cards after shuffling animation end
            }
            shuffleCardsEffects.Clear();
            shufflingCard = false;
            //StartCoroutine(SendHandCards());      // Send card to hand automatically after shuffling animation
        }
        for (int i = 0; i < shuffleCardsEffects.Count; ++i)
        {
            Destroy(shuffleCardsEffects[i].gameObject);  // Destroy show cards after shuffling animation end
        }
        shuffleCardsEffects.Clear();
        shufflingCard = false;
        StartCoroutine(SendHandCards());
    }

    // In card playing effect's second stage, the card dropping to the discard pile
    void PlayCardEffect()
    {
        var clear_flag = true;
        for (int i = 0; i < playingCard.Count; ++i)
        {
            if (playingCard[i] == null) continue;
            clear_flag = false;
            if (playingCard[i].isPlaying == false && playingCard[i].isDropping == false) continue;
            var card = playingCard[i];
            var delta_x = Time.fixedDeltaTime * cardDropFastSpeed;
            if (card.totalDistance == 0)
            {
                card.totalDistance = 0.1f;
            }
            if (card.isDropping == true)
            {
                // Calculate the card motion path by clear card curve 
                var x_distance = dropCardPile.position.x - (card.gameObject.transform.position.x + delta_x);
                var factor = (card.totalDistance - x_distance) / card.totalDistance;
                card.gameObject.transform.position = new Vector3(card.gameObject.transform.position.x + delta_x, card.originHighY + (clear_card_curve.Evaluate(factor) + 0.5f) * Mathf.Abs(card.originHighY - dropCardPile.position.y) / 0.5f, 0);
            }
            else
            {
                // Calculate the card motion path by drop card curve
                var x_distance = dropCardPile.position.x - (card.gameObject.transform.position.x + delta_x);
                var factor = (card.totalDistance - x_distance) / card.totalDistance;
                card.gameObject.transform.position = new Vector3(card.gameObject.transform.position.x + delta_x, card.originHighY + (drop_card_curve.Evaluate(factor) - 1.0f) * Mathf.Abs(card.originHighY - dropCardPile.position.y), 0);
            }
            var cardDropEps = 0.5f;
            if (Mathf.Abs(dropCardPile.position.x - card.gameObject.transform.position.x) <= cardDropEps)
            {
                // Card reached the discard pile will be destroyed
                card.gameObject.SetActive(false);
                AddDiscardPileCard(card);
                playingCard[i] = null;
                var all_destroyed = true;
                for(int j = 0; j < playingCard.Count; ++j)
                {
                    if (playingCard[j] != null) all_destroyed = false;
                }
                if (all_destroyed)
                {
                    // Play shuffle animation when the number of cards in the draw pile is 0
                    if (drawPileCards.Count == 0)
                    {
                        ShuffleCardAnimation();
                    }
                }
            }
        }
        if (clear_flag)
            playingCard.Clear();
    }

    // This function is called to rotate cards to their goal directions calculated by combined states
    void CardRotate()
    {
        for (int i = 0; i < handCards.Count; ++i)
        {
            Card card = handCards[i];
            Transform transform = card.gameObject.transform;
            if (Mathf.Abs(card.curAngle - card.targetAngle) <= Time.fixedDeltaTime * rotateSpeed)
            {
                card.curAngle = card.targetAngle;
                transform.rotation = Quaternion.Euler(0, 0, card.targetAngle);
            }
            else if (card.curAngle > card.targetAngle)
            {
                card.curAngle -= Time.fixedDeltaTime * rotateSpeed;
                transform.Rotate(0, 0, -Time.fixedDeltaTime * rotateSpeed);
            }
            else
            {
                card.curAngle += Time.fixedDeltaTime * rotateSpeed;
                transform.Rotate(0, 0, Time.fixedDeltaTime * rotateSpeed);
            }
        }
    }

    // This function is called to move cards to their goal positions calculated by combined states
    void CardMove()
    {
        for (int i = 0; i < handCards.Count; i++)
        {
            Card card = handCards[i];
            Transform transform = card.gameObject.transform;
            transform.position = new Vector3(transform.position.x, transform.position.y, card.targetPosition.z);
            if ((transform.position - card.targetPosition).magnitude <= Time.fixedDeltaTime * card.moveSpeed)
            {
                transform.position = card.targetPosition;
                card.moveSpeed = slowMoveSpeed;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, card.targetPosition, Time.fixedDeltaTime * card.moveSpeed);
            }
        }
    }

    // This function is called to scale cards to their goal size calculated by combined states
    void CardScale()
    {
        for (int i = 0; i < handCards.Count; i++)
        {
            Card card = handCards[i];
            Transform transform = card.gameObject.transform;
            if (transform.localScale.x >= card.targetScale && transform.localScale.x - card.targetScale <= Time.fixedDeltaTime * card.scaleSpeed)
            {
                transform.localScale = new Vector3(card.targetScale, card.targetScale, 0.0f);
                card.scaleSpeed = slowScaleSpeed;
            }
            else if (transform.localScale.x <= card.targetScale && card.targetScale - transform.localScale.x <= Time.fixedDeltaTime * card.scaleSpeed)
            {
                transform.localScale = new Vector3(card.targetScale, card.targetScale, 0.0f);
                card.scaleSpeed = slowScaleSpeed;
            }
            else
            {
                float scale = 0.0f;
                if (transform.localScale.x <= card.targetScale)
                {
                    scale = Mathf.Min(card.targetScale, transform.localScale.x + Time.fixedDeltaTime * card.scaleSpeed);
                }
                else if (transform.localScale.x >= card.targetScale)
                {
                    scale = Mathf.Max(card.targetScale, transform.localScale.x - Time.fixedDeltaTime * card.scaleSpeed);
                }
                transform.localScale = new Vector3(scale, scale, 0.0f);
            }
        }
    }

    // Calculate the original direction of the card in hand during no interaction
    float OriginalAngle(int idx)
    {
        float leftAngle = (handCards.Count - 1) * rotateAngle / 2;
        return leftAngle - idx * rotateAngle;
    }

    float ConvertAngleToArc(float angle)
    {
        return angle * Mathf.PI / 180;
    }

    // Calculate the card position when it falls down from a rising up state
    Vector3 FallDownPosition(int idx)
    {
        float angle = OriginalAngle(idx) + handCards[idx].offsetAngle;
        return new Vector3(centerPoint.x - centerRadius * Mathf.Sin(ConvertAngleToArc(angle)), centerPoint.y + centerRadius * Mathf.Cos(ConvertAngleToArc(angle)), 0.0f);
    }

    // Calculate the card position when it rises up by checking
    Vector3 PushUpPosition(int idx)
    {
        Vector3 fall_down_position = FallDownPosition(idx);
        return new Vector3(fall_down_position.x, cardOffsetY, -10.0f); 
    }

    // This function is called to check if the mouse has clicked on a card
    void CheckMouseClickCard()
    {
        if (Input.GetMouseButtonDown(0) && mouseClickCard == -1)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.name.StartsWith("Card:"))
            {
                mouseClickCard = int.Parse(hit.collider.gameObject.name.Split(':')[1]);
                var card = handCards[mouseClickCard];
                // Cards in non interact state can no be clicked, used to improve the performance
                if (lastFrameMouseOn != mouseClickCard || Time.time - card.nonInteractBegin < nonInteractDelay)
                {
                    mouseClickCard = -1;
                }
            }
        }
        if (mouseClickCard != -1 && Input.GetMouseButtonDown(1))
        {
            if (lastFrameMouseOn != -1)
            {
                MouseOffCard(lastFrameMouseOn);
                OffsetSideCards(lastFrameMouseOn, 0.0f, 0.0f);
                lastFrameMouseOn = -1;
            }

            var card = handCards[mouseClickCard];
            card.nonInteractBegin = Time.time;
            card.gameObject.transform.position = new Vector3(card.gameObject.transform.position.x, card.gameObject.transform.position.y, 0);
            card.moveSpeed = (card.gameObject.transform.position - FallDownPosition(mouseClickCard)).magnitude * 2 / nonInteractDelay;
            card.scaleSpeed = slowScaleSpeed;
            CalCardsTransform(true);
            mouseClickCard = -1;
            focusOnCard = -1;
        }
    }

    // Update the card directions by different card numbers
    void UpdateCardAngle()
    {
        for (int i = 0; i < cardNumForAngles.Count; ++i)
        {
            if (handCards.Count <= cardNumForAngles[i] && (i == 0 || handCards.Count > cardNumForAngles[i - 1]))
            {
                rotateAngle = anglesForCardNum[i];
            }
        }
    }

    // This function is called for preparing to add a card in hand (draw)
    void AddHandCard()
    {
        if (shufflingCard == true) return;
        if (drawPileCards.Count == 0)
        {
            RefreshDrawPile();
        }
        if (drawPileCards.Count == 0)
        {
            var text = (Text)toast.GetComponent<Text>();
            if (text.enabled == false) text.enabled = true;
            text.text = "No cards left in draw or discard pile!";
            return;
        }
        else if (handCards.Count >= HAND_CARD_LIMIT)
        {
            var text = (Text)toast.GetComponent<Text>();
            if (text.enabled == false) text.enabled = true;
            text.text = "The limit of cards in hand is " + HAND_CARD_LIMIT;
            return;
        }
        // Prepare card states for sending to hand
        Card card = GetCardFromDrawPile();
        card.moveSpeed = sendCardMoveSpeed;
        card.targetScale = cardNormalScale;
        card.scaleSpeed = sendCardScaleSpeed;
        card.nonInteractBegin = Time.time;
        var getCardPileOffsetX = 1.0f;
        card.gameObject.transform.position = new Vector3(getCardPile.position.x + getCardPileOffsetX, getCardPile.position.y, 0);  // The start position for sending card
        card.gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0);      // The start size of the card will be sent to hand
        card.gameObject.transform.parent = handCardObj.transform;
        card.gameObject.name = "Card:" + (handCards.Count).ToString();
        //card.gameObject.GetComponent<SpriteRenderer>().sortingOrder = handCards.Count; 
        handCards.Add(card);
        UpdateCardAngle();
        CalCardsTransform(true);
        cardHalfSize = card.gameObject.transform.localScale.y / 2.0f;
        lastAddHandCardTime = Time.time;
    }

    // This function is called for discarding all cards in hand
    void NextTurn()
    {
        if (shufflingCard == true) return;
        if (Time.time - lastAddHandCardTime <= 0.5f) return;
        for (int i = 0; i < handCards.Count; i++)
        {
            var card = handCards[i];
            card.isDropping = true;
            card.gameObject.GetComponent<TrailRenderer>().enabled = true;
            card.gameObject.transform.localScale = new Vector3(miniCardScale, miniCardScale, miniCardScale);
            card.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            card.gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, Random.Range(0.0f, 30.0f)));                // Every card in hand has random direction for discard effect
            card.totalDistance = Mathf.Abs(card.gameObject.transform.position.x - dropCardPile.position.x);
            card.originHighY = card.gameObject.transform.position.y;
            playingCard.Add(handCards[i]);
        }
        while(handCards.Count > 0)
        {
            DropHandCard(0);
        }
        focusOnCard = -1;
        mouseClickCard = -1;
        for (int i = 0; i < arrows.Count; i++)
        {
            if (arrows[i].activeSelf == true)
            {
                arrows[i].SetActive(false);
            }
        }
        ChangeTurn();
    }

    // This function is called for preparing to drop card after playing effect
    void DropHandCard(int idx)
    {
        if (lastFrameMouseOn != -1)
        {
            MouseOffCard(lastFrameMouseOn);
            OffsetSideCards(lastFrameMouseOn, 0.0f, 0.0f);
            lastFrameMouseOn = -1;
        }
        focusOnCard = -1;
        mouseClickCard = -1;
        handCards[idx].gameObject.GetComponent<BoxCollider2D>().enabled = false;  // Can not be touched anymore
        handCards.RemoveAt(idx);
        ReArrangeCard();
        UpdateCardAngle();
        CalCardsTransform(true);
    }

    // The card's name should be modifed by the number of cards in hand
    void ReArrangeCard()
    {
        for (int i = 0; i < handCards.Count; ++i)
        {
            handCards[i].gameObject.name = "Card:" + i.ToString();
            //handCards[i].gameObject.GetComponent<SpriteRenderer>().sortingOrder = i;
        }
    }

    // Calculate the card transform by combined states
    void CalCardsTransform(bool force_update = false)
    {
        int idx = GetMouseOnCard();

        if(idx >= -1 || force_update == true)
        {
            Card card = null;
            for (int i = 0; i < handCards.Count; i++)
            {
                if (i == idx) continue;
                card = handCards[i];
                card.targetAngle = OriginalAngle(i);
                card.targetPosition = FallDownPosition(i);
                card.gameObject.transform.position = new Vector3(card.gameObject.transform.position.x, card.gameObject.transform.position.y, 0.0f);
            }
            if (idx >= 0)
            {
                card = handCards[idx];
                card.targetPosition = PushUpPosition(idx);
                card.gameObject.transform.position = new Vector3(card.gameObject.transform.position.x, card.gameObject.transform.position.y, -10.0f);
                card.targetAngle = 0.0f;
                card.curAngle = 0.0f;
                card.gameObject.transform.rotation = Quaternion.Euler(0, 0, card.targetAngle);
            }
        }
    }

    float GetAngleByVector(float len_x, float len_y)
    {
        if (len_y == 0)
        {
            if (len_x < 0)
            {
                return 270;
            }
            else if (len_x > 0)
            {
                return 90;
            }
            return 0;
        }
        if (len_x == 0)
        {
            if (len_y >= 0)
            {
                return 0;
            }
            else if (len_y < 0)
            {
                return 180;
            }
        }

        float angle = 0;
        if (len_y > 0 && len_x > 0)
        {
            angle = 270 + Mathf.Atan2(Mathf.Abs(len_y), Mathf.Abs(len_x)) * 180 / Mathf.PI;
        }
        else if (len_y > 0 && len_x < 0)
        {
            angle = 90 - Mathf.Atan2(Mathf.Abs(len_y), Mathf.Abs(len_x)) * 180 / Mathf.PI;
        }
        else if (len_y < 0 && len_x > 0)
        {
            angle = 270 - Mathf.Atan2(Mathf.Abs(len_y), Mathf.Abs(len_x)) * 180 / Mathf.PI;
        }
        else if (len_y < 0 && len_x < 0)
        {
            angle = Mathf.Atan2(Mathf.Abs(len_y), Mathf.Abs(len_x)) * 180 / Mathf.PI + 90;
        }
        return angle;
    }

    // This function is used to calculate cards' transforms by their mutual interaction in hand from the checking card to neighbor cards one by one
    // For example, when the mouse moves on a card, it will rise up and push neighbor cards away
    // Parameters: 
    // idx: the checking card index
    // front_angle: left push angle
    // end_angle: right push angle
    void OffsetSideCards(int idx, float front_angle, float end_angle)
    {
        int front = idx-1;
        int end = idx+1;
        Card card = handCards[idx];
        card.offsetAngle = 0.0f;
        while (front>=0)
        {
            card = handCards[front];
            card.offsetAngle = front_angle;
            front_angle = Mathf.Max(0.0f, front_angle - offsetAngleDelta);  // The push strength decreases from center to left
            front--;
        }
        while(end<handCards.Count)
        {
            card = handCards[end];
            card.offsetAngle = -end_angle;
            end_angle = Mathf.Max(0.0f, end_angle - offsetAngleDelta);   // The push strength decreases from center to right
            end++;
        }
    }

    // Get the card index in hand which the mouse is on
    // idx >= 0 the mouse is on a card
    // idx == -1 the mouse is not on a card in this frame but last frame the mouse is on a card
    // idx == -2 the mouse is not on a card in this frame and last frame
    int GetMouseOnCard()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject.name.StartsWith("Card:"))
        {
            GameObject cardd = hit.collider.gameObject;
            int idx = int.Parse(hit.collider.gameObject.name.Split(':')[1]);
            if (lastFrameMouseOn != idx)
            {
                MouseOffCard(lastFrameMouseOn);
                float currentTime = Time.time;
                if (currentTime - handCards[idx].lastOnTime > mouseOnInterval && currentTime - handCards[idx].nonInteractBegin >= nonInteractDelay)
                {
                    MouseOnCard(idx);
                    OffsetSideCards(idx, leftPushAngle, rightPushAngle);
                    lastFrameMouseOn = idx;
                    handCards[idx].lastOnTime = currentTime;
                    return idx;
                }
                if (lastFrameMouseOn >= 0)
                {
                    OffsetSideCards(lastFrameMouseOn, 0.0f, 0.0f);
                }
                lastFrameMouseOn = -1;
                return -1;
            }
        }
        else if (lastFrameMouseOn != -1)
        {
            MouseOffCard(lastFrameMouseOn);
            OffsetSideCards(lastFrameMouseOn, 0.0f, 0.0f);
            lastFrameMouseOn = -1;
            return -1;
        }
        return -2;
    }

    // This function is called to change the card state which the mouse is on
    void MouseOnCard(int idx)
    {
        Card card = handCards[idx];
        GameObject cardgo = card.gameObject;
        //card.sortOrder = cardgo.GetComponent<SpriteRenderer>().sortingOrder;
        //cardgo.GetComponent<SpriteRenderer>().sortingOrder = 100;   // Move to the topest layer when a card is checking by the player
        card.targetScale = cardBigScale;
        card.moveSpeed = moveSpeed;
        card.scaleSpeed = scaleSpeed;
    }

    // This function is called to change the card state which loses the mouse focus
    void MouseOffCard(int idx)
    {
        if (idx == -1) return;
        Card card = handCards[idx];
        GameObject cardgo = card.gameObject;
        //cardgo.GetComponent<SpriteRenderer>().sortingOrder = card.sortOrder;
        card.targetScale = cardNormalScale;
        card.moveSpeed = slowMoveSpeed;
        card.scaleSpeed = slowScaleSpeed;
    }

    // When click a card, the card follows the mouse on the bottom of the screen, when the mouse moves up, the card moves to the center top of the hand
    void FollowMouse()
    {
        if (mouseClickCard != -1 && focusOnCard == -1)
        {
            var mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            handCards[mouseClickCard].gameObject.transform.position = new Vector3(mouse_pos.x, mouse_pos.y, -10.0f);
        }
    }

    // Prepare card state for rising up
    void CheckMouseRise()
    {
        if (mouseClickCard == -1) return;
        var mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mouse_pos.y >= -3.0f + cardHalfSize / 2)
        {
            var card = handCards[mouseClickCard];
            focusOnCard = mouseClickCard;
            card.nonInteractBegin = Time.time;
            card.targetAngle = 0.0f;
            card.gameObject.transform.position = new Vector3(card.gameObject.transform.position.x, card.gameObject.transform.position.y, -10.0f);
            card.targetPosition = new Vector3(0, -3.0f, -10.0f);
            card.moveSpeed = (card.gameObject.transform.position - FallDownPosition(mouseClickCard)).magnitude * 2 / nonInteractDelay;
        }
        else if (focusOnCard != -1)
        {
            if (lastFrameMouseOn != -1)
            {
                MouseOffCard(lastFrameMouseOn);
                OffsetSideCards(lastFrameMouseOn, 0.0f, 0.0f);
                lastFrameMouseOn = -1;
            }

            var card = handCards[mouseClickCard];
            card.nonInteractBegin = Time.time;
            card.gameObject.transform.position = new Vector3(card.gameObject.transform.position.x, card.gameObject.transform.position.y, 0);
            card.moveSpeed = (card.gameObject.transform.position - FallDownPosition(mouseClickCard)).magnitude * 2 / nonInteractDelay;
            card.scaleSpeed = slowScaleSpeed;
            CalCardsTransform(true);
            mouseClickCard = -1;
            focusOnCard = -1;
        }
    }

    // The joints' positions of the arrow are calculated by Bezier curve, it includes a start point, an end point and two control points
    // The formula of the curve can be expressed as follows:
    // position.x = startPointPosition.x * (1-t)^3 + 3 * controlPointAPosition.x * t * (1-t)^2 + 3 * controlPointBPosition.x * t^2 * (1-t) + endPointPosition.x * t^3;
    // position.y = startPointPosition.y * (1-t)^3 + 3 * controlPointAPosition.y * t * (1-t)^2 + 3 * controlPointBPosition.y * t^2 * (1-t) + endPointPosition.y * t^3;
    // The joint direction of the arrow can be calculated by the vector from this joint to last joint
    void UpdateArrows()
    {
        if (focusOnCard != -1)
        {
            var mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float mouse_x = mouse_pos.x;
            float mouse_y = mouse_pos.y;
            var card_pos = handCards[mouseClickCard].gameObject.transform.position;
            // The control points' positions are changed with the mouse position
            // The parameters below have been modified to best performance, no need to change!!!
            float center_x = 0.0f;
            float center_y = -4.0f;
            float t = 0.0f;
            var controlA_x = center_x - (mouse_x - center_x) * 0.3f;
            var controlA_y = center_y + (mouse_y - center_y) * 0.8f;
            var controlB_x = center_x + (mouse_x - center_x) * 0.1f;
            var controlB_y = center_y + (mouse_y - center_y) * 1.4f;
            for (int i = 0; i < arrows.Count; i++)
            {
                if (arrows[i].activeSelf == false)
                {
                    arrows[i].SetActive(true);
                }
                t = (i + 1) * 1.0f / arrows.Count;
                var transform = arrows[i].transform;
                // Bezier curve calculates the joints' positions
                var arrow_x = center_x * Mathf.Pow(1 - t, 3) + 3 * controlA_x * t * Mathf.Pow(1 - t, 2) + 3 * controlB_x * Mathf.Pow(t, 2) * (1 - t) + mouse_x * Mathf.Pow(t, 3);
                var arrow_y = center_y * Mathf.Pow(1 - t, 3) + 3 * controlA_y * t * Mathf.Pow(1 - t, 2) + 3 * controlB_y * Mathf.Pow(t, 2) * (1 - t) + mouse_y * Mathf.Pow(t, 3);
                if (i == arrows.Count - 1)
                {
                    arrows[i].transform.position = new Vector3(arrow_x, arrow_y, -20.0f);
                }
                else
                {
                    arrows[i].transform.position = new Vector3(arrow_x, arrow_y, -15.0f);
                }
                // The direction of a joint can be calculated by the vector from this joint to last joint
                arrows[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                if (i > 0)
                {
                    var len_x = arrows[i].transform.position.x - arrows[i - 1].transform.position.x;
                    var len_y = arrows[i].transform.position.y - arrows[i - 1].transform.position.y;
                    arrows[i].transform.Rotate(0, 0, GetAngleByVector(len_x, len_y));
                }
                else
                {
                    var len_x = arrows[i+1].transform.position.x - arrows[i].transform.position.x;
                    var len_y = arrows[i+1].transform.position.y - arrows[i].transform.position.y;
                    arrows[i].transform.Rotate(0, 0, GetAngleByVector(len_x, len_y));
                }
                arrows[i].transform.localScale = new Vector3(1.0f - 0.03f * (arrows.Count - 1 - i), 1.0f - 0.03f * (arrows.Count - 1 - i), 0);
            }
            drawBtn.enabled = false;
            m_nextTurnBtn.enabled = false;
        }
        else
        {
            for (int i = 0; i < arrows.Count; i++)
            {
                if (arrows[i].activeSelf == true)
                {
                    arrows[i].SetActive(false);
                }
            }
            drawBtn.enabled = true;
            m_nextTurnBtn.enabled = true;
        }
    }

    public void ChangeTurn()
    {
        m_turnCount++;
        if (m_turnCount % 2 == 0)
        {
            print("Begin Enemy's turn");
            m_player.TurnEnd();
        }
        else
        {
            print("Begin player's turn");
            m_player.TurnBegin();
            shuffleBegin = Time.time;
            shufflingCard = true;
        }
    }
}