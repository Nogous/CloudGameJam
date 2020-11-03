using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Card
{
    Shild,
    Bomb,
    Giant,
    Jump,
    Laser,
    Speed,
}

public class Deck
{
    public List<Card> cards = new List<Card>();
    private List<Card> discard = new List<Card>();

    public void Init()
    {
        if (cards.Count == 0)
        {
            ConstructDeck();
            Debug.Log("Deck constucted");
        }

        Shuffle();
        Debug.Log("Deck shuffle");
        Debug.Log(cards.Count);
    }

    public void ConstructDeck()
    {
        for (int i = 0; i < 100; i++)
        {
            cards.Add(new Card());
            if (Random.Range(0,2)==0)
            {
                cards[i] = Card.Shild;
            }
            else
            {
                cards[i] = Card.Giant;
            }
        }
    }

    public void Shuffle()
    {
        for (int i = cards.Count; i-->0;)
        {
            int j =  Random.Range(0, i+1);
            Card tmpCard = cards[j];
            cards.RemoveAt(j);
            cards.Add(tmpCard);
        }
    }

    public Card TakeCard()
    {
        if (cards.Count == 0)
        {
            if (discard.Count == 0)
            {
                Debug.LogError("No mor card on the deck");
                return Card.Shild;
            }

            Debug.Log("Use discard card to remake a card pile");

            cards = discard;
            Debug.Log(cards.Count);
            discard = new List<Card>();
            Debug.Log(cards.Count);
            Shuffle();
        }

        int j = Random.Range(0, cards.Count);
        Card tmpCard = cards[j];
        cards.RemoveAt(j);

        return tmpCard;
    }

    public void AddDiscardCard(Card card)
    {
        discard.Add(card);
    }
}
