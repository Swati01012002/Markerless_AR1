using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceOnce : MonoBehaviour
{
    private ARPlaneManager planeManager;
    private bool hasPlaced = false;
    [SerializeField]
    private GameObject prefab;

    void Start()
    {
        planeManager = GetComponent<ARPlaneManager>();
    }

    void Update()
    {
        if (!hasPlaced && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                ARPlane plane = hit.transform.GetComponent<ARPlane>();
                if (plane != null)
                {
                    PlaceObjectOnPlane(plane);
                }
            }
        }
    }

    void PlaceObjectOnPlane(ARPlane plane)
    {
        // Disable plane detection and hide existing planes
        planeManager.enabled = false;
        foreach (var p in planeManager.trackables)
        {
            if (p != plane)
                p.gameObject.SetActive(false);
        }

        // Instantiate your 3D object at the center of the plane
        Vector3 position = plane.center;
        Quaternion rotation = Quaternion.identity;
        Instantiate(prefab, position, rotation);

        hasPlaced = true;
    }
}
