using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsVeiw : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform content;

    [SerializeField]
    private RoomName roomName;

    private List<RoomName> roomNames = new List<RoomName>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = roomNames.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index != -1)
                {
                    Destroy(roomNames[index].gameObject);
                    roomNames.RemoveAt(index);
                }

            }
            else
            {
                int index = roomNames.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index == -1)
                {
                    RoomName name = Instantiate(roomName, content);
                    if (name != null)
                    {
                        name.SetRoomInfo(info);
                        roomNames.Add(name);
                    }
                }
            }
        }
    }

}
