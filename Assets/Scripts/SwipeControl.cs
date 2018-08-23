using UnityEngine;

public class SwipeControl : MonoBehaviour
{
    private bool isDragging;
    public bool swipeUp, swipeDown;
    private Vector2 startTouch, swipeDelta;

    // OnMouseDown est appelé quand l'utilisateur appuie sur le bouton de la souris alors que le curseur est sur GUIElement ou Collider
    private void OnMouseDown()
    {
        //Debug.Log("OnMouseDown");
        isDragging = true;
        startTouch = Input.mousePosition;
    }

    // OnMouseDrag est appelé quand l'utilisateur clique sur un GUIElement ou Collider et maintient le bouton de la souris enfoncé
    private void OnMouseDrag()
    {
        //Debug.Log("OnMouseDrag");
        if (!isDragging) return;

        swipeDelta = (Vector2)Input.mousePosition - startTouch;

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

    // OnMouseUp est appelé quand l'utilisateur relâche le bouton de la souris
    private void OnMouseUp()
    {
        //Debug.Log("OnMouseUp Delta=" + swipeDelta + " magnitude=" + swipeDelta.magnitude);
        Reset();
    }

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
}
