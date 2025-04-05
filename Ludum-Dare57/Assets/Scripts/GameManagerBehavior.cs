using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerBehavior : MonoBehaviour
{
    public int cloneMax = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetScene()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(activeScene);
    }

    public void GetNextPlayer()
    {
        // Get current list of characters in the scene
        CharacterBehavior[] players = FindObjectsByType<CharacterBehavior>(FindObjectsSortMode.None);

        if (players.Length <= 1)
        {
            return;
        }

        // Find the active player and deactivate it
        int currentIndex = -1;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].getIsActive())
            {
                currentIndex = i;
                players[i].Deactivate();
                break;
            }
        }

        // Get the next character to activate
        int nextIndex = (currentIndex + 1) % players.Length;
        players[nextIndex].Activate();
    }

    public bool CanCreateClone() {
        // Get current list of characters in the scene
        CharacterBehavior[] players = FindObjectsByType<CharacterBehavior>(FindObjectsSortMode.InstanceID);
        
        // If we've reached the maximum number of clones, then we can make no more
        if(players.Length >= cloneMax + 1)
        {
            return false;
        }

        // Otherwise, clone to your heart's content
        return true;
    }
}
