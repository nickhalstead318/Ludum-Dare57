using UnityEngine;

public class Gizmo : MonoBehaviour
{

    public bool isOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if(!isOn)
        {
            return;
        }

        Gizmos.color = Color.green;
        Vector3 boxSize = new Vector3(1.0f, 1.0f, 0.0f);
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
