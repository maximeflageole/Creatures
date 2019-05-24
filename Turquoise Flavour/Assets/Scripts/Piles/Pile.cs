using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile
{
    protected Queue<Card> m_queuedPile = new Queue<Card>();

    public List<Card> GetPileAsList()
    {
        var arrayPile = m_queuedPile.ToArray();
        List<Card> listPile = new List<Card>();
        foreach (var card in arrayPile)
        {
            listPile.Add(card);
        }
        return listPile;
    }

    public Queue<Card> GetQueuedPile()
    {
        return m_queuedPile;
    }

    public void ShufflePile()
    {
        var arrayPile = m_queuedPile.ToArray();
        List<Card> listPile = new List<Card>();
        foreach (var card in arrayPile)
        {
            listPile.Add(card);
        }
        Queue<Card> shuffledDrawPile = new Queue<Card>();
        for (int i = listPile.Count; i > 0; i--)
        {
            int randomInt = Random.Range(0, i);
            shuffledDrawPile.Enqueue(listPile[randomInt]);
            listPile.RemoveAt(randomInt);
        }
        m_queuedPile.Clear();
        m_queuedPile = shuffledDrawPile;
    }

    public Card Draw()
    {
        return m_queuedPile.Dequeue();
    }

    public void Enqueue(Card card)
    {
        m_queuedPile.Enqueue(card);
    }

    public void Clear()
    {
        m_queuedPile.Clear();
    }

    public int Count()
    {
        return m_queuedPile.Count;
    }
}
