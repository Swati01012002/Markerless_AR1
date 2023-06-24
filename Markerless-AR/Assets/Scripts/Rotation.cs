using UnityEngine;

public class Rotation : MonoBehaviour
{
    private bool isRotating = false;
    private Vector2 initialTouchPos;
    private float initialObjectRotationY;
    public GameObject yourPrefab;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isRotating = true;
                initialTouchPos = touch.position;
                initialObjectRotationY = yourPrefab.transform.eulerAngles.y;
            }
            else if (touch.phase == TouchPhase.Moved && isRotating)
            {
                Vector2 currentTouchPos = touch.position;
                Vector2 touchDelta = currentTouchPos - initialTouchPos;

                // Calculate the rotation amount based on touch delta
                float rotationAmount = touchDelta.x * 1f;

                // Apply the rotation to the object
                Vector3 newRotation = yourPrefab.transform.eulerAngles;
                newRotation.y = initialObjectRotationY + rotationAmount;
                yourPrefab.transform.eulerAngles = newRotation;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isRotating = false;
            }
        }
    }
}
