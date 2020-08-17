using UnityEngine;
using UnityEngine.EventSystems;

public class RightButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [HideInInspector]
    public bool Pressed;


    void Start()
    {

    }

    void FixedUpdate()
    {

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
