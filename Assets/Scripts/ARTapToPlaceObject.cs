using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
//using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using System;

[RequireComponent(typeof(ARRaycastManager))]

public class ARTapToPlaceObject : MonoBehaviour
{
    [SerializeField] private GameObject placementIndicator;
    [SerializeField] private GameObject objectToPlace;
    private GameObject previousObject;

    //private ARSessionOrigin arOrigin;
    private Pose placementPose;
    private ARRaycastManager raycastManager;
    private bool placementPoseIsValid = false;

    // Start is called before the first frame update
    void Start()
    {
        //arOrigin = FindObjectOfType<ARSessionOrigin>();
        raycastManager = GetComponent<ARRaycastManager>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    private void UpdatePlacementIndicator()
    {
        if(placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);

            if(placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began )
            {
                TapToPlaceObject();
            }
        }
        else
        {
            placementIndicator.SetActive(false);
        }

    }

    private void TapToPlaceObject()
    {
        Destroy(previousObject);
        previousObject = Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
        placementIndicator.SetActive(false);
        GetComponent<ARTapToPlaceObject>().enabled = false;
        GetComponent<ARPlaneManager>().enabled = false;
        //var clones = GameObject.FindGameObjectsWithTag("Plane");
        //foreach (var clone in clones)
        //{
        //    Destroy(clone);
        //}
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if(placementPoseIsValid)
        {
            placementPose = hits[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
