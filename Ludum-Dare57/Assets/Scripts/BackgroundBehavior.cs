using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BackgroundBehavior : MonoBehaviour
{
    [SerializeField]
    CameraBehavior cameraBehavior;

    private float numSteps = 20;
    private float step = 0;
    private Color startColor = new Color(164, 121, 109);
    private Color endColor = new Color(51, 33, 29);

    private float startHeight = 10;
    private float endHeight = -10;

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
        Transform target = cameraBehavior.GetTarget();
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
            GetComponent<SpriteRenderer>().color = CalcBackgroundColor(target);
        }
    }

    private Color CalcBackgroundColor(Transform target)
    {
        step = (target.position.y - startHeight) / endHeight;
        float currRed = startColor.r + (endColor.r - startColor.r) * step / numSteps;
        float currGreen = startColor.g + (endColor.g - startColor.g) * step / numSteps;
        float currBlue = startColor.b + (endColor.b - startColor.b) * step / numSteps;

        //step = (step + 1) % numSteps;

        return new Color(currRed / 255, currGreen / 255, currBlue / 255);
    }
}
