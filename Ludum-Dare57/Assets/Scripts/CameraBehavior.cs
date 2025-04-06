using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Camera mainCamera;
    public Transform target;
    public Vector3 offset;
    public float lagTime = 2.0f;
    private bool isSwitching;
    public float switchThreshhold = 0.1f;
    private Coroutine switchingCoroutine;

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
        if (!isSwitching && target != null)
        {
            transform.position = target.position + offset;
        }
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void UpdateTarget(Transform newTarget)
    {
        if (switchingCoroutine != null)
        {
            StopCoroutine(switchingCoroutine);
        }

        switchingCoroutine = StartCoroutine(SmoothSwitch(newTarget));

        // Update the target we are following
        isSwitching = true;
        target = newTarget;
        // StartCoroutine(WaitToSnap());
    }

    IEnumerator SmoothSwitch(Transform newTarget)
    {
        isSwitching = true;

        Vector3 targetPos = target.position + offset;

        Vector3 startPos = transform.position;
        Vector3 endPos = newTarget.position + offset;
        float elapsed = 0f;

        while (elapsed < lagTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / lagTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        // Final snap to ensure exact position
        transform.position = newTarget.position + offset;
        target = newTarget;
        isSwitching = false;

        if (isSwitching)
        {
            Vector3 currPos = transform.position;
            float distance = Vector3.Distance(currPos, targetPos);
            float speed = distance / lagTime;
            transform.position = Vector3.MoveTowards(currPos, targetPos, speed * Time.deltaTime);

            // Check if we've reached (or are extremely close to) the target
            if (distance < switchThreshhold)
            {
                isSwitching = false;
            }
        }

        else
        {
            transform.position = target.position + offset;
        }
    }
    
}
