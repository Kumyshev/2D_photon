using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEvents : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [HideInInspector]
    public bool Pressed;

    public Sprite spriteUp;
    public Sprite spriteDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<SpriteRenderer>().sprite = spriteDown;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<SpriteRenderer>().sprite = spriteUp;
    }
}
