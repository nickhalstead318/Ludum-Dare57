using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerBehavior : MonoBehaviour
{
    private List<CharacterBehavior> allPlayers;
    private int activePlayer = 0;

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

    public void RegisterPlayer(CharacterBehavior player)
    {
        if (allPlayers == null)
        {
            allPlayers = new List<CharacterBehavior>();
        }
        allPlayers.Add(player);
    }

    public void UnregisterPlayer(CharacterBehavior player)
    {
        int index = allPlayers.IndexOf(player);

        if (index == -1)
        {
            return;
        }

        // If this player is currently active
        if (index == activePlayer)
        {
            player.Deactivate();

            // Move to next available player
            activePlayer = (activePlayer + 1) % allPlayers.Count;
            allPlayers[activePlayer].Activate();
        }
        else if (index < activePlayer)
        {
            // Adjust index if the removed player was before the current active
            activePlayer--;
        }

        allPlayers.RemoveAt(index);
    }

    public void GetNextPlayer()
    {
        if(allPlayers.Count <= 1 && activePlayer == 0) 
        {
            return;
        }

        CharacterBehavior oldplayer = allPlayers[activePlayer];

        if((activePlayer+1) >= allPlayers.Count)
        {
            activePlayer = 0;
        }
        else
        {
            activePlayer++;
        }

        oldplayer.Deactivate();
        allPlayers[activePlayer].Activate();
    }

    public bool CanCreateClone() { return true; }
}
