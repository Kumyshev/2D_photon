using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNames : MonoBehaviourPunCallbacks
{
    public GameObject Names;

    [SerializeField]
    private TextMeshProUGUI playerName;
    public void Selected()
    {
        PlayerPrefs.SetString("Names", playerName.text);
    }

}
