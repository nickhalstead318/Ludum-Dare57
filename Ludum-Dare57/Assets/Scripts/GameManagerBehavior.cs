using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerBehavior : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textBoxText;
    [SerializeField] private Canvas textBoxCanvas;
    [SerializeField] private GameObject pauseMenuUI;

    public int cloneMax = 2;
    public CameraBehavior cameraBehavior;

    private Coroutine typingCoroutine;
    private string fullTextToDisplay;
    private float typingSpeed = 3.0f;
    private bool isPaused;
    private bool enteredLevel = false;


    [SerializeField]
    private GameObject exitScreenUI;
    [SerializeField]
    private float exitScreenFadeSpeed = 5f;
    [SerializeField]
    private TextMeshProUGUI wellDoneText;
    [SerializeField]
    private TextMeshProUGUI keepGoingText;
    [SerializeField]
    private Image descendButton;
    [SerializeField]
    private Image mainMenuButton;

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

        if (Input.GetKeyDown(KeyCode.Escape))
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

    public void LoadNextScene()
    {
        Time.timeScale = 1f;

        // Get the next level in the index
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        // Transition to next scene
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("Last scene reached! Restarting...");
            // Loop back to first scene
            SceneManager.LoadScene(0);
        }
    }

    public void EnterLevel()
    {
        CharacterBehavior[] players = FindObjectsByType<CharacterBehavior>(FindObjectsSortMode.None);
        cameraBehavior.UpdateTarget(players[0].transform);
        enteredLevel = true;
    }

    public bool GetEnteredLevel()
    {
        return enteredLevel;
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

    public void ShowExitScreen()
    {
        Image exitScreenImage = exitScreenUI.GetComponent<Image>();
        exitScreenImage.color = new Color(exitScreenImage.color.r, exitScreenImage.color.g, exitScreenImage.color.b, 0);
        wellDoneText.color = new Color(wellDoneText.color.r, wellDoneText.color.g, wellDoneText.color.b, 0);
        keepGoingText.color = new Color(keepGoingText.color.r, keepGoingText.color.g, keepGoingText.color.b, 0);

        descendButton.color = new Color(descendButton.color.r, descendButton.color.g, descendButton.color.b, 0);
        TextMeshProUGUI descendText = descendButton.GetComponentInChildren<TextMeshProUGUI>();
        descendText.color = new Color(descendText.color.r, descendText.color.g, descendText.color.b, 0);

        mainMenuButton.color = new Color(mainMenuButton.color.r, mainMenuButton.color.g, mainMenuButton.color.b, 0);
        TextMeshProUGUI mainMenuText = mainMenuButton.GetComponentInChildren<TextMeshProUGUI>();
        mainMenuText.color = new Color(mainMenuText.color.r, mainMenuText.color.g, mainMenuText.color.b, 0);

        exitScreenUI.SetActive(true);
        StartCoroutine(FadeInExitScreen());
    }

    IEnumerator FadeInExitScreen()
    {
        exitScreenFadeSpeed /= 100f;
        Image exitScreenImage = exitScreenUI.GetComponent<Image>();
        
        // Loop over slowly fading the different parts of the screen in
        for (float alpha = 0; alpha <= 220f; alpha += 10)
        {
            Debug.Log(alpha);
            exitScreenImage.color = new Color(exitScreenImage.color.r, exitScreenImage.color.g, exitScreenImage.color.b, alpha / 255f);
            yield return new WaitForSecondsRealtime(exitScreenFadeSpeed);
        }

        Time.timeScale = 0f;
        isPaused = true;

        for (float alpha = 0; alpha <= 220f; alpha += 10)
        {
            wellDoneText.color = new Color(wellDoneText.color.r, wellDoneText.color.g, wellDoneText.color.b, alpha / 255f);
            yield return new WaitForSecondsRealtime(exitScreenFadeSpeed);
        }

        for (float alpha = 0; alpha <= 220f; alpha += 10)
        {
            keepGoingText.color = new Color(keepGoingText.color.r, keepGoingText.color.g, keepGoingText.color.b, alpha / 255f);
            yield return new WaitForSecondsRealtime(exitScreenFadeSpeed);
        }

        TextMeshProUGUI descendText = descendButton.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI mainMenuText = mainMenuButton.GetComponentInChildren<TextMeshProUGUI>();
        for (float alpha = 0; alpha <= 220f; alpha += 10)
        {
            descendButton.color = new Color(descendButton.color.r, descendButton.color.g, descendButton.color.b, alpha / 255f);
            descendText.color = new Color(descendText.color.r, descendText.color.g, descendText.color.b, alpha / 255f);
            mainMenuButton.color = new Color(mainMenuButton.color.r, mainMenuButton.color.g, mainMenuButton.color.b, alpha / 255f);
            mainMenuText.color = new Color(mainMenuText.color.r, mainMenuText.color.g, mainMenuText.color.b, alpha / 255f);
            yield return new WaitForSecondsRealtime(exitScreenFadeSpeed);
        }
    }
}
