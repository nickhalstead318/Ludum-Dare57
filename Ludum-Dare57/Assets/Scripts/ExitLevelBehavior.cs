using UnityEngine;

public class ExitLevelBehavior : MonoBehaviour
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision != null && collision.collider.CompareTag("Player"))
        {
            CharacterBehavior player = collision.collider.GetComponent<CharacterBehavior>();

            if (player != null && player.getIsPrime())
            {
                cameraBehavior.UpdateTarget(null);
            }
        }
    }
}
