using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI Text_Log;

    [SerializeField]
    private TextMeshProUGUI roomName;

    [SerializeField]
    private TextMeshProUGUI playerName;

    public SelectedCharacter selected;

    public Button createButton;

    public PlayerNames playerNames;

    [SerializeField]
    private RoomsVeiw roomsVeiw;


    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() 
    {
        Log("Connected to Master");

        PhotonNetwork.JoinLobby();
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(correctString(roomName.text)) && correctString(roomName.text).Length > 1)
        {
            createButton.interactable = true;
        }
        else
            createButton.interactable = false;
    }

    static string correctString(string name)
    {
        if (Regex.IsMatch(name, @"[0-9a-zA-Z_.@]"))
        {
            return name;
        }
        else
            return null;
    }
    public void CreateRoom() 
    {
        PhotonNetwork.CreateRoom(roomName.text, new RoomOptions { MaxPlayers = 2 });
    }

    public void JoinRoom(string joinName) 
    {
        PhotonNetwork.JoinRoom(/*roomName.text*/joinName);
    }


    public void SetName() 
    {
        PhotonNetwork.NickName = playerName.text;
    }

    public override void OnJoinedRoom()
    {
        Log("Joined the room");
        PhotonNetwork.LoadLevel("Game");
    }

    private void Log(string message) 
    {
        Debug.Log(message);
        Text_Log.text += "\n";
        Text_Log.text += message;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
