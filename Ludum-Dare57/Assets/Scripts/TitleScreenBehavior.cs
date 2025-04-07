using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenBehavior : MonoBehaviour
{
    private AudioSource _musicPlayer;

    // Start is called before the first frame update
    void Start()
    {
        _musicPlayer = transform.GetComponent<AudioSource>();
        _musicPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
