using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerHand{
    public Card[] card = new Card[3];
}
[System.Serializable]
public class PlayerHandImage{
    public GameObject obj;
    public Image card0;
    public Image card1;
    public Image card2;
}

public class GameManager : MonoBehaviour
{
    public int nbPlayer = 4;

    public Deck deck = new Deck();

    public PlayerHand[] playerHands;

    // UI
    public PlayerHandImage[] playerHandsImage;

    // Prefabs
    public Transform robotFactoryPos;
    public GameObject robotPrefab;

    private void Start()
    {
        for (int i = 0; i < playerHandsImage.Length; i++)
        {
            playerHandsImage[i].obj.SetActive(i < nbPlayer);
        }
        playerHands = new PlayerHand[nbPlayer];
        
        for (int i = 0; i < nbPlayer; i++)
        {
            playerHands[i] = new PlayerHand();
            playerHands[i] = new PlayerHand();
            playerHands[i] = new PlayerHand();
        }
        deck.Init();
        DistributeCards();
        UpdateUI();
    }

    private void DistributeCards()
    {
        for (int i = 0; i < nbPlayer; i++)
        {
            playerHands[i].card[0] = deck.TakeCard();
            playerHands[i].card[1] = deck.TakeCard();
            playerHands[i].card[2] = deck.TakeCard();
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < nbPlayer; i++)
        {
            playerHandsImage[i].card0.sprite = GetSprtite(playerHands[i].card[0]);
            playerHandsImage[i].card1.sprite = GetSprtite(playerHands[i].card[1]);
            playerHandsImage[i].card2.sprite = GetSprtite(playerHands[i].card[2]);
        }
    }

    private Sprite GetSprtite(Card type)
    {
        switch (type)
        {
            case Card.Shild:
                return Resources.Load<Sprite>("Sprites/Armor");
            case Card.Bomb:
                return Resources.Load<Sprite>("Sprites/Bomb");
            case Card.Giant:
                return Resources.Load<Sprite>("Sprites/Giant");
            case Card.Jump:
                return Resources.Load<Sprite>("Sprites/Jump");
            case Card.Laser:
                return Resources.Load<Sprite>("Sprites/Laser");
            case Card.Speed:
                return Resources.Load<Sprite>("Sprites/Speed");
        }
        return null;
    }

    public Card UseCard(int idPlayer, int idCard)
    {
        Card tmpCard = playerHands[idPlayer].card[idCard];
        deck.AddDiscardCard(tmpCard);
        playerHands[idPlayer].card[idCard] = deck.TakeCard();
        UpdateUI();

        return tmpCard;
    }

    public void CreateRobot(List<Card> cards)
    {
        Entity tmpRobot = Instantiate(robotPrefab, robotFactoryPos.position, Quaternion.identity).GetComponent<Entity>();
        foreach (Card item in cards)
        {
            switch (item)
            {
                case Card.Shild:
                    tmpRobot._SetTank = true;
                    break;
                case Card.Bomb:
                    break;
                case Card.Giant:
                    tmpRobot._SetGiant = true;
                    break;
                case Card.Jump:
                    break;
                case Card.Laser:
                    tmpRobot._SetMirror = true;
                    break;
                case Card.Speed:
                    tmpRobot._SetFaster = true;
                    break;
                default:
                    break;
            }
        }
    }

    private void Update()
    {
    }
}
