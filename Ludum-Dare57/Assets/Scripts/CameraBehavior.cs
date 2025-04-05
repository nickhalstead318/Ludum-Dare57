using System;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Camera mainCamera;
    public Transform target;
    public Vector3 offset;

    private int numSteps = 600;
    private int step = 0;
    private Color startColor = new Color(164, 121, 109);
    private Color endColor = new Color(51, 33, 29);

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
    }

    // Called once per frame
    void LateUpdate()
    {
        transform.position = target.position + offset;
        mainCamera.backgroundColor = CalcBackgroundColor();
    }

    private Color CalcBackgroundColor()
    {
        float currRed = startColor.r + endColor.r - startColor.r * step / numSteps;
        float currGreen = startColor.g + endColor.g - startColor.g * step / numSteps;
        float currBlue = startColor.b + endColor.b - startColor.b * step / numSteps;

        // Debug.Log(String.Format("Color info: ({0}, {1}, {2}) on step {3}", currRed, currGreen, currBlue, step));

        step = (step + 1) % numSteps;

        return new Color(currRed / 256, currGreen / 256, currBlue / 256);
    }
}
