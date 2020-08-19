using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public Text PlayersRemaining;
    public RawImage NNetSprite;
    bool showNNet;
    GameManager gm;
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        showNNet = NNetSprite.IsActive();
    }

    void Update()
    {
    }

    public void UpdatePlayerCount()
    {
        PlayersRemaining.text = "Players Remaining: " + gm.players.Count;
    }

    public void ToggleNNetVisualizer()
    {
        showNNet = !showNNet;
        NNetSprite.gameObject.SetActive(showNNet);
    }
}
