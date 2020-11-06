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

    public void OnHoverEnter(Animator animator)
    {
        animator.SetBool("NeedUp", true);
        animator.SetBool("NeedDown", false);
    }

    public void OnHoverExit(Animator animator)
    {
        animator.SetBool("NeedUp", false);
        animator.SetBool("NeedDown", true);
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

    public void Quit()
    {
        //Stop le jeu sur l'editeur quand on appelle la fonction
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        //Sinon quitte l'application si c'est une build
        Application.Quit();
    }
}
