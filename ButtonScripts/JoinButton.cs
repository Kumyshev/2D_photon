using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoinButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [HideInInspector]
    public bool Pressed;

    private lobbyManager lobby_manager;

    [SerializeField]
    private TextMeshProUGUI joinName;
    void Start()
    {
        lobby_manager = FindObjectOfType<lobbyManager>();
    }

    void FixedUpdate()
    {
        if (Pressed)
        {
            lobby_manager.JoinRoom(joinName.text);
            Pressed = false;
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }
}
