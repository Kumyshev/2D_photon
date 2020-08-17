using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviourPunCallbacks
{

    public List<GameObject> PlayerPrefabs;

    [HideInInspector]
    public bool DirectionChange;

    [HideInInspector]
    public Vector3 vector;
    void Start()
    {
        foreach (GameObject PlayerPref in this.PlayerPrefabs)
        {
            if (PlayerPref.name == PlayerPrefs.GetString("Prefs"))
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount != 2)
                {
                    Vector3 pos = new Vector3(-6, -1);
                    PhotonNetwork.Instantiate(PlayerPref.name, pos, Quaternion.identity);
                    vector=pos;
                }
                else
                {
                    Vector3 pos = new Vector3(6, -1);
                    PhotonNetwork.Instantiate(PlayerPref.name, pos, Quaternion.identity);
                    DirectionChange = true;
                    vector = pos;
                }

            }
        }

        PhotonPeer.RegisterType(typeof(Vector2Int), 123, SerializeVector2Int, DeserializeVector2Int);
    }

    void Update()
    {

    }

    public void Leave() 
    {
        PhotonNetwork.LeaveRoom();
    
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //if (newPlayer.NickName == PlayerPrefs.GetString("Names"))
        //{
        //    Debug.Log("aaaaa");
        //}

        Debug.LogFormat("Player {0} entered room", newPlayer.NickName);
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat(otherPlayer.NickName);
    }

    public static object DeserializeVector2Int(byte[] data) 
    {
        Vector2Int result = new Vector2Int();

        result.x = BitConverter.ToInt32(data, 0);
        result.y = BitConverter.ToInt32(data, 4);

        return result;
    }

    public static byte[] SerializeVector2Int(object obj) 
    {
        Vector2Int vector = (Vector2Int)obj;

        byte[] result = new byte[8];

        BitConverter.GetBytes(vector.x).CopyTo(result, 0);
        BitConverter.GetBytes(vector.y).CopyTo(result, 4);

        return result;
    }
}
