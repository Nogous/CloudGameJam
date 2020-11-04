using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject panelPause;
    public GameObject panelVictory;
    public GameObject panelDefeat;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void StatePanelPause(bool pause)
    {
        panelPause.SetActive(pause);
        panelVictory.SetActive(false);
        panelDefeat.SetActive(false);
    }

    public void StatePanelVictory(bool victory)
    {
        panelPause.SetActive(!victory);
        panelVictory.SetActive(victory);
        panelDefeat.SetActive(!victory);
    }

    public void StatePanelDefeat(bool defeat)
    {
        panelPause.SetActive(!defeat);
        panelVictory.SetActive(!defeat);
        panelDefeat.SetActive(defeat);
    }
}
