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
    DetailPanel dp;
    void Start()
    {
        dp = FindObjectOfType<DetailPanel>();
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
        if (!showNNet)
            dp.ClosePanel();
    }
}
