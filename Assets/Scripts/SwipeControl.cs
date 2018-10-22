using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeControl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private bool isDragging;
    public bool swipeUp, swipeDown;
    private Vector2 startTouch, swipeDelta;

    // LateUpdate est appelé pour chaque trame, si le Behaviour est activé
    private void LateUpdate()
    {
        swipeUp = false;
        swipeDown = false;
    }

    // Rétablir les valeurs par défaut
    private void Reset()
    {
        isDragging = false;
        startTouch = swipeDelta = Vector2.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        isDragging = true;
        startTouch = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        swipeDelta = eventData.position - startTouch;

        // Beyond deadzone
        if (swipeDelta.magnitude > 100)
        {
            if (Mathf.Abs(swipeDelta.y) > Mathf.Abs(swipeDelta.x))
            {
                swipeUp = swipeDelta.y > 0;
                swipeDown = swipeDelta.y < 0;
                //Debug.LogFormat("Up={0} Down={1}", swipeUp, swipeDown);
                Reset();
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Reset();
    }
}
