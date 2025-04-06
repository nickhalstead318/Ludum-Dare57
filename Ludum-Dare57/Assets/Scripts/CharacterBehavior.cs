using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBehavior : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private float moveSpeed = 5f;
    private float jumpForce = 7f;

    private bool isActivePlayer;
    private bool isActivating = false;
    private bool isClone;
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;
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

        // Get Canvas
        _cloneTimer = transform.Find("Timer");

        // Sets up the player RigidBody component
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.LogWarning("Player object is missing RigidBody component");
        }

        // Sets up the player BoxCollider2D component
        _boxCollider = GetComponent<BoxCollider2D>();
        if (_boxCollider == null)
        {
            Debug.LogWarning("Player object is missing BoxCollider2D component");
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

        // Define if this is the active player, clones start inactive and begin their death timer
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
        Collider2D[] collisions = Physics2D.OverlapBoxAll(groundCheck.position, _boxCollider.size, LayerMask.GetMask("Ground"));
        foreach (var hit in collisions)
        {
            if (hit.gameObject == gameObject) continue; // ignore self

            if (hit.CompareTag("Platform") || hit.CompareTag("Player"))
            {
                _isGrounded = true;
                return;
            }

        }

        _isGrounded = false;
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
        // Clone's count down their timer of how long until they die
        if (isClone)
        {
            // Update the timer
            timeToDestroy -= Time.deltaTime;
            if(timeToDestroy <= 0f)
            {
                // Switch active player if this clone was the active one
                if(getIsActive() || isActivating)
                {
                    _gameManager.GetNextPlayer();
                }

                // Destroy the clone
                Destroy(gameObject);
            }
            else
            {
                // Update the clone timer image to represent how much time it has left
                Transform timerFill = _cloneTimer.transform.Find("Fill");
                float fillAmount = timeToDestroy / maxTimer;
                timerFill.GetComponent<Image>().fillAmount = fillAmount;

                // Set the color based on how full the circle is
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
        // Reset the current scene. Helpful for a soft lock!
        if (Input.GetKeyDown(KeyCode.R)) {
            _gameManager.ResetScene();
        }
    }

    private void CreateClone()
    {
        // Check to see if new clones can be made, and makes one if so. Clones cannot make clones
        if (_gameManager.CanCreateClone() && !isClone && Input.GetKeyDown(KeyCode.E))
        {
            GameObject clone = Instantiate(gameObject, transform.position, Quaternion.identity);
            CharacterBehavior cloneBehavior = clone.GetComponent<CharacterBehavior>();
            cloneBehavior.isClone = true;
        }
    }

    public void Activate()
    {
        // Make sure we know that we have queued up an activation, so that if the clone dies in the meantime, we can switch again
        isActivating = true;

        // Add a slight delay to marking the clone active so that we don't get duplicative inputs
        StartCoroutine(ActivateAfterDelay(0.1f));
    }

    IEnumerator ActivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        isActivePlayer = true;
        isActivating = false;
    }

    public void Deactivate()
    {
        // Deactivate this player
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

    public bool getIsActive()
    {
        return isActivePlayer;
    }

    public bool getIsPrime()
    {
        return !isClone;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Vector3 boxSize = new Vector3(0.5f, 0.1f, 0.5f);
        Gizmos.DrawWireCube(groundCheck.position, boxSize);
    }
}
