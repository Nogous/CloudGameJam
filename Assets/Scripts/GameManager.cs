using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UnityEngine.SceneManagement;

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

[System.Serializable]
public class PlayerActivationImage
{
    public Image card0;
    public Image card1;
    public Image card2;
    public Image card3;
}

public class GameManager : MonoBehaviour
{
    public Camera cam;

    public int nbPlayer = 4;

    public Deck deck = new Deck();

    public PlayerHand[] playerHands;

    // UI
    public PlayerHandImage[] playerHandsImage;

    public PlayerActivationImage[] playerActivationImage;

    // Prefabs
    public GameObject robotPrefab;

    // rewired
    private Player[] players;

    // inputs
    private bool[] playerChois;
    private List<Card> useCards = new List<Card>();

    // path
    public List<GameObject> path;

    [ReadOnly] public bool pause;

    public GameObject _prefabVFXShieldOn;

    public static GameManager instance;

    private Entity[] robots = new Entity[4];

    public int idNextScene = 0;
    public int idScene = 0;

    private int nbRobotAlive = 4;

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

            playerActivationImage[i].card0.color = new Color(255, 255, 255, 0f);
            playerActivationImage[i].card1.color = new Color(255, 255, 255, 0f);
            playerActivationImage[i].card2.color = new Color(255, 255, 255, 0f);
            playerActivationImage[i].card3.color = new Color(255, 255, 255, 0f);
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
            playerHandsImage[i].card0.sprite = GetSprite(playerHands[i].card[0]);
            playerHandsImage[i].card1.sprite = GetSprite(playerHands[i].card[1]);
            playerHandsImage[i].card2.sprite = GetSprite(playerHands[i].card[2]);

            if (i == nbPlayer - 1)
            {
                switch (nbPlayer)
                {
                    case 2:
                        if ((playerHands[i - i].card[0] != Card.None && playerHands[i - i].card[1] != Card.None && playerHands[i - i].card[2] != Card.None))
                        {
                            for (int z = 0; z < nbPlayer; z++)
                            {
                                playerHandsImage[z].card0.color = new Color(255, 255, 255, 1f);
                                playerHandsImage[z].card1.color = new Color(255, 255, 255, 1f);
                                playerHandsImage[z].card2.color = new Color(255, 255, 255, 1f);
                            }
                        }
                        break;
                    case 3:
                        if ((playerHands[i - i].card[0] != Card.None && playerHands[i - i].card[1] != Card.None && playerHands[i - i].card[2] != Card.None) && (playerHands[i - i + 1].card[0] != Card.None && playerHands[i - i + 1].card[1] != Card.None && playerHands[i - i + 1].card[2] != Card.None))
                        {
                            for (int z = 0; z < nbPlayer; z++)
                            {
                                playerHandsImage[z].card0.color = new Color(255, 255, 255, 1f);
                                playerHandsImage[z].card1.color = new Color(255, 255, 255, 1f);
                                playerHandsImage[z].card2.color = new Color(255, 255, 255, 1f);
                            }
                        }
                        break;
                    case 4:
                        if ((playerHands[i - i].card[0] != Card.None && playerHands[i - i].card[1] != Card.None && playerHands[i - i].card[2] != Card.None) && (playerHands[i - i + 1].card[0] != Card.None && playerHands[i - i + 1].card[1] != Card.None && playerHands[i - i + 1].card[2] != Card.None) && (playerHands[i - i + 2].card[0] != Card.None && playerHands[i - i + 2].card[1] != Card.None && playerHands[i - i + 2].card[2] != Card.None))
                        {
                            for (int z = 0; z < nbPlayer; z++)
                            {
                                playerHandsImage[z].card0.color = new Color(255, 255, 255, 1f);
                                playerHandsImage[z].card1.color = new Color(255, 255, 255, 1f);
                                playerHandsImage[z].card2.color = new Color(255, 255, 255, 1f);
                            }
                        }
                        break;
                }
            }
        }
    }

    private void TakeCardAndUpdateUIAfterEmptyHand(int player)
    {

        playerHands[player].card[0] = deck.TakeCard();
        playerHands[player].card[1] = deck.TakeCard();
        playerHands[player].card[2] = deck.TakeCard();

        playerHandsImage[player].card0.sprite = GetSprite(playerHands[player].card[0]);
        playerHandsImage[player].card1.sprite = GetSprite(playerHands[player].card[1]);
        playerHandsImage[player].card2.sprite = GetSprite(playerHands[player].card[2]);

        playerHandsImage[player].card0.color = new Color(155, 155, 155, 0.4f);
        playerHandsImage[player].card1.color = new Color(155, 155, 155, 0.4f);
        playerHandsImage[player].card2.color = new Color(155, 155, 155, 0.4f);

        if (player == nbPlayer - 1)
        {
            UpdateUI();
            playerHandsImage[player].card0.color = new Color(255, 255, 255, 1f);
            playerHandsImage[player].card1.color = new Color(255, 255, 255, 1f);
            playerHandsImage[player].card2.color = new Color(255, 255, 255, 1f);
        }
    }

    private Sprite GetSprite(Card type)
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
            case Card.None:
                return Resources.Load<Sprite>("Sprites/Clear");
        }
        return null;
    }

    public void AnimationUseCardOnNewRobot(int idPlayer, int idCard, Card type)
    {

        if (robots.GetValue(0) == null)
        {
            playerActivationImage[idPlayer].card0.sprite = GetSprite(type);
            Vector2 pos = playerActivationImage[idPlayer].card0.rectTransform.anchoredPosition;
            switch (idCard)
            {
                case 0:
                    playerActivationImage[idPlayer].card0.rectTransform.anchoredPosition = playerHandsImage[idPlayer].card0.rectTransform.anchoredPosition;
                    break;

                case 1:
                    playerActivationImage[idPlayer].card0.rectTransform.anchoredPosition = playerHandsImage[idPlayer].card1.rectTransform.anchoredPosition;
                    break;

                case 2:
                    playerActivationImage[idPlayer].card0.rectTransform.anchoredPosition = playerHandsImage[idPlayer].card2.rectTransform.anchoredPosition;
                    break;
            }
            playerActivationImage[idPlayer].card0.rectTransform.DOAnchorPos(pos, 0.7f)
                .OnStart(() => {
                    playerActivationImage[idPlayer].card0.color = new Color(255, 255, 255, 1f);
                });
        }
        else if (robots.GetValue(1) == null)
        {
            playerActivationImage[idPlayer].card1.sprite = GetSprite(type);
            Vector2 pos = playerActivationImage[idPlayer].card1.rectTransform.anchoredPosition;
            switch (idCard)
            {
                case 0:
                    playerActivationImage[idPlayer].card1.rectTransform.anchoredPosition = playerHandsImage[idPlayer].card0.rectTransform.anchoredPosition;
                    break;

                case 1:
                    playerActivationImage[idPlayer].card1.rectTransform.anchoredPosition = playerHandsImage[idPlayer].card1.rectTransform.anchoredPosition;
                    break;

                case 2:
                    playerActivationImage[idPlayer].card1.rectTransform.anchoredPosition = playerHandsImage[idPlayer].card2.rectTransform.anchoredPosition;
                    break;
            }
            playerActivationImage[idPlayer].card1.rectTransform.DOAnchorPos(pos, 0.7f)
                .OnStart(() => {
                    playerActivationImage[idPlayer].card1.color = new Color(255, 255, 255, 1f);
                });
        }
        else if (robots.GetValue(2) == null)
        {
            playerActivationImage[idPlayer].card2.sprite = GetSprite(type);
            Vector2 pos = playerActivationImage[idPlayer].card2.rectTransform.anchoredPosition;
            switch (idCard)
            {
                case 0:
                    playerActivationImage[idPlayer].card2.rectTransform.anchoredPosition = playerHandsImage[idPlayer].card0.rectTransform.anchoredPosition;
                    break;

                case 1:
                    playerActivationImage[idPlayer].card2.rectTransform.anchoredPosition = playerHandsImage[idPlayer].card1.rectTransform.anchoredPosition;
                    break;

                case 2:
                    playerActivationImage[idPlayer].card2.rectTransform.anchoredPosition = playerHandsImage[idPlayer].card2.rectTransform.anchoredPosition;
                    break;
            }
            playerActivationImage[idPlayer].card2.rectTransform.DOAnchorPos(pos, 0.7f)
                .OnStart(() => {
                    playerActivationImage[idPlayer].card2.color = new Color(255, 255, 255, 1f);
                });
        }
        else if (robots.GetValue(3) == null)
        {
            playerActivationImage[idPlayer].card3.sprite = GetSprite(type);
            Vector2 pos = playerActivationImage[idPlayer].card3.rectTransform.anchoredPosition;
            switch (idCard)
            {
                case 0:
                    playerActivationImage[idPlayer].card3.rectTransform.anchoredPosition = playerHandsImage[idPlayer].card0.rectTransform.anchoredPosition;
                    break;

                case 1:
                    playerActivationImage[idPlayer].card3.rectTransform.anchoredPosition = playerHandsImage[idPlayer].card1.rectTransform.anchoredPosition;
                    break;

                case 2:
                    playerActivationImage[idPlayer].card3.rectTransform.anchoredPosition = playerHandsImage[idPlayer].card2.rectTransform.anchoredPosition;
                    break;
            }
            playerActivationImage[idPlayer].card3.rectTransform.DOAnchorPos(pos, 0.7f)
                .OnStart(() => {
                    playerActivationImage[idPlayer].card3.color = new Color(255, 255, 255, 1f);
                });
        }
    }

    public Card UseCard(int idPlayer, int idCard)
    {
        if (playerHands[idPlayer].card[idCard] != Card.None)
        {
            Card tmpCard = playerHands[idPlayer].card[idCard];
            deck.AddDiscardCard(tmpCard);
            playerHands[idPlayer].card[idCard] = Card.None;

            UpdateUI();
            useCards.Add(tmpCard);

            AnimationUseCardOnNewRobot(idPlayer, idCard, tmpCard);

            return tmpCard;
        }

        return playerHands[idPlayer].card[idCard];
    }

    public void CreateRobot()
    {
        Entity tmpRobot = Instantiate(robotPrefab, path[0].transform.position, Quaternion.identity).GetComponent<Entity>();

        tmpRobot.billboard.cam = cam.transform;
        tmpRobot._WalkingPath = path;

        for (int i = useCards.Count; i-->0;)
        { 
            switch (useCards[i])
            {
                case Card.Shield:
                    tmpRobot._BonusPlayer[i] = tmpRobot.gameObject.AddComponent<Bonus_Tank>();
                    foreach (Bonus_Tank bonus in tmpRobot._BonusPlayer[i].GetComponents<Bonus_Tank>())
                    {
                        bonus._prefabVFXShieldOn = _prefabVFXShieldOn;
                    }
                    break;
                case Card.Bomb:
                    break;
                case Card.Giant:
                    tmpRobot._BonusPlayer[i] = tmpRobot.gameObject.AddComponent<Bonus_Giant>();
                    break;
                case Card.Jump:
                    tmpRobot._BonusPlayer[i] = tmpRobot.gameObject.AddComponent<Bonus_Jump>();
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

        for (int i = 0; i < robots.Length; i++)
        {
            if (robots[i] == null)
            {
                robots[i] = tmpRobot;
                return;
            }
        }
    }

    public void ChangePauseBool(bool newValue)
    {
        pause = newValue;
        if(UIManager.instance != null)
            UIManager.instance.StatePanelPause(pause);
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

        for (int i = players.Length; i-- > 0;)
        {
            if (playerHands[i].card[0] == Card.None && playerHands[i].card[1] == Card.None && playerHands[i].card[2] == Card.None)
            {
                TakeCardAndUpdateUIAfterEmptyHand(i);
            }

            if (!playerChois[i])
            {
                if (robots[robots.Length - 1] != null) return;

                if (players[i].GetButtonDown("X") && playerHands[i].card[0] != Card.None)
                {
                    UseCard(i, 0);
                    playerChois[i] = true;
                }
                else if (players[i].GetButtonDown("Y") && playerHands[i].card[1] != Card.None)
                {
                    UseCard(i, 1);
                    playerChois[i] = true;
                }
                else if (players[i].GetButtonDown("B") && playerHands[i].card[2] != Card.None)
                {
                    UseCard(i, 2);
                    playerChois[i] = true;
                }
            }

            if (players[i].GetButtonDown("ActiveRobot1") && robots[0] != null)
            {
                if (robots[0]._BonusPlayer[i] != null)
                {
                    robots[0]._BonusPlayer[i].ActiveBonus();
                    playerActivationImage[i].card0.color = new Color(155, 155, 155, 0.4f);
                }
            }
            if (players[i].GetButtonDown("ActiveRobot2") && robots[1] != null)
            {
                if (robots[0]._BonusPlayer[i] != null)
                {
                    robots[1]._BonusPlayer[i].ActiveBonus();
                    playerActivationImage[i].card1.color = new Color(155, 155, 155, 0.4f);
                }
            }
            if (players[i].GetButtonDown("ActiveRobot3") && robots[2] != null)
            {
                if (robots[0]._BonusPlayer[i] != null)
                {
                    robots[2]._BonusPlayer[i].ActiveBonus();
                    playerActivationImage[i].card2.color = new Color(155, 155, 155, 0.4f);
                }
            }
            if (players[i].GetButtonDown("ActiveRobot4") && robots[3] != null)
            {
                if (robots[0]._BonusPlayer[i] != null)
                {
                    robots[3]._BonusPlayer[i].ActiveBonus();
                    playerActivationImage[i].card3.color = new Color(155, 155, 155, 0.4f);
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

    public void RobotDeath()
    {
        nbRobotAlive--;
        if (nbRobotAlive <=0)
        {
            SceneManager.LoadScene(idScene);
        }
    }
}
