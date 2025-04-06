using UnityEngine;

public class ExitLevelBehavior : MonoBehaviour
{
    [SerializeField]
    CameraBehavior cameraBehavior;
    GameManagerBehavior _gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != null && collider.CompareTag("Player"))
        {
            CharacterBehavior player = collider.GetComponent<CharacterBehavior>();

            if (player != null && player.getIsPrime())
            {
                cameraBehavior.UpdateTarget(null);
                _gameManager.ShowExitScreen();
            }
        }
    }
}
