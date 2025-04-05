using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerBehavior : MonoBehaviour
{
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
}
