using System;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Camera mainCamera;
    public Transform target;
    public Vector3 offset;

    private float numSteps = 20;
    private float step = 0;
    private Color startColor = new Color(255, 255, 255);
    private Color endColor = new Color(164, 121, 109);

    private float startHeight = 10;
    private float endHeight = -10;

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

        if (offset == null)
        {
            Debug.LogWarning("Camera offset not defined");
        }

        if (offset.z < 1 && offset.z > -1)
        {
            Debug.LogWarning("Confirm camera Z offset should be: " + offset.z);
        }

        numSteps = startHeight - endHeight;
    }

    // Called once per frame
    void LateUpdate()
    {
        transform.position = target.position + offset;
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void UpdateTarget(Transform newTarget)
    {
        // Update the target we are following
        target = newTarget;
    }

    
}
