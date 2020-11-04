using Rewired;
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

    // rewired
    private Player[] players;

    // inputs
    private bool[] playerChois;
    private List<Card> useCards = new List<Card>();

    // path
    public List<GameObject> path;

    public bool pause;

    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        nbPlayer = GameData.nbPlayer;
    }

    private void Start()
    {
        players = new Player[nbPlayer];
        playerChois = new bool[nbPlayer];

        for (int i = 0; i < nbPlayer; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);
        }

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
            case Card.Shield:
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
        useCards.Add(tmpCard);

        return tmpCard;
    }

    public void CreateRobot()
    {
        Entity tmpRobot = Instantiate(robotPrefab, robotFactoryPos.position, Quaternion.identity).GetComponent<Entity>();

        tmpRobot._WalkingPath = path;

        for (int i = useCards.Count; i-->0;)
        { 
            switch (useCards[i])
            {
                case Card.Shield:
                    tmpRobot._BonusPlayer[i] = tmpRobot.gameObject.AddComponent<Bonus_Tank>();
                    break;
                case Card.Bomb:
                    break;
                case Card.Giant:
                    tmpRobot._BonusPlayer[i] = tmpRobot.gameObject.AddComponent<Bonus_Giant>();
                    break;
                case Card.Jump:
                    break;
                case Card.Laser:
                    tmpRobot._BonusPlayer[i] = tmpRobot.gameObject.AddComponent<Bonus_Mirror>();
                    break;
                case Card.Speed:
                    tmpRobot._BonusPlayer[i] = tmpRobot.gameObject.AddComponent<Bonus_Faster>();
                    break;
                default:
                    break;
            }
        }

        Debug.Log("return");
        return;
    }

    public void ChangePauseBool(bool newValue)
    {
        pause = newValue;
    }

    private void Update()
    {
        for(int i = players.Length; i-- > 0;)
        {
            if(players[i].GetButtonDown("Menu"))
            {
                ChangePauseBool(!pause);
            }
        }

        if(pause) { return; }

        for (int i = players.Length; i-->0;)
        {
            if (!playerChois[i])
            {
                if (players[i].GetButtonDown("X"))
                {
                    UseCard(i, 0);
                    playerChois[i] = true;
                }
                else if (players[i].GetButtonDown("Y"))
                {
                    UseCard(i, 1);
                    playerChois[i] = true;
                }
                else if (players[i].GetButtonDown("B"))
                {
                    UseCard(i, 2);
                    playerChois[i] = true;
                }
            }
        }

        for (int i = 0; i < nbPlayer; i++)
        {
            if (!playerChois[i]) return;
        }

        for (int i = 0; i < nbPlayer; i++)
        {
            playerChois[i] = false;
        }
        CreateRobot();
        useCards.Clear();
    }
}
