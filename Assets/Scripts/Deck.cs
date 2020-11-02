using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public int id = 0;
    public string name = "default";
}

public class Deck : MonoBehaviour
{
    public List<Card> cards = new List<Card>();

    private void Start()
    {
        if (cards.Count == 0)
        {
            ConstructDeck();
            Debug.Log("Deck constucted");
        }

        Shuffle();
        Debug.Log("Deck shuffle");
        /*
        for (int i = 0; i < cards.Count; i++)
        {
            Debug.Log(cards[i].id);
        }
        */

        Debug.Log("card take : " + TakeCard().id);

        for (int i = 0; i < cards.Count; i++)
        {
            Debug.Log(cards[i].id);
        }
        
    }

    public void ConstructDeck()
    {
        for (int i = 0; i < 10; i++)
        {
            cards.Add(new Card());
            cards[i].id = i;
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
        int j = Random.Range(0, cards.Count);
        Card tmpCard = cards[j];
        cards.RemoveAt(j);

        return tmpCard;
    }
}
