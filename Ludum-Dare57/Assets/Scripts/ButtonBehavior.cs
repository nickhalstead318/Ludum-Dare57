using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite buttonOff;
    [SerializeField] private Sprite buttonOn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();
            if(sprite == null )
            {
                Debug.LogWarning("Button object doesn't have sprite renderer component");
            }
        }

        if (buttonOff == null)
        {
            Debug.LogWarning("Button object doesn't have off state defined");
        }
        if (buttonOn == null)
        {
            Debug.LogWarning("Button object doesn't have on state defined");
        }

        sprite.sprite = buttonOff;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CharacterBehavior player = collision.gameObject.GetComponent<CharacterBehavior>();
        if (player != null)
        {
            setOn();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        CharacterBehavior player = collision.gameObject.GetComponent<CharacterBehavior>();
        if (player != null)
        {
            setOff();
        }
    }

    public void setOff()
    {
        sprite.sprite = buttonOff;
    }

    public void setOn()
    {
        sprite.sprite = buttonOn;
    }
}
