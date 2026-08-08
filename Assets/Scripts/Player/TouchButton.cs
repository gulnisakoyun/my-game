using UnityEngine;
using UnityEngine.EventSystems;

public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float direction;
    public PlayerController player;

    public void OnPointerDown(PointerEventData eventData)
    {
        player.SetTouchInput(direction);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        player.SetTouchInput(0f);
    }
}