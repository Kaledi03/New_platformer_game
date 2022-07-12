using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    // Components
    public Transform cameraTransform;    

    // Others
    public float parallaxingFactor;
    Vector3 oldCameraPosition;
    Transform objectTransform;
    float initialX;
    

    void Start()
    {
        objectTransform = GetComponent<Transform>();
        initialX = objectTransform.position.x;
        oldCameraPosition = new Vector3(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z);
    }


    void Update()
    {
        // If the camera position changed (it moved) the object will move in the opposite direction
        if (oldCameraPosition.x != cameraTransform.position.x)
        {
            objectTransform.position = new Vector3(objectTransform.position.x + (oldCameraPosition.x-cameraTransform.position.x)*parallaxingFactor,objectTransform.position.y, objectTransform.position.z);
        }
        // Set the old position of the camera as the actual position
        oldCameraPosition = new Vector3(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z);
    }
    

    public void Reset_Position(){
        objectTransform.position = new Vector3(initialX, objectTransform.position.y, objectTransform.position.z);
    }
}
