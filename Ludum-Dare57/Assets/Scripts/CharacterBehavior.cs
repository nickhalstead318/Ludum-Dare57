using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private bool isActivePlayer;
    private bool isClone;
    private Rigidbody2D _rb;
    public bool _isGrounded;
    private GameManagerBehavior _gameManager;
    private float maxTimer = 10f;
    private float timeToDestroy = 100f;
    private Transform _cloneTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get Game Manager
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerBehavior>();
        _gameManager.RegisterPlayer(this);

        // Get Canvas
        _cloneTimer = transform.Find("Timer");

        // Sets up the player RigidBody component
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.LogWarning("Player object is missing RigidBody component");
        }

        // Ensures ground check (feet/bottom of player) transform is set up
        if (groundCheck == null)
        {
            Debug.LogWarning("Player object does not have a ground check transform defined");
        }

        // Ensures ground layer is defined
        if (groundCheck == null)
        {
            Debug.LogWarning("Player object does not have a ground layer defined");
        }

        // Define if this is the active player
        if (isClone)
        {
            Deactivate();
            StartCloneTimer();
        }
        else
        {
            Activate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivePlayer)
        {
            Move();
            CheckGrounded();
            Jump();
            Reset();
            CreateClone();
            SwitchPlayer();
        }
        CountdownTimer();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        _rb.linearVelocity = new Vector3(moveX * moveSpeed, _rb.linearVelocity.y, 0f);
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.05f, LayerMask.GetMask("Ground"));
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
        }
    }

    private void CountdownTimer()
    {
        if (isClone)
        {
            timeToDestroy -= Time.deltaTime;
            if(timeToDestroy <= 0f)
            {
                _gameManager.UnregisterPlayer(this);
                Destroy(gameObject);
            }
            else
            {
                Transform timerFill = _cloneTimer.transform.Find("Fill");
                float fillAmount = timeToDestroy / maxTimer;
                timerFill.GetComponent<Image>().fillAmount = fillAmount;
                if(fillAmount <= 0.25f)
                {
                    timerFill.GetComponent<Image>().color = Color.red;
                }
                else if (fillAmount <= 0.5f)
                {
                    timerFill.GetComponent<Image>().color = Color.yellow;
                }
                else
                {
                    timerFill.GetComponent<Image>().color = Color.green;
                }
            }
        }
    }

    private void Reset()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            _gameManager.ResetScene();
        }
    }

    private void CreateClone()
    {
        if (_gameManager.CanCreateClone() && Input.GetKeyDown(KeyCode.E))
        {
            GameObject clone = Instantiate(gameObject, transform.position, Quaternion.identity);
            CharacterBehavior cloneBehavior = clone.GetComponent<CharacterBehavior>();
            cloneBehavior.isClone = true;
        }
    }

    public void Activate()
    {
        StartCoroutine(ActivateAfterDelay(0.1f));
    }

    IEnumerator ActivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        isActivePlayer = true;
    }

    public void Deactivate()
    {
        isActivePlayer = false;
    }

    private void SwitchPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _gameManager.GetNextPlayer();
        }
    }

    private void StartCloneTimer()
    {
        if (isClone)
        {
            _cloneTimer.gameObject.SetActive(true);
            timeToDestroy = maxTimer;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Vector3 boxSize = new Vector3(0.5f, 0.1f, 0.5f);
        Gizmos.DrawWireCube(groundCheck.position, boxSize);
    }
}
