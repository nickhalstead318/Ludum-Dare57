using UnityEngine;

public class MusicBehavior : MonoBehaviour
{

    [SerializeField] private AudioSource[] _audioSources;
    [SerializeField] private AudioSource _currentAudioSource;
    [SerializeField] private bool _isPlaying;
    [SerializeField] private int _currentTrackNum;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        if(_audioSources == null)
        {
            _audioSources = new AudioSource[0];
            Debug.LogWarning("No audio sources linked!");
        }
    }

    public void SelectTrack(int trackNum)
    {
        if(_audioSources == null || _audioSources.Length == 0)
        {
            Debug.LogWarning("No music found!");
            return;
        }
        if(trackNum >= _audioSources.Length || trackNum < 0)
        {
            Debug.LogWarning("Invalid track number! Only " + _audioSources.Length + " tracks exist");
            return;
        }
        AudioSource selection = _audioSources[trackNum];
        if(selection = null)
        {
            Debug.LogWarning("Selection found to be null: Track #" + _audioSources.Length);
            return;
        }

        // Stop existing track
        StopMusic();

        Debug.Log("Selecting track #" + trackNum);
        // Set track and play it
        _currentAudioSource = selection;
        _currentTrackNum = trackNum;
    }

    public void StartMusic()
    {
        Debug.Log("Attempting to start music");
        if (_currentAudioSource != null)
        {
            if (_isPlaying)
            {
                Debug.Log("...stopping music before starting music");
                StopMusic();
            }

            Debug.Log("Starting music");
            _currentAudioSource.Play();
            _isPlaying = true;
        }
    }

    public void StopMusic()
    {
        Debug.Log("Attempting to stop music");
        if (_currentAudioSource != null && _isPlaying)
        {
            Debug.Log("Stopping music");
            _currentAudioSource.Stop();
            _isPlaying = false;
        }
    }

    public void PauseMusic()
    {
        Debug.Log("Attempting to pause music");
        if (_currentAudioSource != null && _isPlaying)
        {
            Debug.Log("Pausing music");
            _currentAudioSource.Pause();
            _isPlaying = false;
        }
    }

    public void ResumeMusic()
    {
        Debug.Log("Attempting to resume music");
        if (_currentAudioSource != null && !_isPlaying)
        {
            Debug.Log("Resuming music");
            _currentAudioSource.UnPause();
            _isPlaying = true;
        }
    }

    public bool IsPlaying()
    {
        return _isPlaying;
    }

    public int GetTrackNum()
    {
        return _currentTrackNum;
    }

}
