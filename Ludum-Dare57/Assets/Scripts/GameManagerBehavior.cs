using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerBehavior : MonoBehaviour
{
    public int cloneMax = 2;
    public CameraBehavior cameraBehavior;

    private Coroutine typingCoroutine;
    private string fullTextToDisplay;
    [SerializeField]
    private float typingSpeed = 3.0f;
    [SerializeField]
    private TextMeshProUGUI textBoxText;
    [SerializeField]
    private Canvas textBoxCanvas;

    private bool isPaused;
    [SerializeField]
    private GameObject pauseMenuUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (typingCoroutine != null && textBoxText.text != fullTextToDisplay)
            {
                // If the user interrupts the typing, immediately display the full text
                StopCoroutine(typingCoroutine);
                textBoxText.text = fullTextToDisplay;
            }
            else if (!textBoxCanvas.gameObject.activeSelf)
            {
                // Debug for now, just shows the text if it isn't there already. Remove later
                textBoxCanvas.gameObject.SetActive(true);
                StartTyping("Something must stay behind...");
            }
            else
            {
                // If the text is already displayed, close the box
                textBoxCanvas.gameObject.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreen");
    }

    public void ResetScene()
    {
        Time.timeScale = 1f;
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

        // Update the camera to follow the right character
        cameraBehavior.UpdateTarget(players[nextIndex].transform);
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

    public void StartTyping(string message)
    {
        // Stop any current coroutines that are typing
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Set the full message and start typing it
        fullTextToDisplay = message;
        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        // Reset Text
        textBoxText.text = "";

        // Loop over slowly adding one character at a time
        foreach (char c in fullTextToDisplay)
        {
            textBoxText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
