using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private float cameraBaseSpeed = 5.0f;
    [SerializeField] private float camSpeed;
    [SerializeField] private BackgroundBehavior background;

    [SerializeField] private float camZOffset = -10;

    public Camera mainCamera;
    public Transform target;
    private Vector3 cameraOffset;

    [SerializeField] private bool isSwitching;
    public float switchThreshhold = 0.1f;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        if(mainCamera == null )
        {
            Debug.LogWarning("Camera script attached to object without camera component");
        }


        if (target == null)
        {
            Debug.LogWarning("Camera has no target");
        }

        if (camZOffset >= 0)
        {
            Debug.LogWarning("Camera on top of scene, resetting to -10...");
            camZOffset = -10;
        }

        if (background == null)
        {
            Debug.LogWarning("Background not attached to camera");
        }

        camSpeed = cameraBaseSpeed;
        cameraOffset = new Vector3(0, 0, camZOffset);
    }

    // Called once per frame
    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 targetPosition = target.position;
        Vector3 currrentPosition = transform.position;

        float distance = Vector3.Distance(currrentPosition, targetPosition + cameraOffset);
        if (distance < switchThreshhold)
        {
            isSwitching = false;
            camSpeed = cameraBaseSpeed;
        }

        if (!isSwitching)
        {
            transform.position = targetPosition + cameraOffset;
            background.transform.position = targetPosition;
            return;
        }
        else
        {
            float step = camSpeed * Time.deltaTime;
            if (distance > 1)
            {
                step *= distance;
            }
            transform.position = Vector3.MoveTowards(currrentPosition, targetPosition + cameraOffset, step);
            background.transform.position = transform.position - cameraOffset;
            camSpeed += 0.1f;
        }
        
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void UpdateTarget(Transform newTarget)
    {
        // Update the target we are following
        isSwitching = true;
        target = newTarget;
    }
    
}
