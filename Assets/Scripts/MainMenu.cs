using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject titre;

    public GameObject nbPlayer;

    public GameObject credit;

    private void Start()
    {
        titre.SetActive(true);
        nbPlayer.SetActive(false);
        credit.SetActive(false);
    }

    public void SetScreeActivate(GameObject screen)
    {
        titre.SetActive(false);
        nbPlayer.SetActive(false);
        credit.SetActive(false);

        screen.SetActive(true);
    }

    public void SetNbPlayer(int i)
    {
        GameData.nbPlayer = i;
    }

    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }
}
