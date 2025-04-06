using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BackgroundBehavior : MonoBehaviour
{
    [SerializeField]
    CameraBehavior cameraBehavior;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        if (cameraBehavior.GetTarget() != null)
        {
            transform.position = new Vector3(cameraBehavior.GetTarget().position.x, cameraBehavior.GetTarget().position.y, transform.position.z);
        }
    }
}
