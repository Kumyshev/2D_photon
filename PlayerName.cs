using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerName : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    public Player Player { get; private set; }

    public void PlayerInfo(Player player)
    {
        Player = player;
        nameText.text = player.NickName;
    }
}
