using UnityEngine;
using UnityEngine.EventSystems;

public class ShotArea : MonoBehaviour
    , IDragHandler
    , IPointerDownHandler
    , IPointerUpHandler
{
    SpriteRenderer sprite;
    Color target = Color.red;

    public Player _player;

    public void OnDrag(PointerEventData eventData)
    {
        _player.dragHandler();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _player.startShot();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _player.performShot();
    }
}