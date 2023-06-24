using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacement1 : MonoBehaviour
{
    public List<GameObject> arObjectsToSpawn;  // List of different 3D models
    public GameObject placementIndicator;
    private GameObject spawnedObject;
    private Pose placementPose;
    private ARRaycastManager arRaycastManager;
    private bool placementPoseIsValid = false;
    private int currentModelIndex = 0;
    private Vector2 touchStartPos;

    void Start()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        if (spawnedObject == null && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ARPlaceObject();
        }

        UpdatePlacementPose();
        UpdatePlacementIndicator();

        // Swipe gesture detection
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    break;
                case TouchPhase.Ended:
                    Vector2 touchEndPos = touch.position;
                    float swipeDistance = touchEndPos.x - touchStartPos.x;

                    if (Mathf.Abs(swipeDistance) > 50f)
                    {
                        int direction = (swipeDistance < 0) ? 1 : -1;
                        CycleModel(direction);
                    }
                    break;
            }
        }
    }

    void UpdatePlacementIndicator()
    {
        if (spawnedObject == null && placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
        }
    }

    void ARPlaceObject()
    {
        spawnedObject = Instantiate(arObjectsToSpawn[currentModelIndex], placementPose.position, placementPose.rotation);
    }

    void CycleModel(int direction)
    {
        currentModelIndex += direction;
        if (currentModelIndex < 0)
        {
            currentModelIndex = arObjectsToSpawn.Count - 1;
        }
        else if (currentModelIndex >= arObjectsToSpawn.Count)
        {
            currentModelIndex = 0;
        }

        Destroy(spawnedObject);
        spawnedObject = null;
    }
}

