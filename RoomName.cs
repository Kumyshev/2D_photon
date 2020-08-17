using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomName : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI roomNameText;

    public RoomInfo RoomInfo { get; private set; }
    public void SetRoomInfo(RoomInfo roomInfo) 
    {
        RoomInfo = roomInfo;
        roomNameText.text = roomInfo.Name;
    }
}
