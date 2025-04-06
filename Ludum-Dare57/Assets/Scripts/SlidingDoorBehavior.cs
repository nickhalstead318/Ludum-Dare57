using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SlidingDoorBehavior : MonoBehaviour
{

    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;

    [SerializeField] private bool active = false;
    [SerializeField] private float moveUpSpeed = 1;
    [SerializeField] private float moveDownSpeed = 1;

    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private ButtonBehavior[] buttons;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        if( boxCollider == null )
        {
            Debug.LogWarning("Door doesn't have a box collider");
        }

        if (buttons == null)
        {
            buttons = new ButtonBehavior[0];
        }
            
        if(buttons.Length == 0)
        {
            Debug.LogWarning("Door does not have any buttons!");
            active = true;
        }

        minHeight = transform.position.y;
        maxHeight = transform.position.y + boxCollider.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        //string info = string.Format("Current y: {0}, min y: {1}, max y: {2}", transform.position.y, minHeight, maxHeight);
        // Debug.Log(info);

        // CheckIfActive();

        if (active)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    public void CheckIfActive()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if (!buttons[i].GetStatus())
            {
                active = false;
                return;
            }
        }

        active = true;
    }

    private void MoveUp()
    {
        // Debug.Log("I am a door, I am moving up by " + moveUpSpeed + " if I can");
        if (transform.position.y < maxHeight)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + moveUpSpeed * Time.deltaTime, 0);
        }
    }

    private void MoveDown()
    {
        // Debug.Log("I am a door, I am moving down by " + moveDownSpeed + "if I can");
        if (transform.position.y > minHeight)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - moveDownSpeed * Time.deltaTime, 0);
        }
    }
}
