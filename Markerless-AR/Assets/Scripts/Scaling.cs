using UnityEngine;

public class Scaling : MonoBehaviour
{
    private bool isScaling = false;
    private float initialTouchDistance;
    public GameObject yourPrefab;

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                isScaling = true;
                initialTouchDistance = Vector2.Distance(touch1.position, touch2.position);
            }
            else if ((touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved) && isScaling)
            {
                float currentTouchDistance = Vector2.Distance(touch1.position, touch2.position);
                float touchDeltaDistance = currentTouchDistance - initialTouchDistance;

                // Calculate the scale factor based on touch delta distance
                float scaleFactor = touchDeltaDistance * 0.5f;

                // Apply the scaling to the object
                Vector3 newScale = yourPrefab.transform.localScale + new Vector3(scaleFactor, scaleFactor, scaleFactor);
                yourPrefab.transform.localScale = newScale;
            }
            else if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
            {
                isScaling = false;
            }
        }
    }
}
